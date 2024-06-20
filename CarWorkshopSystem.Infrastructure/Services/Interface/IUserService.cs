using CarWorkshopSystem.Core.Domain;

namespace CarWorkshopSystem.Infrastructure.Services.Interface
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
    }
}