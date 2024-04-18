using Microsoft.EntityFrameworkCore;
using HexStrategyInRazor.DB;
using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.Map;
using HexStrategyInRazor.Map.DB.Models;
using HexStrategyInRazor.Managers;
using HexStrategyInRazor.Map.DB.Respository;
using HexStrategyInRazor.DB.Respository;
using Microsoft.Extensions.DependencyInjection;

namespace HexStrategyInRazor
{
	public static class Program
	{
		private static WadbContext context;
		private static WebApplication application;
		private static IConfigurationRoot configuration;

		public const string userIdCookieName = "USERID";
		public static readonly CookieOptions cookieOptions = new CookieOptions()
		{
			Path = @"\",
			HttpOnly = true,
			IsEssential = true,
			Expires = DateTime.Now.AddMonths(1) // TODO think about it
		};

		public static WadbContext Context { get => context;}
		public static WebApplication App { get => application;}
		public static IConfigurationRoot Configuration { get => configuration;}

		private static async Task Main(string[] args)
		{

			configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false)
			.Build();

			//context = SetUpDb(configuration);

			application = SetUpWebApplication();

			return;

			var mapRepo = new MapRepository(context);
			var userRepo = new UserRespository(context);
			var rawRepo = new RawRepository(context);
			var cellRepo = new CellRepository(context);

			await mapRepo.Add(new MapModel(0, "ABD"));
			await userRepo.Add(new UserModel(0, 1, "ABD"));

			await rawRepo.Add(new RawModel(0, 1));
			await rawRepo.Add(new RawModel(0, 1));
			await rawRepo.Add(new RawModel(0, 1));
			await cellRepo.Add(new CellModel(0, 1, 1, 1, 1, 0));
			await cellRepo.Add(new CellModel(0, 1, 2, 2, 2, 0));
			await cellRepo.Add(new CellModel(0, 1, 3, 3, 3, 0));

			var map = await mapRepo.GetById(1);

			//context.UsersRepository.Add(new User() { Id = 0, UserName = "UserName", Password = "Password", Email = "7366723@stud.nau.edu.ua" });
			//context.UsersRepository.Add(new User() { Id = 0, UserName = "Test", Password = "Test", Email = "7366723@stud.nau.edu.ua" });
			//context.UsersRepository.Add(new User() { Id = 0, UserName = "Admin", Password = "123", Email = "7366723@stud.nau.edu.ua" });
		}

		private static WadbContext SetUpDb(IConfigurationRoot configuration)
		{
			var options = new DbContextOptionsBuilder<WadbContext>();
			WadbContext cont = new (options.Options, connectionString: configuration.GetConnectionString("connectionPath"));
			cont.Database.EnsureDeleted();
			if (cont.Database.EnsureCreated())
			{
				Console.WriteLine("DB created");
			}

			return cont;
		}

		private static WebApplication SetUpWebApplication()
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();

			// Add services to the container.
			builder.Services.AddRazorPages();

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
			app.MapGet("/getMapData", WorldMapManager.GetMapData);
			app.MapGet("/getUserData", WorldMapManager.GetPlayerInfo);

			app.MapPost("/sendArmyData", WorldMapManager.SendArmy);
			app.MapPost("/restartMap", WorldMapManager.RestartMap);
			app.MapPost("/endTurn", WorldMapManager.EndTurn);

			app.UseRouting();

			app.UseAuthorization();

			app.UseCookiePolicy(new CookiePolicyOptions()
			{
				
			});

			app.MapRazorPages();

			app.Run();

			return app;
		}
	}
}