using Microsoft.EntityFrameworkCore;
using HexStrategyInRazor.DB;
using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.Map;
using HexStrategyInRazor.Map.DB.Models;
using HexStrategyInRazor.Managers;
using HexStrategyInRazor.Map.DB.Respository;
using HexStrategyInRazor.DB.Respository;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace HexStrategyInRazor
{
	public static class Program
	{
		private static WebApplication application;

		public const string userIdCookieName = "USERID";
		public static readonly CookieOptions cookieOptions = new CookieOptions()
		{
			Path = @"\",
			HttpOnly = true,
			IsEssential = true,
			Expires = DateTime.Now.AddMonths(1) // TODO
		};

		public static WebApplication App { get => application;}

		private static WadbContext context;

		public static WadbContext GetContext()
		{
			var scope = application.Services.CreateAsyncScope();
			var services = scope.ServiceProvider;

			return services.GetRequiredService<WadbContext>();
		}

		private static void Main(string[] args)
		{
			SetUpWebApplication();
		}

		private static async Task SetUpWebApplication()
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();

			builder.Configuration.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.IndexOf("bin\\"))));
			builder.Configuration.AddJsonFile("appsettings.json").AddEnvironmentVariables();

			// Add services to the container.
			builder.Services.AddRazorPages();

			builder.Services.AddDbContext<WadbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

			builder.Services.Configure<CookiePolicyOptions>(options => 
			{
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			WebApplication app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
				//app.UseSession(new SessionOptions()
				//{
				//	IdleTimeout = TimeSpan.FromMinutes(5),
				//	Cookie = new CookieBuilder()
				//	{
				//		HttpOnly = true,
				//		IsEssential = true
				//	}
				//});
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			//app.MapGet("/getData", (HttpContext context) => $"COOKIES DUDE: {context.Request.Cookies[userIdCookieName]}");
			//app.MapGet("/getCellData", (int xCoords, int yCoords) => $"Coords - {xCoords} : {yCoords}");
			app.MapGet("/getMapData", handler: WorldMapManager.GetMapData);

			app.MapPost("/sendArmyData", handler: WorldMapManager.SendArmy);
			app.MapPost("/restartMap", handler: WorldMapManager.RestartMap);
			app.MapPost("/endTurn", handler: WorldMapManager.EndTurn);

			app.UseRouting();

			app.UseAuthorization();

			app.UseCookiePolicy(new CookiePolicyOptions()
			{
				
			});

			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;

				context = services.GetRequiredService<WadbContext>();
				context.Database.EnsureCreated();
			}

			app.MapRazorPages();

			application = app;

			app.Run();
		}
	}
}