using CarWorkshopSystem.Core.Domain;

namespace CarWorkshopSystem.Infrastructure.Repositories.Interfaces
{
    public interface IMechanicRepository : IBaseRepository<Mechanic>
    {
        Task<List<Mechanic>> GetMechanicList();
        Task<Mechanic> ReadDetailByIdAsync(int id);
    }
}