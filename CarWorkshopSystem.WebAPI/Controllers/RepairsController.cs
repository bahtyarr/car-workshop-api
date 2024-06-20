using System.Globalization;
using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Core.Enums;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using CarWorkshopSystem.WebAPI.Utility;
using CarWorkshopSystem.WebAPI.ViewModel.Repairs;
using CarWorkshopSystem.WebAPI.ViewModel.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarWorkshopSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairsController : ControllerBase
    {
        private readonly IRepairRepository _repairRepository;
        private readonly EmailService _emailService;

        public RepairsController(IRepairRepository repairRepository, EmailService emailService)
        {
            _repairRepository = repairRepository;
            _emailService = emailService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<ActionResult<List<Repair>>> GetRepairList()
        {
            var repairs = await _repairRepository.GetListRepairsAsync();
            var results = repairs.Select(x => new RepairsVm
            {
                Id = x.Id,
                Name = x.Car.Owner.User.Name,
                CarRegistrationNumber = x.Car.RegistrationNumber,
                StartDate = x.StartDate,
                Status = x.Status,
                Services = string.Join(", ", x.Car.Jobs.Select(x => x.Service.Name)),
            }).ToList();

            return Ok(results);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<ActionResult<Repair>> GetById(int id)
        {
            var result = await _repairRepository.GetDetailRepairAsync(id);

            if (result == null) return NotFound();

            var repairDetail = new RepairDetailVm
            {
                Id = result.Id,
                CarId = result.CarId,
                OwnerName = result.Car.Owner.User.Name,
                CarRegistrationNumber = result.Car.RegistrationNumber,
                StartDate = result.StartDate,
                EndDate = result.EndDate,
                Status = result.Status,
                Services = result.Car.Jobs.Select(j => new ServiceVm
                {
                    Id = j.ServiceId,
                    Name = j.Service.Name,
                    Price = j.Service.Price,
                    Status = j.Status
                }).ToList()
            };

            return Ok(repairDetail);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<ActionResult<Repair>> Create(CreateRepairVm model)
        {
            var repair = new Repair
            {
                CarId = model.CarId,
                StartDate = model.StartDate,
                Status = model.Status
            };

            var result = await _repairRepository.AddAsync(repair);
            return Ok(new { id = result.Id });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<IActionResult> Update(int id, CreateRepairVm model)
        {
            var repair = await _repairRepository.GetDetailRepairAsync(id);
            if (repair == null) return NotFound();

            repair.Status = model.Status;
            if (model.Status == GeneralProcessStatusType.Completed.ToString())
            {
                repair.EndDate = DateTime.Now;
            }

            await _repairRepository.UpdateAsync(repair);
            await this.SendEmailRepairNotif(repair);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var repair = await _repairRepository.GetByIdAsync(id);
            if (repair == null) return NotFound();

            await _repairRepository.DeleteAsync(repair);
            return NoContent();
        }


        #region Private methods

        private async Task SendEmailRepairNotif(Repair repair)
        {
            var carOwner = repair.Car.Owner;

            if (carOwner != null)
            {
                var selectedCar = repair.Car;
                var servicesList = new List<ServiceVm>();
                servicesList.AddRange(selectedCar.Jobs.Select(x => new ServiceVm
                {
                    Name = x.Service.Name,
                    Price = x.Service.Price,
                }));

                var totalCost = servicesList.Sum(x => x.Price);
                var formattedTotalCost = totalCost.ToString("#,##0.00", CultureInfo.InvariantCulture);

                var subject = "Car Workshop Repair Notification";

                var messageToOwner = $@"
                    <html>
                    <body>
                    <h2>Your car repair status: {repair.Status}</h2>
                    <p>
                        Your car repair has been marked as <strong>{repair.Status}</strong>. Below are the details:
                    </p>
                    <table border='1'>
                    <thead>
                        <tr>
                            <th>Service</th>
                            <th>Price</th>
                        </tr>
                    </thead>
                    <tbody>";

                foreach (var service in servicesList)
                {
                    messageToOwner += $@"
                        <tr>
                        <td>{service.Name}</td>
                        <td>Rp. {service.Price.ToString("#,##0.00", CultureInfo.InvariantCulture)}</td>
                        </tr>";
                }

                messageToOwner += $@"
                <tr>
                    <td><strong>Total</strong></td>
                    <td>Rp. {formattedTotalCost}</td>
                </tr>
                </tbody>
                </table>
                <p>Thank you for choosing our workshop!</p>
                </body>
                </html>";

                await _emailService.SendEmailAsync(carOwner.User.Email, subject, messageToOwner);
            }
        }

        #endregion
    }
}