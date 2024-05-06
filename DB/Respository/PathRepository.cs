using HexStrategyInRazor.DB.Interfaces;
using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.Map.DB.Respository;
using Microsoft.EntityFrameworkCore;

namespace HexStrategyInRazor.DB.Respository
{
	public class PathRepository : AbstractRepository<PathModel>, IPathRepository
	{
		public PathRepository(WadbContext context) : base(context)
		{

		}

		public async Task Add(PathModel entity)
		{
			await dbSet.AddAsync(entity);

			await context.SaveChangesAsync();
		}

		public async Task Delete(PathModel entity)
		{
			dbSet.Remove(entity);
			await context.SaveChangesAsync();
		}

		public async Task DeleteById(int id)
		{
			var toRemove = await dbSet.FindAsync(id);

			if (toRemove != null)
			{
				dbSet.Remove(toRemove);
				await context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<PathModel>> GetAll()
		{
			return await dbSet.ToListAsync();
		}

		public async Task<PathModel?> GetById(int id)
		{
			return await dbSet.FindAsync(id);
		}

		public async Task Update(PathModel entity)
		{
			dbSet.Update(entity);
			await context.SaveChangesAsync();
		}
	}
}
