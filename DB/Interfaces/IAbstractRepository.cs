using WebApplication1.DB.Models;

namespace WebApplication1.DB.Interfaces
{
    public interface IAbstractRepository<T> where T : BaseModel
    {
        public void Add(T entity);
        public void Delete(T entity);
        public void DeleteById(int id);
        public IEnumerable<T> GetAll();
        public T? GetById(int id);
        public void Update(T entity);
    }
}
