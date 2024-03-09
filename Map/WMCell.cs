using HexStrategyInRazor.Generator;
using Svg;
using System.Drawing;
using System.Numerics;

namespace HexStrategyInRazor.Map
{
	public class WMCell
	{
		public SvgDocument Image;
		public string ImagePath;
		public int unitsCount = 0;
		public int buildingsCount = 0;
		public int defenceCount = 0;
		public Player controller;
		private readonly Color emptyColor = Color.FromArgb(120, 0, 0, 0);
		private Vector2 position;
		public List<WMCell> neighbors = new();

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
				if (controller == null)
					return Color.FromArgb(120, 0, 0, 0);

				return controller.PlayerColor;
			}
		}

		public string ControllerName
		{
			get
			{
				if (controller == null)
					return "None";

				return controller.PlayerName;
			}
		}

		public string CellColorHTML
		{
			get
			{
				if (controller == null)
					return ColorTranslator.ToHtml(emptyColor);
				return ColorTranslator.ToHtml(controller.PlayerColor);
			}
		}

		public Vector2 Position { get => position; }

		public WMCell(Vector2 position)
		{
			this.position = position;
		}

		public WMCellData ToData()
		{
			return new WMCellData()
			{
				buildingsCount = buildingsCount,
				defenceCount = defenceCount,
				unitsCount = unitsCount,
				positionId = ID,
				//controllerId = controller?.PlayerId,
				controllerName = controller == null ? "N" : controller.PlayerName,
			};
		}
	}
}