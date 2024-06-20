using System.ComponentModel.DataAnnotations;

namespace CarWorkshopSystem.WebAPI.ViewModel.Car
{
    public class CreateCarVm
    {
        #region Car

        [Required] public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string Color { get; set; }

        #endregion Car

        #region User

        [Required] public string OwnerName { get; set; }
        [Required] public string Email { get; set; }
        public string? Password { get; set; }

        #endregion User

        #region Repair

        [Required] public DateTime StartDate { get; set; }

        #endregion

    }
}