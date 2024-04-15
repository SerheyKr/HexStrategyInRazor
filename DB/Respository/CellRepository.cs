using HexStrategyInRazor.DB.Interfaces;
using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.Map.DB.Respository;
using Microsoft.EntityFrameworkCore;

namespace HexStrategyInRazor.DB.Respository
{
	public class CellRepository : AbstractRepository<CellModel>, ICellRepository
	{
		public CellRepository(WadbContext context) : base(context)
		{

		}

		public async Task Add(CellModel entity)
		{
			await dbSet.AddAsync(entity);

			await context.SaveChangesAsync();
		}

		public async Task Delete(CellModel entity)
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

		public async Task<IEnumerable<CellModel>> GetAll()
		{
			return await dbSet.ToListAsync();
		}

		public async Task<CellModel?> GetById(int id)
		{
			return await dbSet.FindAsync(id);
		}

		public async Task Update(CellModel entity)
		{
			dbSet.Update(entity);
			await context.SaveChangesAsync();
		}
	}
}
