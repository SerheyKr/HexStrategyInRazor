namespace HexStrategyInRazor.Map.DB.Interfaces
{
	public interface ICrud<T> where T : class
	{
		Task<IEnumerable<T>> GetAll();
		Task<T?> GetById(int id);
		Task Add(T model);
		Task Update(T model);
		Task Delete(int modelId);
	}
}
