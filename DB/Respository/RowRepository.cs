using HexStrategyInRazor.DB.Interfaces;
using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.Map.DB.Respository;
using Microsoft.EntityFrameworkCore;

namespace HexStrategyInRazor.DB.Respository
{
	public class RowRepository : AbstractRepository<RowModel>, IRawRepository
	{
		public RowRepository(WadbContext context) : base(context)
		{

		}

		public async Task Add(RowModel entity)
		{
			await dbSet.AddAsync(entity);

			await context.SaveChangesAsync();
		}

		public async Task Delete(RowModel entity)
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

		public async Task<IEnumerable<RowModel>> GetAll()
		{
			return await dbSet.ToListAsync();
		}

		public async Task<RowModel?> GetById(int id)
		{
			return await dbSet.FindAsync(id);
		}

		public async Task Update(RowModel entity)
		{
			dbSet.Update(entity);
			await context.SaveChangesAsync();
		}
	}
}
