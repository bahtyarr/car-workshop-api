using CarWorkshopSystem.Core.Domain;

namespace CarWorkshopSystem.Infrastructure.Repositories.Interfaces
{
    public interface IJobRepository : IBaseRepository<Job>
    {
        Task<List<Job>> GetListJobsAsync();
        Task<List<Job>> GetListJobsByMechanicAsync(string mechanicUserId);
        Task<Job> GetDetailJobAsync(int id);
    }
}