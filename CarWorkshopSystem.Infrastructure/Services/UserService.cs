using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using CarWorkshopSystem.Infrastructure.Security;
using CarWorkshopSystem.Infrastructure.Services.Interface;

namespace CarWorkshopSystem.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);

            if (user != null && PasswordHasher.VerifyPassword(password, user.Password))
            {
                return user;
            }

            return null;
        }
    }
}