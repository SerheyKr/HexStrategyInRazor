using HexStrategyInRazor.Map.DB.Models;

namespace HexStrategyInRazor.Map.Map.DB.Interfaces
{
	public interface IAbstractRepository<T> where T : IAbstractModel
	{
		public Task Add(T entity);
		public Task Delete(T entity);
		public Task DeleteById(int id);
		public Task<IEnumerable<T>> GetAll();
		public Task<T?> GetById(int id);
		public Task Update(T entity);
	}
}
