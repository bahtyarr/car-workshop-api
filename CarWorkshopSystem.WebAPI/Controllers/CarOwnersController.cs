using System.Security.Claims;
using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using CarWorkshopSystem.WebAPI.ViewModel.Car;
using CarWorkshopSystem.WebAPI.ViewModel.Job;
using CarWorkshopSystem.WebAPI.ViewModel.Repairs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarWorkshopSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarOwnersController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly ICarOwnerRepository _carOwnerRepository;
        private readonly IUserRepository _userRepository;

        public CarOwnersController(ICarOwnerRepository carOwnerRepository, ICarRepository carRepository, IUserRepository userRepository)
        {
            _carRepository = carRepository;
            _carOwnerRepository = carOwnerRepository;
            _userRepository = userRepository;
        }

        [HttpGet("cars")]
        [Authorize(Roles = "CarOwner")]
        public async Task<ActionResult<IEnumerable<CarStatusVm>>> GetCarStatuses()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var carOwner = await _carOwnerRepository.GetByUserIdAsync(userId);
            if (carOwner == null) return NotFound();

            var carStatuses = carOwner.Cars.Select(c => new CarStatusVm
            {
                CarId = c.Id,
                RegistrationNumber = c.RegistrationNumber,
                Model = c.Model,
                Make = c.Make,
                Color = c.Color,
                Repairs = c.Repairs.Select(r => new RepairStatusVm
                {
                    RepairId = r.Id,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Status = r.Status
                }).ToList(),
                Jobs = c.Jobs.Select(j => new JobStatusVm
                {
                    JobId = j.Id,
                    ServiceName = j.Service.Name,
                    MechanicName = j.Mechanic.User.Name,
                    Status = j.Status
                }).ToList()
            }).ToList();

            return Ok(carStatuses);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<CarOwner>>> GetCarOwners()
        {
            var carOwners = await _carOwnerRepository.GetListWithDetailsAsync();
            var results = carOwners.Select(x => new
            {
                id = x.Id,
                userId = x.User.Id,
                name = x.User.Name,
                price = x.User.Email,
            }).ToList();

            return Ok(results);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CarOwner>> GetById(int id)
        {
            var result = await _carOwnerRepository.GetByIdAsync(id);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CarOwner carOwner)
        {
            var result = await _carOwnerRepository.GetByIdAsync(id);

            if (result == null) return NotFound();
            result.UserId = carOwner.UserId;

            await _carOwnerRepository.UpdateAsync(result);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _carOwnerRepository.GetByIdAsync(id);

            if (result == null) return NotFound();

            await _carOwnerRepository.DeleteAsync(result);
            return NoContent();
        }
    }
}