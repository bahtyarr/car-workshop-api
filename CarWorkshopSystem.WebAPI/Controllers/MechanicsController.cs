using CarWorkshopSystem.Core.Constant;
using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Core.Enums;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using CarWorkshopSystem.Infrastructure.Security;
using CarWorkshopSystem.WebAPI.ViewModel.Job;
using CarWorkshopSystem.WebAPI.ViewModel.Mechanics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarWorkshopSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MechanicsController : ControllerBase
    {
        private readonly IMechanicRepository _mechanicRepository;
        private readonly IUserRepository _userRepository;

        public MechanicsController(IMechanicRepository mechanicRepository, IUserRepository userRepository)
        {
            _mechanicRepository = mechanicRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Mechanic>> Create(CreateMechanicVm model)
        {
            var user = await _userRepository.GetUserByEmail(model.Email);
            if (user != null && !string.IsNullOrEmpty(model.Password))
            {
                user.Name = model.Name;
                user.Password = PasswordHasher.HashPassword(model.Password);
                await _userRepository.UpdateAsync(user);
                return Ok(new { id = user.UserId });
            }

            user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                Email = model.Email,
                Name = model.Name,
                Password = PasswordHasher.HashPassword(string.IsNullOrEmpty(model.Password) ? PasswordConstant.DefaultPassword : model.Password),
                UserRole = UserRoleType.Mechanic.ToString(),
            };

            user = await _userRepository.AddAsync(user);

            var mechanic = new Mechanic()
            {
                UserId = user.Id,
            };

            await _mechanicRepository.AddAsync(mechanic);

            return Ok(new { id = user.UserId });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<ActionResult<IEnumerable<Mechanic>>> GetMechanicList()
        {
            var mechanics = await _mechanicRepository.GetMechanicList();
            var results = mechanics.Select(item => new MechanicVm
            {
                Id = item.Id,
                Name = item.User.Name,
                Email = item.User.Email,
                UserId = item.User.UserId,
            }).ToList();

            return Ok(results);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Mechanic>> GetDetail(int id)
        {
            var mechanic = await _mechanicRepository.ReadDetailByIdAsync(id);
            if (mechanic == null) return NotFound();

            var response = new MechanicVm
            {
                Id = mechanic.Id,
                Name = mechanic.User.Name,
                Email = mechanic.User.Email,
                UserId = mechanic.User.UserId,
                Jobs = mechanic.Jobs.Select(x => new JobStatusVm
                {
                    JobId = x.Id,
                    ServiceName = x.Service.Name,
                    MechanicName = mechanic.User.Name,
                    Status = x.Status,
                }).ToList(),
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, CreateMechanicVm model)
        {
            var mechanic = await _mechanicRepository.ReadDetailByIdAsync(id);
            if (mechanic == null) return NotFound();

            mechanic.User.Name = model.Name;
            mechanic.User.Email = model.Email;
            mechanic.User.Password = PasswordHasher.HashPassword(string.IsNullOrEmpty(model.Password) ? PasswordConstant.DefaultPassword : model.Password);

            await _mechanicRepository.UpdateAsync(mechanic);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var mechanic = await _mechanicRepository.GetByIdAsync(id);
            if (mechanic == null) return NotFound();

            await _mechanicRepository.DeleteAsync(mechanic);
            return NoContent();
        }
    }
}