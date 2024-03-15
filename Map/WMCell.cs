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
		public int? unitsCount => (MapReference.Units.Find(x => x.currentCell == this)?.UnitsCount);
		public Player Controller;
		private readonly Color emptyColor = Color.FromArgb(0, 255, 255, 255);
		private Vector2 position;
		public List<WMCell> Neighbors = new();
		public WorldMap MapReference;

        public int GCost;
        public int FCost;
        public int HCost;
        public WMCell CameFrom;

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
    }
}