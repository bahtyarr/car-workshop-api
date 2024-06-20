using System.Diagnostics.CodeAnalysis;
using CarWorkshopSystem.Core.Constant;
using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Core.Enums;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using CarWorkshopSystem.Infrastructure.Security;
using CarWorkshopSystem.WebAPI.Helpers;
using CarWorkshopSystem.WebAPI.ViewModel.Car;
using CarWorkshopSystem.WebAPI.ViewModel.Job;
using CarWorkshopSystem.WebAPI.ViewModel.User;
using Microsoft.AspNetCore.Mvc;

namespace CarWorkshopSystem.WebAPI.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SystemsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMechanicRepository _mechanicRepository;
        private readonly ICarOwnerRepository _carOwnerRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ICarRepository _carRepository;
        private readonly IJobRepository _jobRepository;

        public SystemsController(
            IUserRepository userRepository,
            IMechanicRepository mechanicRepository,
            ICarOwnerRepository carOwnerRepository,
            IServiceRepository serviceRepository,
            ICarRepository carRepository,
            IJobRepository jobRepository)
        {
            _userRepository = userRepository;
            _mechanicRepository = mechanicRepository;
            _carOwnerRepository = carOwnerRepository;
            _serviceRepository = serviceRepository;
            _carRepository = carRepository;
            _jobRepository = jobRepository;
        }

        /// <summary>
        /// Please run this only one time
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SeedData()
        {
            #region create Admin

            var createAdminUser = new CreateUserVm
            {
                Name = "Admin",
                Email = "admin@mailtrap.io",
                Password = PasswordConstant.DefaultPassword
            };

            await this.CreateUser(createAdminUser);

            #endregion create Admin

            #region create Mechanic

            var createMechanicUser = new CreateUserVm
            {
                Name = "Mechanic Ganteng",
                Email = "mechan@mailtrap.io",
                Password = PasswordConstant.DefaultPassword,
                Role = UserRoleType.Mechanic.ToString()
            };

            await this.CreateUser(createMechanicUser);

            #endregion create Mechanic

            #region Create Services

            var services = new List<Service>
            {
                new Service
                {
                    Name = "Change tyres",
                    Price = 150000
                },
                new Service
                {
                    Name = "Fix dents",
                    Price = 1350000
                },
                new Service
                {
                    Name = "Change battery",
                    Price = 3150000
                }
            };

            await this.CreateServices(services);

            #endregion create Services

            #region create Car

            var carVm = new CreateCarVm
            {
                RegistrationNumber = "DK 01 AA",
                Model = "Pajero Sport",
                Make = "Mitsubishi",
                Color = "Black",
                OwnerName = "Bapak Budi",
                Email = "budi@mailtrap.io",
                Password = PasswordConstant.DefaultPassword,
                StartDate = DateTime.UtcNow,
            };

            await this.CreateCar(carVm);

            #endregion create Car

            #region create Job

            var readCars = await _carRepository.GetAllAsync();
            var readServices = await _serviceRepository.GetAllAsync();
            var readMechanics = await _mechanicRepository.GetAllAsync();

            if (readCars.Any() && readServices.Any() && readMechanics.Any())
            {
                var jobVm = new CreateJobVm()
                {
                    CarId = readCars.FirstOrDefault().Id,
                    MechanicId = readMechanics.FirstOrDefault().Id,
                    ServiceId = readServices.FirstOrDefault().Id,
                    Status = GeneralProcessStatusType.Progress.ToString(),
                };

                await this.CreateJob(jobVm);
            }

            #endregion create Job

            return Ok();
        }

        #region Private Methods

        private async Task CreateUser(CreateUserVm model)
        {
            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                Email = model.Email,
                Name = model.Name,
                Password = PasswordHasher.HashPassword(model.Password),
                UserRole = UserRoles.GenerateUserRoles(model.Role),
            };

            var result = await _userRepository.AddAsync(user);
            await this.AssignUserToRole(result);
        }

        private async Task CreateServices(List<Service> models)
        {
            await _serviceRepository.BulkCreateAsync(models);
        }

        private async Task CreateCar(CreateCarVm model)
        {
            var car = await _carRepository.ReadByRegistrationNumberAsync(model.RegistrationNumber);
            if (car == null)
            {
                var createUser = await this.CreateCarUser(model);
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
        }

        private async Task<User> CreateCarUser(CreateCarVm model)
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

        private async Task CreateJob(CreateJobVm model)
        {
            var job = new Job
            {
                CarId = model.CarId,
                ServiceId = model.ServiceId,
                MechanicId = model.MechanicId,
                Status = model.Status
            };

            await _jobRepository.AddAsync(job);
        }

        private async Task AssignUserToRole(User user)
        {
            if (user.UserRole == UserRoleType.CarOwner.ToString())
            {
                var carOwner = new CarOwner { UserId = user.Id };
                await _carOwnerRepository.AddAsync(carOwner);
            }
            else if (user.UserRole == UserRoleType.Mechanic.ToString())
            {
                var mechanic = new Mechanic { UserId = user.Id };
                await _mechanicRepository.AddAsync(mechanic);
            }
        }

        #endregion

    }
}