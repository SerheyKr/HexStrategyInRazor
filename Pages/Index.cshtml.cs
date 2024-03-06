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
		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGetAsync()
		{
			string? userId = Request.Cookies["USERID"];

			if (string.IsNullOrEmpty(userId))
			{
				userId = Guid.NewGuid().ToString();
				Response.Cookies.Append("USERID", userId);
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
				var players = new List<Player>()
				{
					new (Color.Blue, "F", user, true),
					new (Color.Red, "E")
				};

				newMap = WorldMap.CreateMap(new System.Numerics.Vector2(4, 8), players);
				user.UserCurrentWorld = newMap;
			}
			else
			{
				newMap = user.UserCurrentWorld;
			}
		}
	}
}