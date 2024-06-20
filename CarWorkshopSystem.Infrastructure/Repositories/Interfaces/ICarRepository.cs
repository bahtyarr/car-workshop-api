using CarWorkshopSystem.Core.Domain;

namespace CarWorkshopSystem.Infrastructure.Repositories.Interfaces
{
    public interface ICarRepository : IBaseRepository<Car>
    {
        Task<List<Car>> GetListAsync();
        Task<Car> ReadByRegistrationNumberAsync(string registrationNumber);
        Task<Car> ReadDetailByIdAsync(int id);
    }
}