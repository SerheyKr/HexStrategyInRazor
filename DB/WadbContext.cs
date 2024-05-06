using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.Map.DB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace HexStrategyInRazor.DB
{
	public class WadbContext(DbContextOptions<WadbContext> options) : DbContext(options)
	{
		public DbSet<UserModel> Users { get; set; }
		public DbSet<MapModel> Maps { get; set; }
		public DbSet<CellModel> Cells { get; set; }
		public DbSet<RowModel> Rows { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var opt = options.FindExtension<SqlServerOptionsExtension>();
			optionsBuilder.UseLazyLoadingProxies().UseSqlServer(opt.ConnectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MapModel>()
			.HasOne(map => map.User)
			.WithOne(user => user.Map)
			.HasForeignKey<UserModel>(user => user.MapId)
			.IsRequired(false);

			modelBuilder.Entity<MapModel>()
			.HasMany(map => map.Rows)
			.WithOne(row => row.Map)
			.HasForeignKey(raw => raw.MapId)
			.IsRequired(false);

			modelBuilder.Entity<RowModel>()
			.HasMany(raw => raw.Cells)
			.WithOne(cell => cell.Row)
			.HasForeignKey(cell => cell.RowId)
			.IsRequired(false);

			modelBuilder.Entity<CellModel>()
			.HasMany(cell => cell.Paths)
			.WithOne(path => path.CellFrom)
			.HasForeignKey(path => path.CellFromdID)
			.IsRequired(false);
		}
	}
}
