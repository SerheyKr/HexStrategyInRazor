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


		public List<WMCell> Neighbors = new();
		private readonly List<WMCell> neighborsSendArmy = new();
		public List<WMCell> NeighborsSendArmy => new(neighborsSendArmy);
		public IEnumerable<WMCell> NeighborsInputArmy => MapReference.AllCells.Where(x => x.NeighborsSendArmy.Contains(this));
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

			} else
			{
				if (Controller == null)
				{
					this.UnitsCount += (unitsCount - 1);
					Controller = side;
					ControllerChangedAtLastTurn = true;
					neighborsSendArmy.Clear();
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
						neighborsSendArmy.Clear();
					} else
					{
						UnitsCount = 0;
						Controller = null;
						ControllerChangedAtLastTurn = true;
						neighborsSendArmy.Clear();
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
			if (Controller == null)
			{
				return;
			}

			int cellIndex = 0;

			for (; UnitsCount > 0; UnitsCount--)
			{
				neighborsSendArmy[cellIndex].AddUnitsCount(Controller, 1);
				cellIndex++;
				if (cellIndex >= neighborsSendArmy.Count)
				{
					cellIndex = 0;
				}
			}
		}

		public void ClearAllWays()
		{
			neighborsSendArmy.Clear();
		}

		public void AddNeighborsSendArmy(WMCell endCell)
		{
			if (neighborsSendArmy.Contains(endCell))
			{
				neighborsSendArmy.Remove(endCell);
			} else
			{
				neighborsSendArmy.Add(endCell);
			}
		}
	}
}