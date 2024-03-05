using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using WebApplication1.Generator;

namespace WebApplication1.Pages
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
			string userId = Request.Cookies["USERID"];

			if (string.IsNullOrEmpty(userId))
			{
				userId = Guid.NewGuid().ToString();
				Response.Cookies.Append("USERID", userId);
				Program.AllUsers.Add(new BrowserUser()
				{
					UserId = userId
				});
			}

			var players = new List<Player>()
				{
					new (Color.Blue, "F"),
					new (Color.Red, "E")
				};

			var user = Program.AllUsers.Find(x => x.UserId == userId);
			if (user == null)
			{
				user = new BrowserUser() { UserId = userId };
				Program.AllUsers.Add(user);
				newMap = WorldMap.CreateMap(new System.Numerics.Vector2(4, 8), players);
			}
			if (user.UserCurrentWorld == null)
			{
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