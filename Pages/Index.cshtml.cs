using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using HexStrategyInRazor.Generator;
using HexStrategyInRazor.Map;
using HexStrategyInRazor.Managers;

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

		public void OnGetAsync()
		{
			string? userId = Request.Cookies[Program.userIdCookieName];

			if (string.IsNullOrEmpty(userId))
			{
				userId = Guid.NewGuid().ToString();
				Response.Cookies.Append(Program.userIdCookieName, userId, Program.cookieOptions);
				PlayerManager.AllUsers.Add(new BrowserUser()
				{
					UserId = userId
				});
			}

			var user = PlayerManager.AllUsers.Find(x => x.UserId == userId);
			if (user == null)
			{
				user = new BrowserUser() { UserId = userId };
				PlayerManager.AllUsers.Add(user);
			}
			if (user.UserCurrentWorld == null)
			{
				currentPlayer = new Player(Color.Blue, "F", user, true);
				var players = new List<Player>()
				{
					currentPlayer,
					new (Color.Red, "E")
				};

				newMap = WorldMap.CreateMap(new System.Numerics.Vector2(4, 8), players);
				user.UserCurrentWorld = newMap;
			}
			else
			{
				newMap = user.UserCurrentWorld;
				currentPlayer = user.UserCurrentWorld.Players.Single(x => x.User?.UserId == userId);
			}
		}
	}
}