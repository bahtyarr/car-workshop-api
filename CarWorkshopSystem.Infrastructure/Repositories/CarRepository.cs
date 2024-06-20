using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopSystem.Infrastructure.Repositories
{
    public class CarRepository : BaseRepository<Car>, ICarRepository
    {
        public CarRepository(CoreDbContext context) : base(context)
        {

        }

        public async Task<List<Car>> GetListAsync()
        {
            return (await SearchAsync(delegate (DbSet<Car> dbSet)
           {
               return dbSet.Include(item => item.Owner).ThenInclude(x => x.User);
           })).ToList();
        }

        public Task<Car?> ReadByRegistrationNumberAsync(string registrationNumber)
        {
            return ReadAsync(delegate (DbSet<Car> dbSet)
            {
                return dbSet
                    .Include(item => item.Owner).ThenInclude(x => x.User)
                    .Include(item => item.Repairs)
                    .Include(item => item.Jobs).ThenInclude(x => x.Service)
                    .Where(item => item.RegistrationNumber == registrationNumber);
            });
        }

        public Task<Car?> ReadDetailByIdAsync(int id)
        {
            return ReadAsync(delegate (DbSet<Car> dbSet)
            {
                return dbSet
                    .Include(item => item.Owner).ThenInclude(x => x.User)
                    .Include(item => item.Repairs)
                    .Include(item => item.Jobs).ThenInclude(x => x.Service)
                    .Where(item => item.Id == id);
            });
        }
    }
}