namespace CarWorkshopSystem.Core.Domain
{
    public class Repair : EntityCore
    {
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }

        public Car Car { get; set; }
    }
}