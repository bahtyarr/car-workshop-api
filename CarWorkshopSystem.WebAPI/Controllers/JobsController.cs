using CarWorkshopSystem.Core.Domain;
using CarWorkshopSystem.Core.Enums;
using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using CarWorkshopSystem.WebAPI.Utility;
using CarWorkshopSystem.WebAPI.ViewModel.Job;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarWorkshopSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMechanicRepository _mechanicRepository;
        private readonly ICarOwnerRepository _carOwnerRepository;
        private readonly EmailService _emailService;

        public JobsController(
            IJobRepository jobRepository,
            IMechanicRepository mechanicRepository,
            ICarOwnerRepository carOwnerRepository,
            EmailService emailService)
        {
            _jobRepository = jobRepository;
            _mechanicRepository = mechanicRepository;
            _carOwnerRepository = carOwnerRepository;
            _emailService = emailService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<ActionResult<IEnumerable<JobDetailVm>>> GetJobs()
        {
            var jobs = await _jobRepository.GetListJobsAsync();
            var jobDetails = jobs.Select(j => new JobDetailVm
            {
                Id = j.Id,
                CarId = j.CarId,
                ServiceId = j.ServiceId,
                MechanicId = j.MechanicId,
                Status = j.Status,
                CarOwner = j.Car.Owner.User.Name,
                CarModel = j.Car.Model,
                CarNumber = j.Car.RegistrationNumber,
                ServiceName = j.Service.Name,
                MechanicName = j.Mechanic.User.Name
            }).ToList();

            return Ok(jobDetails);
        }

        [HttpGet("mechanic/{mechanicUserId}")]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<ActionResult<IEnumerable<JobDetailVm>>> GetJobsByMechanic(string mechanicUserId)
        {
            var jobs = await _jobRepository.GetListJobsByMechanicAsync(mechanicUserId);
            var jobDetails = jobs.Select(j => new JobDetailVm
            {
                Id = j.Id,
                CarId = j.CarId,
                ServiceId = j.ServiceId,
                MechanicId = j.MechanicId,
                Status = j.Status,
                CarOwner = j.Car.Owner.User.Name,
                CarModel = j.Car.Model,
                CarNumber = j.Car.RegistrationNumber,
                ServiceName = j.Service.Name,
                MechanicName = j.Mechanic.User.Name
            }).ToList();

            return Ok(jobDetails);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<ActionResult<JobDetailVm>> GetJobDetail(int id)
        {
            var job = await _jobRepository.GetDetailJobAsync(id);
            var jobDetail = new JobDetailVm
            {
                Id = job.Id,
                CarId = job.CarId,
                ServiceId = job.ServiceId,
                MechanicId = job.MechanicId,
                Status = job.Status,
                CarOwner = job.Car.Owner.User.Name,
                CarModel = job.Car.Model,
                CarNumber = job.Car.RegistrationNumber,
                ServiceName = job.Service.Name,
                MechanicName = job.Mechanic.User.Name
            };

            return Ok(jobDetail);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Job>> Create(CreateJobVm model)
        {
            var job = new Job
            {
                CarId = model.CarId,
                ServiceId = model.ServiceId,
                MechanicId = model.MechanicId,
                Status = model.Status
            };

            await _jobRepository.AddAsync(job);
            await this.SendEmailJobNotif(model);

            return Ok(new { id = job.Id });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<IActionResult> UpdateJob(int id, UpdateJobStatusVm model)
        {
            var job = await _jobRepository.GetByIdAsync(id);
            if (job == null) return NotFound();

            job.Status = model.Status;
            await _jobRepository.UpdateAsync(job);
            await this.SendUpdateJobNotif(job);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _jobRepository.GetByIdAsync(id);
            if (job == null) return NotFound();

            await _jobRepository.DeleteAsync(job);
            return NoContent();
        }

        #region Private Methods

        private async Task SendUpdateJobNotif(Job job)
        {
            var model = new CreateJobVm
            {
                CarId = job.Id,
                MechanicId = job.MechanicId,
                ServiceId = job.ServiceId,
                Status = job.Status,
            };

            await this.SendEmailJobNotif(model);
        }

        private async Task SendEmailJobNotif(CreateJobVm jobModel)
        {
            var carOwner = await _carOwnerRepository.GetByCarIdAsync(jobModel.CarId);
            var mechanic = await _mechanicRepository.ReadDetailByIdAsync(jobModel.MechanicId);

            if (carOwner != null && mechanic != null)
            {
                var selectedCar = carOwner.Cars.FirstOrDefault(x => x.Id == jobModel.CarId);

                var subject = "Car Workshop Job Notification";
                var messageToOwner = jobModel.Status == GeneralProcessStatusType.Progress.ToString() ?
                    $"Your car is currently being worked on and is in [{jobModel.Status}] status, worked on by mechanic [{mechanic.User.Name}]" :
                    $"Your car is currently in [{jobModel.Status}] job status, assigned mechanic: [{mechanic.User.Name}]";
                var messageToMechanic = $"You have assigned a job for a car model [{selectedCar.Model}] with number plate [{selectedCar.RegistrationNumber}]";

                await _emailService.SendEmailAsync(carOwner.User.Email, subject, messageToOwner);

                if (jobModel.Status == GeneralProcessStatusType.Progress.ToString())
                    await _emailService.SendEmailAsync(mechanic.User.Email, subject, messageToMechanic);
            }
        }

        #endregion
    }
}