namespace CarWorkshopSystem.WebAPI.ViewModel.Job
{
    public class JobDetailVm
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int ServiceId { get; set; }
        public int MechanicId { get; set; }
        public string Status { get; set; }
        public string CarOwner { get; set; }
        public string CarModel { get; set; }
        public string CarNumber { get; set; }
        public string ServiceName { get; set; }
        public string MechanicName { get; set; }
    }
}