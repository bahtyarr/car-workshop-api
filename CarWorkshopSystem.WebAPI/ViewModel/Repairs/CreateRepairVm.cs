namespace CarWorkshopSystem.WebAPI.ViewModel.Repairs
{
    public class CreateRepairVm
    {
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
    }
}