using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using CarWorkshopSystem.WebAPI.ViewModel.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarWorkshopSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;

        public ServicesController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<ActionResult<List<Service>>> GetServiceList()
        {
            var services = await _serviceRepository.GetAllAsync();
            var results = services.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                price = x.Price,
            }).ToList();

            return Ok(results);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Service>> GetById(int id)
        {
            var result = await _serviceRepository.GetByIdAsync(id);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Service>> Create(CreateServiceVm model)
        {
            var service = new Service
            {
                Name = model.Name,
                Price = model.Price,
            };

            var result = await _serviceRepository.AddAsync(service);
            return Ok(new { id = result.Id });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, CreateServiceVm model)
        {
            var result = await _serviceRepository.GetByIdAsync(id);

            if (result == null) return NotFound();

            result.Name = model.Name;
            result.Price = model.Price;

            await _serviceRepository.UpdateAsync(result);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _serviceRepository.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            await _serviceRepository.DeleteAsync(result);
            return NoContent();
        }

    }
}