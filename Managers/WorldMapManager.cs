﻿using HexStrategyInRazor.Generator;
using HexStrategyInRazor.Map;
using System.Text.Json;

namespace HexStrategyInRazor.Managers
{
	public class WorldMapManager
	{
		//TODO connect with DB
		public static List<WorldMap> WorldMaps = new List<WorldMap>();

		public static string GetCells(HttpContext context)
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
	}
}