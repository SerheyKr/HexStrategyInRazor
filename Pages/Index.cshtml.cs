using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using HexStrategyInRazor.Generator;
using HexStrategyInRazor.Map;
using HexStrategyInRazor.Managers;
using HexStrategyInRazor.Map.DB;

namespace HexStrategyInRazor.Pages
{
	public class IndexModel : PageModel
	{
		public WorldMap newMap;
		public Player currentPlayer;
		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public async Task OnGetAsync()
		{
			string? userId = Request.Cookies[Program.userIdCookieName];

			if (string.IsNullOrEmpty(userId))
			{
				userId = Guid.NewGuid().ToString();
				Response.Cookies.Append(Program.userIdCookieName, Encryption.Encrypt(userId), Program.cookieOptions);
				await WorldMapManager.AddPlayer(new Player() { PlayerId = userId });
			} else
			{
				userId = Encryption.Decrypt(userId);
			}

			var user = await WorldMapManager.GetPlayer(userId);
			if (user == null)
			{
				user = new Player() { PlayerId = userId };
				await WorldMapManager.AddPlayer(user);
			}
			var map = await WorldMapManager.GetMap(user);
			if (map == null)
			{
				user.PlayerColor = Color.Blue;
				user.PlayerName = "Y";
				user.IsMainPlayer = true;
				var players = new List<Player>()
				{
					user,
					new AI(Color.Red, "E")
				};

				newMap = await WorldMapManager.CreateMapAsync(new System.Numerics.Vector2(4, 8), players);
				user.CurrentMap = newMap;
				user.MapId = newMap.DbId;
				await WorldMapManager.UpdatePlayer(user);
			}
			else
			{
				newMap = user.CurrentMap;
			}
			currentPlayer = user;
		}
	}
}