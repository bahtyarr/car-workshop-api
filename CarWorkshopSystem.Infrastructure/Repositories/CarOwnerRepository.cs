using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopSystem.Infrastructure.Repositories
{
    public class CarOwnerRepository : BaseRepository<CarOwner>, ICarOwnerRepository
    {
        public CarOwnerRepository(CoreDbContext context) : base(context)
        {
        }

        public async Task<List<CarOwner>> GetListWithDetailsAsync()
        {
            return (await SearchAsync(delegate (DbSet<CarOwner> dbSet)
            {
                return dbSet.Include(item => item.User);
            })).ToList();
        }

        public Task<CarOwner?> GetByUserIdAsync(string userId)
        {
            return ReadAsync(delegate (DbSet<CarOwner> dbSet)
            {
                return dbSet
                    .Include(item => item.User)
                    .Include(item => item.Cars).ThenInclude(x => x.Jobs).ThenInclude(y => y.Service)
                    .Include(item => item.Cars).ThenInclude(x => x.Jobs).ThenInclude(y => y.Mechanic).ThenInclude(z => z.User)
                    .Include(item => item.Cars).ThenInclude(x => x.Repairs)
                    .Where(item => item.User.UserId == userId);
            });
        }

        public Task<CarOwner?> GetByCarIdAsync(int carId)
        {
            return ReadAsync(delegate (DbSet<CarOwner> dbSet)
           {
               return dbSet
                   .Include(item => item.User)
                   .Include(item => item.Cars).ThenInclude(x => x.Jobs)
                   .Include(item => item.Cars).ThenInclude(x => x.Repairs)
                   .Where(item => item.Cars.Any(x => x.Id == carId));
           });
        }
    }
}