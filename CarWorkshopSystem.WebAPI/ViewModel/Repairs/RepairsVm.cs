namespace CarWorkshopSystem.WebAPI.ViewModel.Repairs
{
    public class RepairsVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CarRegistrationNumber { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
        public string Services { get; set; }
    }
}