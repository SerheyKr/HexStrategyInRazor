using Microsoft.EntityFrameworkCore;
using HexStrategyInRazor.Map.DB.Models;
using HexStrategyInRazor.DB;

namespace HexStrategyInRazor.Map.DB.Respository
{
    public abstract class AbstractRepository<T> where T : class
    {
        protected readonly WadbContext context;
        protected readonly DbSet<T> dbSet;

        public AbstractRepository(WadbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }
    }
}
