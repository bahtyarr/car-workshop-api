using System.Diagnostics.CodeAnalysis;
using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Core.Enums;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using CarWorkshopSystem.Infrastructure.Security;
using CarWorkshopSystem.Infrastructure.Services.Interface;
using CarWorkshopSystem.WebAPI.Helpers;
using CarWorkshopSystem.WebAPI.Utility;
using CarWorkshopSystem.WebAPI.ViewModel.Login;
using CarWorkshopSystem.WebAPI.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CarWorkshopSystem.WebAPI.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly IMechanicRepository _mechanicRepository;
        private readonly ICarOwnerRepository _carOwnerRepository;

        public AccountsController(
            IUserService userService,
            JwtService jwtService,
            IUserRepository userRepository,
            IMechanicRepository mechanicRepository,
            ICarOwnerRepository carOwnerRepository)
        {
            _userService = userService;
            _jwtService = jwtService;
            _userRepository = userRepository;
            _mechanicRepository = mechanicRepository;
            _carOwnerRepository = carOwnerRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginVm model)
        {
            var user = await _userService.Authenticate(model.Email, model.Password);
            if (user == null)
                return Unauthorized();

            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        [HttpPost("create")]
        public async Task<ActionResult<User>> CreateUser(CreateUserVm model)
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

            return Ok(new { id = result.Id });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new { id = user.Id, name = user.Name, email = user.Email });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            if (!users.Any())
            {
                return NotFound();
            }

            var result = users.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                email = x.Email
            }).ToList();

            return Ok(result);
        }

        #region Private Methods

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