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
		public List<WMCell> AllCells = new List<WMCell>();
		public List<Player> Players = new List<Player>();
		public List<Army> AllUnits = new List<Army>();
		public string HostId;
		public Vector2 Sizes;
		public bool WorldSuspended = false;
		public int TotalTurnsCount = 0;

		public DateTime Expires;

		private WorldMap()
		{
			Expires = DateTime.Now.AddMonths(1);
			WorldMapManager.WorldMaps.Add(this);
		}

		private List<WMCell>? FindPath(WMCell startNode, WMCell endNode)
		{
			var openList = new List<WMCell>() { startNode };
			var closedList = new List<WMCell>();

			foreach (var x in AllCells)
			{
				x.GCost = int.MaxValue;
				x.CalculateFCost();
				x.CameFrom = null;
			}

			startNode.GCost = 0;
			startNode.HCost = CalculateDistance(startNode, endNode);
			startNode.CalculateFCost();

			while (openList.Count > 0)
			{
				var currentNode = openList.OrderBy(x => x.FCost).FirstOrDefault();

				if (currentNode == endNode)
				{
					return CalculatePath(endNode);
				}

				openList.Remove(currentNode);
				closedList.Add(currentNode);

				foreach (var neigbourNode in currentNode.Neighbors)
				{
					if (closedList.Contains(neigbourNode))
					{
						continue;
					}

					int tentativeGCost = currentNode.GCost + CalculateDistance(currentNode, neigbourNode);

					if (tentativeGCost < neigbourNode.GCost)
					{
						neigbourNode.CameFrom = currentNode;
						neigbourNode.GCost = tentativeGCost;
						neigbourNode.HCost = CalculateDistance(neigbourNode, endNode);
						neigbourNode.CalculateFCost();

						if (!openList.Contains(neigbourNode))
						{
							openList.Add(neigbourNode);
						}
					}
				}
			}

			return null;
		}

		private List<WMCell> CalculatePath(WMCell endNode)
		{
			List<WMCell> path = new List<WMCell>() { endNode };
			var currentNode = endNode;
			while (currentNode.CameFrom != null) 
			{
				path.Add(currentNode.CameFrom);
				currentNode = currentNode.CameFrom;
			}

			path.Reverse();
			return path;
		}

		private int CalculateDistance(WMCell a, WMCell b)
		{
			int xDistance = (int)Math.Abs(a.position.X - b.position.X);
			int yDistance = (int)Math.Abs(a.position.Y - b.position.Y);
			int remaining = Math.Abs(xDistance - yDistance);

			return MOVE_BASIC_COST * remaining;
		}

		public void CreateMovement(Player player, UnitMoveData moveData)
		{
			WMCell? startCell = AllCells.Find(x => x.ID == moveData.FromId);
			WMCell? endCell = AllCells.Find(x => x.ID == moveData.ToId);

			if (startCell == null || endCell == null)
			{
				return;
			}

			if (startCell.Controller == null || startCell.Controller != player)
			{
				return;
			}

			if (startCell.unitsCount <= 0)
			{
				return;
			}

            List<WMCell>? way = FindPath(startCell, endCell);

			if (way == null)
			{
				return;
			}

			//startCell.currentUnits;
			//Units.Add();
		}

		private void GenerateBasicCells(Vector2 sizes)
		{
			Rows.Clear();
			AllCells.Clear();
			Sizes = sizes;

			for (int i = 0; i < sizes.X; i++)
			{
				var nr = new WMRow();
				Rows.Add(nr);
				for (int j = 0; j < sizes.Y; j++)
				{
					var nc = new WMCell(new Vector2(i, j), this);
					nr.Cells.Add(nc);
					AllCells.Add(nc);
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
				randomCell.ChangeUnitsCount(5);
				np.CurrentMap = this;
			}
		}

		public static WorldMap CreateMap(Vector2 sizes, List<Player> players)
		{
			WorldMap map = new WorldMap();
			map.GenerateBasicCells(sizes);
			map.PlacePlayers(players);
			map.SetNeighbors();

			//TODO what if none of players is main?
			map.HostId = players.Find(x => x.IsMainPlayer).User.UserId;

			return map;
		}

		public WorldMapData ToData()
		{
			List<WMRowData> x = new List<WMRowData>(Rows.Select(x => x.ToData()));

			return new WorldMapData()
			{
				Rows = x,
				TotalTicks = TotalTurnsCount
			};
		}

		public void Restart()
        {
            AllUnits.Clear();
            GenerateBasicCells(Sizes);
			PlacePlayers(Players);
			SetNeighbors();
		}

        public void EndTurn()
        {
            TotalTurnsCount++;
			AllCells.ForEach(cell => cell.EndTurn());
			AllUnits.ForEach(unit => unit.DoMove());
        }
    }
}