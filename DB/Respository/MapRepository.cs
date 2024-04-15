using HexStrategyInRazor.DB.Interfaces;
using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.Map.DB.Interfaces;
using HexStrategyInRazor.Map.DB.Models;
using HexStrategyInRazor.Map.DB.Respository;
using Microsoft.EntityFrameworkCore;

namespace HexStrategyInRazor.DB.Respository
{
	public class MapRepository : AbstractRepository<MapModel>, IMapRepository
	{
		public MapRepository(WadbContext context) : base(context)
		{

		}

		public async Task Add(MapModel entity)
		{
			await dbSet.AddAsync(entity);

			await context.SaveChangesAsync();
		}

		public async Task Delete(MapModel entity)
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

		public async Task<IEnumerable<MapModel>> GetAll()
		{
			return await dbSet.ToListAsync();
		}

		public async Task<MapModel?> GetById(int id)
		{
			return await dbSet.FindAsync(id);
		}

		public async Task Update(MapModel entity)
		{
			dbSet.Update(entity);
			await context.SaveChangesAsync();
		}
	}
}
