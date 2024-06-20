namespace CarWorkshopSystem.WebAPI.ViewModel.Job
{
    public class JobStatusVm
    {
        public int JobId { get; set; }
        public string ServiceName { get; set; }
        public string MechanicName { get; set; }
        public string Status { get; set; }
    }
}