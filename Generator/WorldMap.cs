using Svg;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;

namespace WebApplication1.Generator
{
	public class WorldMap
	{
		public List<WMRow> Rows = new List<WMRow>();
		public List<Player> players = new List<Player>();

		private WorldMap() 
		{
			
		}

		private void GenerateBasicCells(Vector2 sizes)
		{
			for (int i = 0; i < sizes.X; i++)
			{
				var nr = new WMRow();
				Rows.Add(nr);
				for (int j = 0; j < sizes.Y; j++)
				{
					var nc = new WMCell(new Vector2(i, j));
					nr.Cells.Add(nc);
				}
			}
		}

		private void SetNeighbors()
		{
			for (int i = 0; i < Rows.Count; i++)
			{
				var nr = Rows[i];
				for (int j = 0; j < nr.Cells.Count; j++)
				{
					var nc = nr.Cells[i];

					for (int x = -1; x < 1; x++)
						for (int y = -1; y < 1; y++)
						{
							if (x != 0 && y != 0 &&
								i + x > 0 && j + y > 0 &&
								i + x < Rows.Count && j + y < Rows[i + x].Cells.Count)
							{
								nc.neighbors.Add(Rows[i + x].Cells[j + y]);
							}
						}
				}
			}
		}

		private void PlacePlayers(List<Player> players)
		{
			foreach (var np in players)
			{
				np.playerColor = Color.Red;
				WMCell randomCell;
				do
				{
					randomCell = Rows.PickRandom().Cells.PickRandom();
				} while (randomCell.controller != null);

				randomCell.controller = np;
				randomCell.unitsCount = 1;
				randomCell.buildingsCount = 1;
			}
		}

		public static WorldMap CreateMap(Vector2 sizes, List<Player> players)
		{
			WorldMap map = new WorldMap();
			map.GenerateBasicCells(sizes);
			map.PlacePlayers(players);
			map.SetNeighbors();

			return map;
		}
	}
}
