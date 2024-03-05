using WebApplication1.DB.Models;

namespace WebApplication1.DB.Interfaces
{
    public interface IUserRepository: IAbstractRepository<User>
    {
        public string AddWithErrorText(User entity, out bool foundedError);
    }
}
