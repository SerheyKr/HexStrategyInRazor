using HexStrategyInRazor.Map.DB.Models;

namespace HexStrategyInRazor.Map.DB.Respository
{
    public interface IUserRespository
    {
        void Add(User entity);
        string AddWithErrorText(User entity, out bool foundedError);
        void Delete(User entity);
        void DeleteById(int id);
        IEnumerable<User> GetAll();
        User? GetById(int id);
        void Update(User entity);
    }
}