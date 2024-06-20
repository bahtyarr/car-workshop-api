using CarWorkshopSystem.WebAPI.ViewModel.Job;
using CarWorkshopSystem.WebAPI.ViewModel.Repairs;

namespace CarWorkshopSystem.WebAPI.ViewModel.Car
{
    public class CarStatusVm
    {
        public int CarId { get; set; }
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string Color { get; set; }
        public List<RepairStatusVm> Repairs { get; set; }
        public List<JobStatusVm> Jobs { get; set; }
    }
}