using CarWorkshopSystem.Core.Domain;

namespace CarWorkshopSystem.Infrastructure.Repositories.Interfaces
{
    public interface IRepairRepository : IBaseRepository<Repair>
    {
        Task<List<Repair>> GetListRepairsAsync();
        Task<Repair> GetDetailRepairAsync(int id);
    }
}