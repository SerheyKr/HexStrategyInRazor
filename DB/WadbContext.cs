using HexStrategyInRazor.Map.DB.Interfaces;
using HexStrategyInRazor.Map.DB.Models;
using HexStrategyInRazor.Map.DB.Respository;
using Microsoft.EntityFrameworkCore;

namespace HexStrategyInRazor.DB
{
    public class WadbContext(DbContextOptions<WadbContext> options, string connectionString) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public IUserRepository UsersRepository
        {
            get
            {
                usersRepository ??= new UserRespository(this);
                return usersRepository;
            }
        }
        private IUserRepository usersRepository;
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString);
        }
    }
}
