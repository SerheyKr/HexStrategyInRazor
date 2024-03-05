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
			Console.WriteLine("GETED");
			var players = new List<Player>()
			{
				new (Color.Blue, "F"),
				new (Color.Red, "E")
			};

			newMap = WorldMap.CreateMap(new System.Numerics.Vector2(4, 8), players);
		}
	}
}