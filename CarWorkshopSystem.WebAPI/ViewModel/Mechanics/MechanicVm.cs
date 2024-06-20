using CarWorkshopSystem.WebAPI.ViewModel.Job;

namespace CarWorkshopSystem.WebAPI.ViewModel.Mechanics
{
    public class MechanicVm
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<JobStatusVm> Jobs { get; set; }
    }
}