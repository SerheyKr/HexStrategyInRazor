using HexStrategyInRazor.Generator;
using HexStrategyInRazor.Map.Data;
using Svg;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace HexStrategyInRazor.Map
{
	public class WMCell(Vector2 position, WorldMap mapReference)
	{
		public int? UnitsCount = 0;

		public Player? Controller;
		private readonly Color emptyColor = Color.FromArgb(0, 255, 255, 255);
		private Vector2 position = position;
		public Vector2 Position { get => position; }

		public List<WMCell> Neighbors = new ();
		public List<WMCell> NeighborsSendArmy = new ();
		//public IEnumerable<WMCell> NeighborsIncomeArmy => MapReference.AllCells.Where(x => x.NeighborsSendArmy.Contains(this));
		//public IEnumerable<WMCell> NeighborsAllIncomeArmy
		//{
		//	get
		//	{
		//		return MapReference.AllCells.Where(x => x.NeighborsSendArmy.Contains(this));
		//	}
		//}

		//public int Priority => NeighborsIncomeArmy.Count();

		private WorldMap MapReference = mapReference;
		public bool ControllerChangedAtLastTurn = false;


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
					return "None";

				return Controller.PlayerName;
			}
		}

		public string CellColorHTML
		{
			get
			{
				if (Controller == null)
					return ColorTranslator.ToHtml(emptyColor);
				return ColorTranslator.ToHtml(Controller.PlayerColor);
			}
		}

		public WMCellData ToData()
		{
			return new WMCellData()
			{
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

			} else
			{
				if (Controller == null)
				{
					this.UnitsCount += (unitsCount - 1);
					Controller = side;
					ControllerChangedAtLastTurn = true;
					NeighborsSendArmy.Clear();
				} else
				{
					if (this.UnitsCount - unitsCount > 0)
					{
						this.UnitsCount -= unitsCount;
					} else if (this.UnitsCount - unitsCount < 0)
					{
						this.UnitsCount = unitsCount - this.UnitsCount;
						Controller = side;
						ControllerChangedAtLastTurn = true;
						NeighborsSendArmy.Clear();
					} else
					{
						UnitsCount = 0;
						Controller = null;
						ControllerChangedAtLastTurn = true;
						NeighborsSendArmy.Clear();
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
			if (Controller is not null && NeighborsSendArmy.Count > 0)
			{
				int splitCount = UnitsCount.GetValueOrDefault() / NeighborsSendArmy.Count;
				if (splitCount == 0)
				{
					return;
				}

				foreach (var x in NeighborsSendArmy)
				{
					x.AddUnitsCount(Controller, splitCount);
				}

				UnitsCount = UnitsCount.GetValueOrDefault() % NeighborsSendArmy.Count;
				int cellIndex = 0;

				for (; UnitsCount > 0; UnitsCount--)
				{
					NeighborsSendArmy[0].AddUnitsCount(Controller, 1);
					cellIndex++;
					if (cellIndex >= NeighborsSendArmy.Count)
					{
						cellIndex = 0;
					}
				}
			}
			ControllerChangedAtLastTurn = false;
		}

		public void ClearAllWays()
		{
			NeighborsSendArmy.Clear();
		}

		public void AddNeighborsSendArmy(WMCell endCell)
		{
			if (NeighborsSendArmy.Contains(endCell))
			{
				NeighborsSendArmy.Remove(endCell);
			} else
			{
				NeighborsSendArmy.Add(endCell);
			}
		}
	}
}