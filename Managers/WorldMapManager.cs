using HexStrategyInRazor.Generator;
using HexStrategyInRazor.Map;
using HexStrategyInRazor.Map.DB;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace HexStrategyInRazor.Managers
{
	public static class WorldMapManager
	{
		//TODO connect with DB
		public static List<WorldMap> WorldMaps = new List<WorldMap>();

		public static ActionResult<string> GetMapData(HttpContext context)
		{
			//TODO what if dude deleted cookies?
			string? userId = Encryption.Decrypt(context.Request.Cookies[Program.userIdCookieName]);

			if (string.IsNullOrEmpty(userId))
			{
				return new BadRequestResult();
			}

			WorldMap? wm = WorldMaps.Find(x => x.HostId == userId);
			if (wm == null)
			{
				return new BadRequestResult();
			}

			return JsonSerializer.Serialize(wm.ToData());
		}

		public static ActionResult<string> GetPlayerInfo(HttpContext context)
		{
			//TODO what if dude deleted cookies?
			string? userId = Encryption.Decrypt(context.Request.Cookies[Program.userIdCookieName]);

			if (string.IsNullOrEmpty(userId))
			{
				return new BadRequestResult();
			}

			WorldMap? wm = WorldMaps.Find(x => x.HostId == userId);
			if (wm == null)
			{
				return new BadRequestResult();
			}

			Player? player = wm.Players.Find(x => x.User.UserId == userId);

			if (player == null)
			{
				return new BadRequestResult();
			}

			return JsonSerializer.Serialize(player.ToData());
		}

		public static async Task<ActionResult> SendArmy(HttpContext context)
		{
			//TODO what if dude deleted cookies?
			string? userId = Encryption.Decrypt(context.Request.Cookies[Program.userIdCookieName]);

			if (string.IsNullOrEmpty(userId))
			{
				return new BadRequestResult();
			}

			WorldMap? wm = WorldMaps.Find(x => x.HostId == userId);
			if (wm == null)
			{
				return new BadRequestResult();
			}

			Player? player = wm.Players.Find(x => x.User.UserId == userId);

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

			return new OkResult();
		}

		internal static async Task<ActionResult> EndTurn(HttpContext context)
		{
			//TODO what if dude deleted cookies?
			string? userId = Encryption.Decrypt(context.Request.Cookies[Program.userIdCookieName]);

			if (string.IsNullOrEmpty(userId))
			{
				return new BadRequestResult();
			}

			WorldMap? wm = WorldMaps.Find(x => x.HostId == userId);
			if (wm == null)
			{
				return new BadRequestResult();
			}

			wm.EndTurn();
			return new OkResult();
		}

		internal static async Task<ActionResult> RestartMap(HttpContext context)
		{
			//TODO what if dude deleted cookies?
			string? userId = Encryption.Decrypt(context.Request.Cookies[Program.userIdCookieName]);

			if (string.IsNullOrEmpty(userId))
			{
				return new BadRequestResult();
			}

			WorldMap? wm = WorldMaps.Find(x => x.HostId == userId);
			if (wm == null)
			{
				return new BadRequestResult();
			}

			wm.Restart();

			return new OkResult();
		}
	}
}
