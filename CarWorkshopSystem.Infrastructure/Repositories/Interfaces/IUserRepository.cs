using CarWorkshopSystem.Core.Domain;

namespace CarWorkshopSystem.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByEmail(string email);
    }
}