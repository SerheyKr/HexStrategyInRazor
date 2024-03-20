using HexStrategyInRazor.Generator;
using HexStrategyInRazor.Map.Data;
using Svg;
using System.Drawing;
using System.Numerics;

namespace HexStrategyInRazor.Map
{
	public class WMCell
	{
		public SvgDocument Image;
		public string ImagePath;
		public int? unitsCount => MapReference.AllUnits.FindAll(x => x.currentCell == this)?.Sum(x => x.UnitsCount);
		public Army? currentUnit
		{
			get
			{
				GroupUnits();
				return MapReference.AllUnits.Find(x => x.currentCell == this);
			}
		}

		public Player Controller;
		private readonly Color emptyColor = Color.FromArgb(0, 255, 255, 255);
		public Vector2 position;
		public List<WMCell> Neighbors = new();
		public WorldMap MapReference;

		public int GCost;
		public int FCost;
		public int HCost;
		public WMCell? CameFrom;

		public void ChangeUnitsCount(int count)
		{
			if (count > 0)
			{
				MapReference.AllUnits.Add(new Army()
				{
					currentCell = this,
					Controller = Controller,
					UnitsCount = count,
					moves = false,
				});

				GroupUnits();
			} else
			{
				if (currentUnit != null)
				{
					currentUnit.UnitsCount -= count;
				}
			}
		}

		public void GroupUnits()
		{
			var currentUnits = MapReference.AllUnits.FindAll(x => x.currentCell == this);
			List<Army> toGroup = new List<Army>();
			if (currentUnits.Count > 1)
			{
				foreach (var x in currentUnits)
				{
					if (!x.moves)
					{
						toGroup.Add(x);
					}
				}
			}
			var groupArmy = currentUnits.FindAll(x => !x.moves)[0];
			toGroup.Remove(groupArmy);

			foreach (var x in toGroup)
			{
				if (groupArmy != null)
				{
					groupArmy.UnitsCount += toGroup.Sum(x => x.UnitsCount);
				}
				MapReference.AllUnits.RemoveAll(x => toGroup.Contains(x));
			}
		}

		public void CalculateFCost()
		{
			FCost = GCost + HCost;
		}

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

		public Vector2 Position { get => position; }

		public WMCell(Vector2 position, WorldMap mapReference)
		{
			this.position = position;
			MapReference = mapReference;
		}

		public WMCellData ToData()
		{
			return new WMCellData()
			{
				unitsCount = unitsCount ?? 0,
				positionId = ID,
				controllerName = Controller == null ? "N" : Controller.PlayerName,
			};
		}

		public void EndTurn()
		{
			if (Controller is not null)
			{
				ChangeUnitsCount(1);
			}
		}
	}
}