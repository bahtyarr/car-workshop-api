using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopSystem.Infrastructure.Repositories
{
    public class JobRepository : BaseRepository<Job>, IJobRepository
    {
        public JobRepository(CoreDbContext context) : base(context)
        {

        }

        public async Task<List<Job>> GetListJobsAsync()
        {
            return (await SearchAsync(delegate (DbSet<Job> dbSet)
            {
                return dbSet
                    .Include(item => item.Car).ThenInclude(x => x.Jobs).ThenInclude(x => x.Service)
                    .Include(item => item.Car).ThenInclude(x => x.Owner).ThenInclude(x => x.User)
                    .Include(item => item.Mechanic).ThenInclude(x => x.User)
                    .Include(item => item.Service);
            })).ToList();
        }

        public async Task<List<Job>> GetListJobsByMechanicAsync(string mechanicUserId)
        {
            return (await SearchAsync(delegate (DbSet<Job> dbSet)
            {
                return dbSet
                    .Include(item => item.Car).ThenInclude(x => x.Jobs).ThenInclude(x => x.Service)
                    .Include(item => item.Car).ThenInclude(x => x.Owner).ThenInclude(x => x.User)
                    .Include(item => item.Mechanic).ThenInclude(x => x.User)
                    .Include(item => item.Service)
                    .Where(item => item.Mechanic.User.UserId == mechanicUserId);
            })).ToList();
        }

        public Task<Job?> GetDetailJobAsync(int id)
        {
            return ReadAsync(delegate (DbSet<Job> dbSet)
            {
                return dbSet
                    .Include(item => item.Car).ThenInclude(x => x.Jobs).ThenInclude(x => x.Service)
                    .Include(item => item.Car).ThenInclude(x => x.Owner).ThenInclude(x => x.User)
                    .Include(item => item.Mechanic).ThenInclude(x => x.User)
                    .Include(item => item.Service)
                    .Where(item => item.Id == id);
            });
        }
    }
}