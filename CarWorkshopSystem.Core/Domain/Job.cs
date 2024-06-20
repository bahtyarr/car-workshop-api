namespace CarWorkshopSystem.Core.Domain
{
    public class Job : EntityCore
    {
        public int CarId { get; set; }
        public int ServiceId { get; set; }
        public int MechanicId { get; set; }
        public string Status { get; set; }

        public Mechanic Mechanic { get; set; }
        public Car Car { get; set; }
        public Service Service { get; set; }
    }
}