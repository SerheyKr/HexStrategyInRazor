using HexStrategyInRazor.Map.DB.Models;
using HexStrategyInRazor.Map.Map.DB.Interfaces;

namespace HexStrategyInRazor.Map.DB.Interfaces
{
    public interface IUserRepository: IAbstractRepository<User>
    {
        public string AddWithErrorText(User entity, out bool foundedError);
    }
}
