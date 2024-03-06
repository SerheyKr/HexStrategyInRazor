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
			string? userId = context.Request.Cookies["USERID"];

			if (string.IsNullOrEmpty(userId))
			{
				return "WE ALL DOOMED";
			}

			WorldMap? wm = WorldMaps.Find(x => x.HostId == userId);
			if (wm == null)
			{
				return "WE ALL DOOMED";
			}

			return JsonSerializer.Serialize(wm.ToData());
		}
	}
}
