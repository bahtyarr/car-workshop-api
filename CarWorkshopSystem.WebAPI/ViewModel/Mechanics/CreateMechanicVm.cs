using System.ComponentModel.DataAnnotations;

namespace CarWorkshopSystem.WebAPI.ViewModel.Mechanics
{
    public class CreateMechanicVm
    {
        [Required] public string Name { get; set; }
        [Required] public string Email { get; set; }
        public string? Password { get; set; }
    }
}