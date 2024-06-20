using System.ComponentModel.DataAnnotations;
namespace CarWorkshopSystem.WebAPI.ViewModel.User
{
    public class CreateUserVm
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public string? Role { get; set; }
    }
}