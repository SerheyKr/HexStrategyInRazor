using Microsoft.EntityFrameworkCore;
using WebApplication1.DB.Interfaces;
using WebApplication1.DB.Models;
using WebApplication1.DB.Respository;

namespace WebApplication1.DB
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
