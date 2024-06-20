using CarWorkshopSystem.WebAPI.ViewModel.Service;

namespace CarWorkshopSystem.WebAPI.ViewModel.Repairs
{
    public class RepairDetailVm
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string OwnerName { get; set; }
        public string CarRegistrationNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public List<ServiceVm> Services { get; set; }
    }
}