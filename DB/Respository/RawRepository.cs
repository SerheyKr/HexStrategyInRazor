using HexStrategyInRazor.DB.Interfaces;
using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.Map.DB.Respository;
using Microsoft.EntityFrameworkCore;

namespace HexStrategyInRazor.DB.Respository
{
	public class RawRepository : AbstractRepository<RawModel>, IRawRepository
	{
		public RawRepository(WadbContext context) : base(context)
		{

		}

		public async Task Add(RawModel entity)
		{
			await dbSet.AddAsync(entity);

			await context.SaveChangesAsync();
		}

		public async Task Delete(RawModel entity)
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

		public async Task<IEnumerable<RawModel>> GetAll()
		{
			return await dbSet.ToListAsync();
		}

		public async Task<RawModel?> GetById(int id)
		{
			return await dbSet.FindAsync(id);
		}

		public async Task Update(RawModel entity)
		{
			dbSet.Update(entity);
			await context.SaveChangesAsync();
		}
	}
}
