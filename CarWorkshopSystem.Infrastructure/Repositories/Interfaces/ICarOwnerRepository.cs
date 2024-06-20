using CarWorkshopSystem.Core.Domain;

namespace CarWorkshopSystem.Infrastructure.Repositories.Interfaces
{
    public interface ICarOwnerRepository : IBaseRepository<CarOwner>
    {
        Task<List<CarOwner>> GetListWithDetailsAsync();
        Task<CarOwner> GetByUserIdAsync(string userId);
        Task<CarOwner> GetByCarIdAsync(int carId);
    }
}