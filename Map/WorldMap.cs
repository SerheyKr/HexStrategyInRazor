using HexStrategyInRazor.Generator;
using HexStrategyInRazor.Managers;
using HexStrategyInRazor.Map.Data;
using System.Drawing;
using System.Numerics;

namespace HexStrategyInRazor.Map
{
    public class WorldMap
	{
		private const int MOVE_BASIC_COST = 10;

		public List<WMRow> Rows = new List<WMRow>();
		public List<WMCell> allCells = new List<WMCell>();
		public List<Player> Players = new List<Player>();
        public List<Army> Units = new List<Army>();
        public string HostId;
		public Vector2 Sizes;
		public bool WorldSuspended = false;
		public int TotalTickCount = 0;

		private WorldMap()
		{
			WorldMapManager.WorldMaps.Add(this);
		}

		private List<WMCell> openList = new List<WMCell>();
		private List<WMCell> closedList = new List<WMCell>();

		private List<WMCell> FindPath(WMCell startNode, WMCell endNode)
		{
			var cellWay = new List<WMCell>();
			openList = new List<WMCell>() { startNode };
			closedList = new List<WMCell>();

			foreach (var x in allCells)
			{
				x.GCost = int.MaxValue;
				x.CalculateFCost();
				x.CameFrom = null;
			}

			startNode.GCost = 0;
			startNode.HCost = CalculateDistance(startNode, endNode);

            return cellWay;
		}

		private int CalculateDistance(WMCell a, WMCell b)
		{
			int xDistance;
			int yDistance;
			int remaining;

			return 0;
		}

		public void Tick()
		{
			if (!WorldSuspended)
            {
                TotalTickCount++;
            }
        }

        public void CreateMovement(Player player, UnitMoveData moveData)
		{
			//TODO mark them with ?
			WMCell startCell = allCells.Find(x => x.ID == moveData.FromId);
			WMCell endCell = allCells.Find(x => x.ID == moveData.ToId);

			if (startCell.Controller != player)
			{
				return;
			}

			if (startCell.unitsCount <= 0)
			{
				return;
			}

			var way = FindPath(startCell, endCell);
		}

		private void GenerateBasicCells(Vector2 sizes)
		{
			Rows.Clear();
			allCells.Clear();
			Sizes = sizes;

			for (int i = 0; i < sizes.X; i++)
			{
				var nr = new WMRow();
				Rows.Add(nr);
				for (int j = 0; j < sizes.Y; j++)
				{
					var nc = new WMCell(new Vector2(i, j), this);
					nr.Cells.Add(nc);
					allCells.Add(nc);
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
							nc.Neighbors.Add(Rows[x].Cells[y]);
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
				} while (randomCell.Controller != null);

				randomCell.Controller = np;
				Units.Add(new Army()
				{
					currentCell = randomCell,
					Controller = np,
					UnitsCount = 5,
					moves = false,
				});
				np.CurrentMap = this;
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

		public WorldMapData ToData()
		{
			List<WMRowData> x = new List<WMRowData>(Rows.Select(x => x.ToData()));

			return new WorldMapData()
			{
				Rows = x,
				TotalTicks = TotalTickCount
			};
		}

		public void Restart()
		{
			GenerateBasicCells(Sizes);
			PlacePlayers(Players);
			SetNeighbors();
		}
	}
}