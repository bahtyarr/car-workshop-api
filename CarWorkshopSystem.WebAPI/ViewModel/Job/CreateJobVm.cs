namespace CarWorkshopSystem.WebAPI.ViewModel.Job
{
    public class CreateJobVm
    {
        public int CarId { get; set; }
        public int ServiceId { get; set; }
        public int MechanicId { get; set; }
        public string Status { get; set; }
    }
}