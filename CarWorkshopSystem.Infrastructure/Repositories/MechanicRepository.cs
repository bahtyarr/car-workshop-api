using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopSystem.Infrastructure.Repositories
{
    public class MechanicRepository : BaseRepository<Mechanic>, IMechanicRepository
    {
        public MechanicRepository(CoreDbContext context) : base(context)
        {
        }

        public async Task<List<Mechanic>> GetMechanicList()
        {
            return (await SearchAsync(delegate (DbSet<Mechanic> dbSet)
            {
                return dbSet
                    .Include(item => item.User)
                    .Include(item => item.Jobs).ThenInclude(x => x.Service);
            })).ToList();
        }

        public Task<Mechanic?> ReadDetailByIdAsync(int id)
        {
            return ReadAsync(delegate (DbSet<Mechanic> dbSet)
            {
                return dbSet
                    .Include(item => item.User)
                    .Include(item => item.Jobs).ThenInclude(x => x.Service)
                    .Where(item => item.Id == id);
            });
        }
    }
}