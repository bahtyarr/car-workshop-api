using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopSystem.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(CoreDbContext context) : base(context)
        {
        }

        public Task<User?> GetUserByEmail(string email)
        {
            return ReadAsync(delegate (DbSet<User> dbSet)
            {
                return dbSet.Where(item => item.Email == email);
            });
        }

    }
}