﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using HexStrategyInRazor.Map.DB.Interfaces;
using HexStrategyInRazor.DB;
using HexStrategyInRazor.Map.DB.Models;

namespace HexStrategyInRazor.Map.DB.Respository
{
	public class UserRespository : AbstractRepository<UserModel>, IUserRepository
	{
		public UserRespository(WadbContext context) : base(context)
		{

		}

		public async Task Add(UserModel entity)
		{
			await dbSet.AddAsync(entity);

			await context.SaveChangesAsync();
		}

		public async Task Delete(UserModel entity)
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

		public async Task<IEnumerable<UserModel>> GetAll()
		{
			return await dbSet.ToListAsync();
		}

		public async Task<UserModel?> GetById(int id)
		{
			return await dbSet.FindAsync(id);
		}

		public async Task Update(UserModel entity)
		{
			dbSet.Update(entity);
			await context.SaveChangesAsync();
		}
	}
}
