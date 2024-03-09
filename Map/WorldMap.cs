using HexStrategyInRazor.Generator;
using HexStrategyInRazor.Managers;
using System.Drawing;
using System.Numerics;

namespace HexStrategyInRazor.Map
{
    public class WorldMap
	{
		public List<WMRow> Rows = new List<WMRow>();
		public List<Player> Players = new List<Player>();
		public string HostId;
		public Vector2 Sizes;

		private WorldMap()
		{
			WorldMapManager.WorldMaps.Add(this);
		}

		private void GenerateBasicCells(Vector2 sizes)
		{
			Sizes = sizes;

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
					var nc = nr.Cells[j];

					int[,] coordsToCheck;
					if (j % 2 == 0)
					{
						coordsToCheck = new int[,]
						{
							{ +1, 0 }, { +1, -1 }, { 0, -1 },
							{ -1, 0 }, { -1, +1 }, { 0, +1 }
						};
					} else
					{
						coordsToCheck = new int[,]
						{
							{ +1, 0 }, { +1, -1 }, { 0, -1 },
							{ -1, 0 }, { -1, +1 }, { 0, +1 },
							{ +1, +1 }, { -1, -1 }
						};
					}

					for (int coordsToCheckI = 0; coordsToCheckI < coordsToCheck.Length / 2; coordsToCheckI++)
					{
						var x = i + coordsToCheck[coordsToCheckI, 0];
						var y = j + coordsToCheck[coordsToCheckI, 1];

						if (x >= 0 && y >= 0 &&
						x < Rows.Count && y < Rows[x].Cells.Count)
							nc.neighbors.Add(Rows[x].Cells[y]);
					}
				}
			}
		}

		private void PlacePlayers(List<Player> players)
		{
			Players = players;

			foreach (var np in Players)
			{
				np.PlayerColor = Color.Red;
				WMCell randomCell;
				do
				{
					randomCell = Rows.PickRandom().Cells.PickRandom();
				} while (randomCell.controller != null);

				randomCell.controller = np;
				randomCell.buildingsCount = 1;
			}
		}

		public static WorldMap CreateMap(Vector2 sizes, List<Player> players)
		{
			WorldMap map = new WorldMap();
			map.GenerateBasicCells(sizes);
			map.PlacePlayers(players);
			map.SetNeighbors();

			map.HostId = players.Find(x => x.IsMainPlayer).User.UserId;

			return map;
		}

		public void Tick()
		{
			
		}

		public WorldMapData ToData()
		{
			List<WMRowData> x = new List<WMRowData>();

			x.AddRange(Rows.Select(x => x.ToData()));

			return new WorldMapData()
			{
				Rows = x
			};
		}

		public void Restart()
		{
			GenerateBasicCells(Sizes);
			PlacePlayers(Players);
			SetNeighbors();
		}
	}

	public class WorldMapData
	{
		public List<WMRowData> Rows { get; set; }
	}

	public class WMRowData
	{
		public List<WMCellData> Cells { get; set; }
	}

	public class WMCellData
	{
		public int unitsCount { get; set; }
		public int buildingsCount { get; set; }
		public int defenceCount { get; set; }
		public string positionId{ get; set; }
		public string controllerName { get; set; }
	}
}