using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.DB.Respository;
using HexStrategyInRazor.Map.Data;
using System.Drawing;
using System.Numerics;

namespace HexStrategyInRazor.Map
{
	public class WMCell
	{
		public int DbId = 0;
		public int MapDbId => MapReference.DbId;
		public int? UnitsCount = 0;
		private int cellIndex = 0;

		public Player? Controller;
		public string ControllerId
		{
			get
			{
				if (Controller == null)
					return "";
				else
					return Controller.PlayerId;
			}
		}

		private Vector2 position;
		public Vector2 Position { get => position; }


		public List<WMCell> Neighbors = new();
		//private List<WMCell> neighborsSendArmy = new();
		//private List<int> neighborsSendArmyIds = new();

		private Dictionary<WMCell, int> neighborsSendArmy = new();


		public List<WMCell> NeighborsSendArmy => new(neighborsSendArmy.Keys.ToList());
		public IEnumerable<WMCell> NeighborsInputArmy => MapReference.AllCells.Where(x => x.NeighborsSendArmy.Contains(this));

		private WorldMap MapReference;
		private WMRow RowReference;
		public bool ControllerChangedAtLastTurn = false;
		public bool IsControlledByBot => Controller is AI;


		#region A*
		public int GCost;
		public int FCost;
		public int HCost;
		public WMCell? CameFrom;

		public void CalculateFCost()
		{
			FCost = GCost + HCost;
		}

		#endregion

		public string ID
		{
			get
			{
				return $"CoordsX{Position.X}Y{Position.Y}";
			}
		}

		public Color CellColor
		{
			get
			{
				if (Controller == null)
					return Color.FromArgb(120, 0, 0, 0);

				return Controller.PlayerColor;
			}
		}

		public string ControllerName
		{
			get
			{
				if (Controller == null)
					return "N";

				return Controller.PlayerName;
			}
		}

		public string CellColorHTML
		{
			get
			{
				if (Controller == null)
					return "#FFFFFF";
				return ColorTranslator.ToHtml(Controller.PlayerColor);
			}
		}

		public WMCell(Vector2 position, WorldMap mapReference, WMRow rowReference)
		{
			this.position = position;
			MapReference = mapReference;
			RowReference = rowReference;
		}

		public WMCell()
		{

		}

		public WMCellData ToData()
		{
			return new WMCellData()
			{
				positionX = (int)position.X,
				positionY = (int)position.Y,
				sendArmyToPositionX = NeighborsSendArmy.Select(x => x.position).Select(x => (int)x.X).ToList(),
				sendArmyToPositionY = NeighborsSendArmy.Select(x => x.position).Select(x => (int)x.Y).ToList(),
				unitsCount = UnitsCount ?? 0,
				positionId = ID,
				controllerName = Controller == null ? "N" : Controller.PlayerName,
				cellColorHTML = CellColorHTML,
			};
		}

		public void AddUnitsCount(Player side, int unitsCount)
		{
			if (side == Controller)
			{
				this.UnitsCount += unitsCount;

			}
			else
			{
				if (Controller == null)
				{
					this.UnitsCount += (unitsCount - 1);
					Controller = side;
					ControllerChangedAtLastTurn = true;
					ClearAllWays();
				}
				else
				{
					if (this.UnitsCount - unitsCount > 0)
					{
						this.UnitsCount -= unitsCount;
					}
					else if (this.UnitsCount - unitsCount < 0)
					{
						this.UnitsCount = unitsCount - this.UnitsCount;
						Controller = side;
						ControllerChangedAtLastTurn = true;
						ClearAllWays();
					}
					else
					{
						UnitsCount = 0;
						Controller = null;
						ControllerChangedAtLastTurn = true;
						ClearAllWays();
					}

				}
			}
		}

		public void EndAfter()
		{
			if (Controller is not null)
			{
				UnitsCount++;
			}
		}

		public void EndTurn()
		{
			if (Controller is not null && neighborsSendArmy.Count > 0)
			{
				SpentLeftUnits();
			}
			ControllerChangedAtLastTurn = false;
		}

		private void SpentLeftUnits()
		{
			if (cellIndex >= neighborsSendArmy.Count)
			{
				cellIndex = 0;
			}

			for (; UnitsCount > 0; UnitsCount--)
			{
				neighborsSendArmy.Keys.ElementAt(cellIndex).AddUnitsCount(Controller, 1);
				cellIndex++;
				if (cellIndex >= neighborsSendArmy.Count)
				{
					cellIndex = 0;
				}
			}
		}

		public void ClearAllWays()
		{
			neighborsSendArmy.Values.ToList().ForEach(async id => await new PathRepository(Program.GetContext()).DeleteById(id));

			neighborsSendArmy.Clear();
		}

		public async Task AddNeighborsSendArmyAsync(WMCell endCell)
		{
			if (neighborsSendArmy.TryGetValue(endCell, out int x))
			{
				await new PathRepository(Program.GetContext()).DeleteById(x);
				neighborsSendArmy.Remove(endCell);
			}
			else
			{
				var path = new PathModel()
				{
					CellFromdID = this.DbId,
					CellToID = endCell.DbId,
				};
				await new PathRepository(Program.GetContext()).Add(path);

				neighborsSendArmy.Add(endCell, path.Id);
			}
		}

		public async Task AddNeighborsSendArmy(WMCell endCell)
		{
			if (neighborsSendArmy.TryGetValue(endCell, out int x))
			{
				await new PathRepository(Program.GetContext()).DeleteById(x);
				neighborsSendArmy.Remove(endCell);
			}
			else
			{
				var path = new PathModel()
				{
					CellFromdID = this.DbId,
					CellToID = endCell.DbId,
				};
				await new PathRepository(Program.GetContext()).Add(path);
				neighborsSendArmy.Add(endCell, path.Id);
			}
		}

		private CellModel Model;

		public static WMCell Load(CellModel model, WorldMap mapRef, WMRow rowRef)
		{
			return new WMCell()
			{
				position = new Vector2(model.CellPositionX, model.CellPositionY),
				DbId = model.Id,
				UnitsCount = model.UnitsCount,
				RowReference = rowRef,
				MapReference = mapRef,
				Controller = mapRef.Players.Find(x => x.PlayerId == model.ControllerId),
				Model = model,
				cellIndex = model.CellIndex,
			};
		}

		public void LoadPathes(WorldMap mapRef)
		{
			Model.Paths.FindAll(x => x.CellFromdID == DbId).ForEach(x =>
			{
				var cell = mapRef.AllCells.Find(cell => cell.DbId == x.CellToID)!;
				if (!neighborsSendArmy.ContainsKey(mapRef.AllCells.Find(cell => cell.DbId == x.CellToID)!))
					neighborsSendArmy.Add(cell, x.Id);
			});
		}

		public CellModel ToDBData()
		{
			List<PathModel> paths = new List<PathModel>();
			if (DbId != 0)
				paths = neighborsSendArmy.Where(x => x.Key.DbId != 0).Select(x => new PathModel()
				{
					CellFromdID = this.DbId,
					CellToID = x.Key.DbId,
					Id = x.Value,
				}).ToList();

			return new CellModel()
			{
				CellPositionX = (int)position.X,
				CellPositionY = (int)position.Y,
				Id = DbId,
				RowId = RowReference.DbId,
				Paths = paths,
				UnitsCount = UnitsCount ?? 0,
				ControllerId = ControllerId,
				CellIndex = cellIndex,
			};
		}
	}
}