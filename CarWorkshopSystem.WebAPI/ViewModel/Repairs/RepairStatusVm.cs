namespace CarWorkshopSystem.WebAPI.ViewModel.Repairs
{
    public class RepairStatusVm
    {
        public int RepairId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
    }
}