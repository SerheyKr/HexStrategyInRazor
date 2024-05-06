using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.Managers;
using HexStrategyInRazor.Map.Data;
using System.Numerics;

namespace HexStrategyInRazor.Map
{
	public class WorldMap
	{
		private const int MOVE_BASIC_COST = 10;
		private const int SPAWN_UNITS_COUNT = 5;

		public List<WMRow> Rows = new List<WMRow>();
		public List<WMCell> AllCells => Rows.SelectMany(x => x.Cells).ToList();
		public List<Player> Players = new List<Player>();
		public string HostId => MainPlayer.PlayerId;
		public Vector2 Sizes;
		public int TotalTurnsCount = 0;

		public bool IsEnded = false;
		private Player winSide;

		public Player MainPlayer => Players.Find(x => x.IsMainPlayer);
		public int DbId;

		private WorldMap()
		{

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

			if (startCell.Controller == endCell.Controller && startCell.NeighborsInputArmy.ToList().Contains(endCell))
			{
				startCell.AddNeighborsSendArmy(endCell).Wait();
				(startCell, endCell) = (endCell, startCell);
			}

			startCell.AddNeighborsSendArmy(endCell).Wait();
		}

		public void CreateMovement(WMCell startCell, WMCell endCell)
		{
			if (!startCell.Neighbors.Contains(endCell) || startCell.NeighborsSendArmy.Contains(endCell))
			{
				return;
			}

			startCell.AddNeighborsSendArmy(endCell).Wait();
		}

		private void GenerateBasicCells(Vector2 sizes)
		{
			Rows.Clear();
			Sizes = sizes;

			for (int i = 0; i < sizes.X; i++)
			{
				var nr = new WMRow(this)
				{
					PositionX = i
				};
				Rows.Add(nr);
				for (int j = 0; j < sizes.Y; j++)
				{
					var nc = new WMCell(new Vector2(i, j), this, nr);
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
							{ +1, 0 }, { -1, -1 }, { 0, -1 },
							{ -1, 0 }, { -1, +1 }, { 0, +1 }
						};
					}
					else
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

		private bool WinCheck()
		{
			if (!AllCells.Exists(x => x.Controller == Players[0]))
			{
				//You loosed
				IsEnded = true;
				winSide = Players[0];
				return true;
			}
			else if (!AllCells.Exists(x => x.Controller == Players[1]))
			{
				//Bot loosed
				IsEnded = true;
				winSide = Players[1];
				return true;
			}
			return false;
		}

		private void SetPlayer(Player player, WMCell cell)
		{
			cell.Controller = player;
			cell.UnitsCount = SPAWN_UNITS_COUNT;
			player.CurrentMap = this;
		}

		private void PlacePlayers(List<Player> players)
		{
			Players = players;
			//We basicly have only 2 players...

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
			var mainUser = players.Find(x => x.IsMainPlayer);
			return mainUser == null ? throw new ArgumentException() : map;
		}

		public void OnTurnEnd()
		{
			Players.ForEach(x => x.OnTurnEnd());
		}

		public WorldMapData ToData()
		{
			//semaphore.WaitOne();

			List<WMRowData> x = new List<WMRowData>(Rows.Select(x => x.ToData()));

			//semaphore.Release();
			return new WorldMapData()
			{
				Rows = x,
				TurnsCount = TotalTurnsCount + 1,
				IsEnded = IsEnded,
				EndText = GenerateEndText(),
				TotalArmy = AllCells.FindAll(x =>
				{
					if (x.Controller == null)
						return false;
					return x.Controller.IsMainPlayer;
				}).Sum(x => x.UnitsCount) ?? 0
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
				}
				else if (winSide == Players[1])
				{
					returnText = "You winned your opponent!\r\n";
				}

				returnText += $"Game taked {TotalTurnsCount + 1} turns!";
			}

			return returnText;
		}

		public void Restart()
		{
			//semaphore.WaitOne();
			//GenerateBasicCells(Sizes);
			//SetNeighbors();

			IsEnded = false;
			TotalTurnsCount = 0;
			AllCells.ForEach(x =>
			{
				x.Controller = null;
				x.UnitsCount = 0;
				x.ClearAllWays();
			});
			PlacePlayers(Players);

			Players.ForEach(x => x.OnTurnEnd());
			//semaphore.Release();
		}

		public void EndTurn()
		{
			if (IsEnded)
			{
				return;
			}
			TotalTurnsCount++;
			// TODO Maybe some better way?
			do
			{
				AllCells.ForEach(cell => cell.EndTurn());
			} while (AllCells.Exists(cell => cell.UnitsCount > 0 && cell.NeighborsSendArmy.Count != 0));
			AllCells.ForEach(cell => cell.EndAfter());

			if (WinCheck())
				return;
			Players.ForEach(player => player.OnTurnEnd());
		}

		public static WorldMap? Load(MapModel map, Player user)
		{
			var player = user;

			if (map == null)
				return null;

			var wmap = new WorldMap()
			{
				DbId = map.Id,
			};

			player.CurrentMap = wmap;
			wmap.Players.Add(user);
			var ai = new AI
			{
				PlayerId = map.BotId,
				CurrentMap = wmap,
			};
			wmap.Players.Add(ai);

			wmap.Rows = map.Rows.Select(x => WMRow.Load(x, wmap)).ToList();

			player!.CurrentMap = wmap;

			wmap.AllCells.ForEach(cell => cell.LoadPathes(wmap));

			wmap.TotalTurnsCount = map.TotalTurnsCount;

			wmap.SetNeighbors();

			wmap.WinCheck();

			return wmap;
		}

		public MapModel ToDBData()
		{
			return new MapModel()
			{
				Id = DbId,
				PlayerCookieId = this.HostId,
				BotId = Players.Find(x => x is AI)!.PlayerId,
				TotalTurnsCount = TotalTurnsCount,
				Rows = Rows.Select(x => x.ToDBData()).ToList(),
			};
		}
	}
}