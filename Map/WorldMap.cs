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
		private const int INITIAL_SEMAPHORE_COUNT = 1;
		private const int MAXIMUM_SEMAPHORE_COUNT = 1;

		public List<WMRow> Rows = new List<WMRow>();
		public List<WMCell> AllCells = new List<WMCell>();
		public List<Player> Players = new List<Player>();
		public string HostId;
		public Vector2 Sizes;
		public bool WorldSuspended = false;
		public int TotalTurnsCount = 0;

		public bool IsEnded = false;
		private Player winSide;

		private readonly Semaphore pool = new(INITIAL_SEMAPHORE_COUNT, MAXIMUM_SEMAPHORE_COUNT);

		public DateTime Expires;

		public Player MainPlayer;

		private WorldMap()
		{
			Expires = DateTime.Now.AddMonths(1);
			WorldMapManager.WorldMaps.Add(this);
		}

		#region A*
		public List<WMCell>? FindPath(WMCell startNode, WMCell endNode)
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
			int xDistance = (int)Math.Abs(a.Position.X - b.Position.X);
			int yDistance = (int)Math.Abs(a.Position.Y - b.Position.Y);

			return (int)(MOVE_BASIC_COST * Math.Sqrt(xDistance * xDistance + yDistance * yDistance));
		}
		#endregion

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

			if (!startCell.Neighbors.Contains(endCell))
			{
				return;
			}

			startCell.AddNeighborsSendArmy(endCell);
		}
		public void CreateMovement(WMCell startCell, WMCell endCell)
		{
			if (!startCell.Neighbors.Contains(endCell))
			{
				return;
			}

			startCell.AddNeighborsSendArmy(endCell);
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
							{ +1, 0 }, { -1, -1 }, { 0, -1 },
							{ -1, 0 }, { -1, +1 }, { 0, +1 }
						};
					} else
					{
						coordsToCheck = new int[,]
						{
							{ +1, 0 }, { +1, -1 }, { 0, -1 },
							{ -1, 0 }, { 0, +1 }, { +1, +1 }
						};
					}

					for (int coordsToCheckI = 0; coordsToCheckI < coordsToCheck.Length / 2; coordsToCheckI++)
					{
						var x = i + coordsToCheck[coordsToCheckI, 0];
						var y = j + coordsToCheck[coordsToCheckI, 1];

						if (x >= 0 && y >= 0 &&
						x < Rows.Count && y < Rows[x].Cells.Count)
						{
							nc.Neighbors.Add(Rows[x].Cells[y]);
						}
					}
				}
			}
		}

		private void WinCheck()
		{
			if (!AllCells.Exists(x => x.Controller == Players[0]))
			{
				//You loosed
				IsEnded = true;
				winSide = Players[0];
			} else if (!AllCells.Exists(x => x.Controller == Players[1]))
			{
				//Bot loosed
				IsEnded = true;
				winSide = Players[1];
			}
		}

		private void SetPlayer(Player player, WMCell cell)
		{
			cell.Controller = player;
			cell.UnitsCount = 5;
			player.CurrentMap = this;
		}

		private void PlacePlayers(List<Player> players)
		{
			Players = players;
			//We basicly have only 2 players...
			MainPlayer = players.Find(x => x.IsMainPlayer);

			SetPlayer(players[0], Rows[0].Cells[0]);
			SetPlayer(players[1], Rows[^1].Cells[^1]);
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
			pool.WaitOne();

			List<WMRowData> x = new List<WMRowData>(Rows.Select(x => x.ToData()));

			pool.Release();
			return new WorldMapData()
			{
				Rows = x,
				TurnsCount = TotalTurnsCount + 1,
				IsEnded = IsEnded,
				EndText = GenerateEndText()
			};
		}

		public string GenerateEndText()
		{
			string returnText = "No text";
			if (IsEnded)
			{
				if (winSide == Players[0])
				{
					returnText = "You loosed to your opponent!\r\n";
				} else if (winSide == Players[1])
				{
					returnText = "You winned your opponent!\r\n";
				}

				returnText += $"Game taked {TotalTurnsCount + 1} turns!";
			}

			return returnText;
		}

		public void Restart()
		{
			pool.WaitOne();
			GenerateBasicCells(Sizes);
			PlacePlayers(Players);
			SetNeighbors();
			pool.Release();

			IsEnded = false;
			TotalTurnsCount = 0;
		}

		public void EndTurn()
		{
			if (IsEnded)
			{
				return;
			}
			TotalTurnsCount++;
			Players.ForEach(player => player.OnTurnEnd());
			//AllCells.OrderBy(x => x.Priority).ToList().ForEach(cell => cell.EndTurn());

			do
			{
				AllCells.ForEach(cell => cell.EndTurn());
			} while (AllCells.Exists(cell => cell.UnitsCount > 0 && cell.NeighborsSendArmy.Count != 0));

			AllCells.ForEach(cell => cell.EndAfter());
			WinCheck();
		}
	}
}