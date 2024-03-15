using HexStrategyInRazor.Generator;
using HexStrategyInRazor.Map;
using System.Text;
using System.Text.Json;

namespace HexStrategyInRazor.Managers
{
	public class WorldMapManager
	{
		//TODO connect with DB
		public static List<WorldMap> WorldMaps = new List<WorldMap>();

		public static string GetMapData(HttpContext context)
		{
			//TODO what if dude deleted cookies?
			string? userId = context.Request.Cookies[Program.userIdCookieName];

			if (string.IsNullOrEmpty(userId))
			{
				return "WE ALL DOOMED COOKIES";
			}

			WorldMap? wm = WorldMaps.Find(x => x.HostId == userId);
			if (wm == null)
			{
				return "WE ALL DOOMED MAP";
			}

			return JsonSerializer.Serialize(wm.ToData());
		}

		public static string GetPlayerInfo(HttpContext context)
		{
			//TODO what if dude deleted cookies?
			string? userId = context.Request.Cookies[Program.userIdCookieName];

			if (string.IsNullOrEmpty(userId))
			{
				return "WE ALL DOOMED COOKIES";
			}

			WorldMap? wm = WorldMaps.Find(x => x.HostId == userId);
			if (wm == null)
			{
				return "WE ALL DOOMED MAP";
			}

			Player? player = wm.Players.Find(x => x.User.UserId == userId);

			if (player == null)
			{
				return "WE ALL DOOMED PLAYER";
			}

			return JsonSerializer.Serialize(player.ToData());
		}

		public static async Task SendArmy(HttpContext context)
        {
            string? userId = context.Request.Cookies[Program.userIdCookieName];

            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            WorldMap? wm = WorldMaps.Find(x => x.HostId == userId);
            if (wm == null)
            {
                return;
            }

            Player? player = wm.Players.Find(x => x.User.UserId == userId);

			if (player == null)
			{
				return;
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
        }

		internal static async Task RestartMap(HttpContext context)
		{
			//TODO what if dude deleted cookies?
			string? userId = context.Request.Cookies[Program.userIdCookieName];

			if (string.IsNullOrEmpty(userId))
			{
				return;
			}

			WorldMap? wm = WorldMaps.Find(x => x.HostId == userId);
			if (wm == null)
			{
				return;
			}

			wm.Restart();
		}
	}

	public class UnitMoveData()
	{
		public string FromId { get; set; }
		public string ToId { get; set; }
		public int ArmyCount {  get; set; }
	}
}
