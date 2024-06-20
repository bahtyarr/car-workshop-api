using System.ComponentModel.DataAnnotations;

namespace CarWorkshopSystem.WebAPI.ViewModel.Car
{
    public class UpdateCarVm
    {
        [Required] public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string Color { get; set; }
    }
}