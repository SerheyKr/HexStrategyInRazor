using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WebApplication1.DB;
using WebApplication1.DB.Models;
using WebApplication1.Generator;

namespace WebApplication1
{
    public static class Program
    {
        private static WadbContext context;
        private static WebApplication application;
        private static IConfigurationRoot configuration;

        public static WadbContext Context { get => context;}
        public static WebApplication App { get => application;}
        public static IConfigurationRoot Configuration { get => configuration;}

        public static WorldMap wm;

        private static void Main(string[] args)
        {
            configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            context = SetUpDb(configuration);

            context.UsersRepository.Add(new User() { Id = 0, UserName = "UserName", Password = "Password", Email = "7366723@stud.nau.edu.ua" });
            context.UsersRepository.Add(new User() { Id = 0, UserName = "Test", Password = "Test", Email = "7366723@stud.nau.edu.ua" });
            context.UsersRepository.Add(new User() { Id = 0, UserName = "Admin", Password = "123", Email = "7366723@stud.nau.edu.ua" });

            context.SaveChanges();

            application = SetUpWebApplication(configuration);
        }

        private static WadbContext SetUpDb(IConfigurationRoot configuration)
        {
            var options = new DbContextOptionsBuilder<WadbContext>();
            WadbContext cont = new(options.Options, configuration.GetConnectionString("connectionPath"));
            cont.Database.EnsureDeleted();
            if (cont.Database.EnsureCreated())
            {
                Console.WriteLine("DB created");
            }

            return cont;
        }

        private static WebApplication SetUpWebApplication(IConfigurationRoot configuration) 
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();

            // Add services to the container.
            builder.Services.AddRazorPages();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.MapGet("/getData", () => $"{DateTime.Now}");

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();

            return app;
        }
    }
}