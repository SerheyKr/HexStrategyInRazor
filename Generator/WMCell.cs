using Svg;
using System.Drawing;
using System.Numerics;

namespace WebApplication1.Generator
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
				return $"CoordsX{Position.X.ToString()}Y{Position.Y}";
			}
		}

		public Color CellColor
		{
			get
			{
				if (controller == null)
					return Color.FromArgb(120, 0, 0, 0);
				
				return controller.playerColor;
			}
		}

		public string ControllerName
		{
			get
			{
				if (controller == null)
					return "None";
				
				return controller.playerName;
			}
		}

		public string CellColorHTML
		{
			get
			{
				if (controller == null)
					return ColorTranslator.ToHtml(emptyColor);
				return ColorTranslator.ToHtml(controller.playerColor);
			}
		}

		public Vector2 Position { get => position;}

		public WMCell(Vector2 position)
		{
			this.position = position;
		}
	}

	public class WMCellData
	{
		public int unitsCount = 0;
		public int buildingsCount = 0;
		public int defenceCount = 0;
		public int positionX;
		public int positionY;
	}
}