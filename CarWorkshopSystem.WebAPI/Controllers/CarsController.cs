using CarWorkshopSystem.Core.Constant;
using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Core.Enums;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using CarWorkshopSystem.Infrastructure.Security;
using CarWorkshopSystem.WebAPI.ViewModel.Car;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarWorkshopSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;

        public CarsController(ICarRepository carRepository, IUserRepository userRepository)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Car>> Create(CreateCarVm model)
        {
            var car = await _carRepository.ReadByRegistrationNumberAsync(model.RegistrationNumber);
            if (car == null)
            {
                var createUser = await this.CreateUser(model);
                car = new Car()
                {
                    Owner = new CarOwner
                    {
                        UserId = createUser.Id,
                    },
                    RegistrationNumber = model.RegistrationNumber,
                    Model = model.Model,
                    Make = model.Make,
                    Color = model.Color,
                    Repairs = new List<Repair>(),
                };

                car = await _carRepository.AddAsync(car);
            }

            var repair = new Repair
            {
                CarId = car.Id,
                StartDate = model.StartDate,
                Status = GeneralProcessStatusType.Waiting.ToString(),
            };

            car.Repairs.Add(repair);
            await _carRepository.UpdateAsync(car);

            return Ok(new { id = car.Id });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CarDetailVm>> GetById(int id)
        {
            var result = await _carRepository.ReadDetailByIdAsync(id);
            if (result == null) return NotFound();

            var response = new CarDetailVm
            {
                Id = result.Id,
                RegistrationNumber = result.RegistrationNumber,
                Model = result.Model,
                Make = result.Make,
                Color = result.Color,
            };

            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<ActionResult<List<Car>>> GetCarList()
        {
            var cars = await _carRepository.GetListAsync();
            var results = cars.Select(x => new
            {
                id = x.Id,
                name = $"{x.Model} - {x.RegistrationNumber}",
                ownerName = x.Owner.User.Name,
                ownerEmail = x.Owner.User.Email,
                registrationNumber = x.RegistrationNumber,
                model = x.Model,
                color = x.Color,
            }).ToList();

            return Ok(results);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, UpdateCarVm model)
        {
            var car = await _carRepository.ReadDetailByIdAsync(id);
            if (car == null) return NotFound();

            car.RegistrationNumber = model.RegistrationNumber;
            car.Color = model.Color;
            car.Model = model.Model;
            car.Make = model.Make;

            await _carRepository.UpdateAsync(car);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var job = await _carRepository.GetByIdAsync(id);
            if (job == null) return NotFound();

            await _carRepository.DeleteAsync(job);
            return NoContent();
        }


        #region PrivateMethods

        private async Task<User> CreateUser(CreateCarVm model)
        {
            var readUser = await _userRepository.GetUserByEmail(model.Email);
            if (readUser != null)
            {
                return readUser;
            }

            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                Name = model.OwnerName,
                Email = model.Email,
                Password = PasswordHasher.HashPassword(string.IsNullOrEmpty(model.Password) ? PasswordConstant.DefaultPassword : model.Password),
                UserRole = UserRoleType.CarOwner.ToString(),
            };

            var result = await _userRepository.AddAsync(user);
            return result;
        }

        #endregion PrivateMethods
    }

}