using HexStrategyInRazor.DB;
using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.DB.Respository;
using HexStrategyInRazor.Generator;
using HexStrategyInRazor.Map;
using HexStrategyInRazor.Map.DB;
using HexStrategyInRazor.Map.DB.Models;
using HexStrategyInRazor.Map.DB.Respository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace HexStrategyInRazor.Managers
{
	public static class WorldMapManager
	{
		public static async Task<WorldMap?> GetMap(string userId)
		{
			var user = await GetPlayer(userId);
			if (user == null) 
			{
				return null;
			}
			var mapDB = await new MapRepository(Program.GetContext()).GetById(user.MapId);
			if (mapDB == null)
			{
				return null;
			}
			var map = WorldMap.Load(mapDB, user);
			return map;
		}

		public static async Task<WorldMap?> GetMap(Player user)
		{
			if (user == null) 
			{
				return null;
			}
			var mapDB = await new MapRepository(Program.GetContext()).GetById(user.MapId);
			if (mapDB == null)
			{
				return null;
			}
			var map = WorldMap.Load(mapDB, user);

			return map;
		}

		public static async Task<MapModel> AddMap(WorldMap worldMap)
		{
			var data = worldMap.ToDBData();
			await new MapRepository(Program.GetContext()).Add(data);

			worldMap.DbId = data.Id;

			worldMap.Rows.ForEach(row =>
			{
				row.DbId = data.Rows.ToList().Find(x => x.PositionX == row.PositionX).Id;
			});

			worldMap.AllCells.ForEach(cell => cell.DbId = data.Rows.SelectMany(row => row.Cells).
			ToList().Find(x => x.CellPositionX == cell.Position.X && x.CellPositionY == cell.Position.Y).Id);

			worldMap.OnTurnEnd();

			await UpdateMap(worldMap);

			return data;
		}

		public static async Task UpdateMap(WorldMap worldMap)
		{
			await new MapRepository(Program.GetContext()).Update(worldMap.ToDBData());
		}

		public static async Task<Player?> GetPlayer(string userId)
		{
			return Player.Load(await new UserRespository(Program.GetContext()).GetById(userId));
		}

		public static async Task<UserModel> AddPlayer(Player player)
		{
			var data = player.ToDBData();
			await new UserRespository(Program.GetContext()).Add(data);

			return data;
		}

		public static async Task UpdatePlayer(Player player)
		{
			await new UserRespository(Program.GetContext()).Update(player.ToDBData());
		}

		public static async Task<WorldMap> CreateMapAsync(Vector2 vector2, List<Player> players)
		{
			var newMap = WorldMap.CreateMap(vector2, players);

			await AddMap(newMap);

			return newMap;
		}

		public static async Task<ActionResult<string>> GetMapData(HttpContext context)
		{
			//TODO what if dude deleted cookies?
			string? userId = Encryption.Decrypt(context.Request.Cookies[Program.userIdCookieName]);

			if (string.IsNullOrEmpty(userId))
			{
				return new BadRequestResult();
			}

			WorldMap? wm = await GetMap(userId);
			if (wm == null)
			{
				return new BadRequestResult();
			}

			var json = JsonSerializer.Serialize(wm.ToData());
			return json;
		}

		public static async Task<ActionResult> SendArmy(HttpContext context)
		{
			//TODO what if dude deleted cookies?
			string? userId = Encryption.Decrypt(context.Request.Cookies[Program.userIdCookieName]);

			if (string.IsNullOrEmpty(userId))
			{
				return new BadRequestResult();
			}

			WorldMap? wm = await GetMap(userId);
			if (wm == null)
			{
				return new BadRequestResult();
			}

			Player? player = wm.MainPlayer;

			if (player == null)
			{
				return new BadRequestResult();
			}

			var req = context.Request;
			string bodyStr;

			using (StreamReader reader
				  = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
			{
				bodyStr = await reader.ReadToEndAsync();
			}


			var moveData = JsonSerializer.Deserialize<UnitMoveData>(bodyStr);
			wm.CreateMovement(player, moveData);

			//await UpdateMap(wm);

			return new OkObjectResult(JsonSerializer.Serialize(wm.ToData()));
		}

		internal static async Task<ActionResult> EndTurn(HttpContext context)
		{
			//TODO what if dude deleted cookies?
			string? userId = Encryption.Decrypt(context.Request.Cookies[Program.userIdCookieName]);

			if (string.IsNullOrEmpty(userId))
			{
				return new BadRequestResult();
			}
			
			WorldMap? wm = await GetMap(userId);
			if (wm == null)
			{
				return new BadRequestResult();
			}

			wm.EndTurn();

			await UpdateMap(wm);
			return new OkObjectResult(JsonSerializer.Serialize(wm.ToData()));
		}

		internal static async Task<ActionResult> RestartMap(HttpContext context)
		{
			//TODO what if dude deleted cookies?
			string? userId = Encryption.Decrypt(context.Request.Cookies[Program.userIdCookieName]);

			if (string.IsNullOrEmpty(userId))
			{
				return new BadRequestResult();
			}

			WorldMap? wm = await GetMap(userId);
			if (wm == null)
			{
				return new BadRequestResult();
			}

			wm.Restart();

			await UpdateMap(wm);

			return new OkObjectResult(JsonSerializer.Serialize(wm.ToData()));
		}
	}
}
