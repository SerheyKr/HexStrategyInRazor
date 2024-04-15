using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.Map.DB.Interfaces;
using HexStrategyInRazor.Map.DB.Models;
using HexStrategyInRazor.Map.DB.Respository;
using Microsoft.EntityFrameworkCore;

namespace HexStrategyInRazor.DB
{
	public class WadbContext(DbContextOptions<WadbContext> options, string connectionString) : DbContext(options)
	{
		public DbSet<UserModel> Users { get; set; }
		public DbSet<MapModel> Maps { get; set; }
		public DbSet<CellModel> Cells { get; set; }
		public DbSet<RawModel> Raws { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MapModel>()
			.HasOne(map => map.User)
			.WithOne(user => user.Map)
			.HasForeignKey<UserModel>(user => user.MapId)
			.IsRequired(false);

			modelBuilder.Entity<MapModel>()
			.HasMany(map => map.Raws)
			.WithOne(raw => raw.Map)
			.HasForeignKey(raw => raw.MapId)
			.IsRequired(false);

			modelBuilder.Entity<RawModel>()
			.HasMany(raw => raw.Cells)
			.WithOne(cell => cell.Raw)
			.HasForeignKey(cell => cell.RawId)
			.IsRequired(false);

			modelBuilder.Entity<CellModel>()
			.HasMany(cell => cell.Paths)
			.WithOne(path => path.CellFrom)
			.HasForeignKey(path => path.CellFromdID)
			.IsRequired(false);
		}
	}
}
