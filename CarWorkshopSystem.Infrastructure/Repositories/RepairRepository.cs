using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopSystem.Infrastructure.Repositories
{
    public class RepairRepository : BaseRepository<Repair>, IRepairRepository
    {
        public RepairRepository(CoreDbContext context) : base(context)
        {
        }

        public async Task<List<Repair>> GetListRepairsAsync()
        {
            return (await SearchAsync(delegate (DbSet<Repair> dbSet)
            {
                return dbSet
                    .Include(item => item.Car).ThenInclude(x => x.Jobs).ThenInclude(x => x.Service)
                    .Include(item => item.Car).ThenInclude(x => x.Owner).ThenInclude(x => x.User);
            })).ToList();
        }

        public Task<Repair?> GetDetailRepairAsync(int id)
        {
            return ReadAsync(delegate (DbSet<Repair> dbSet)
            {
                return dbSet
                    .Include(item => item.Car).ThenInclude(x => x.Jobs).ThenInclude(x => x.Service)
                    .Include(item => item.Car).ThenInclude(x => x.Jobs).ThenInclude(x => x.Mechanic).ThenInclude(x => x.User)
                    .Include(item => item.Car).ThenInclude(x => x.Owner).ThenInclude(x => x.User)
                    .Where(item => item.Id == id);
            });
        }
    }
}