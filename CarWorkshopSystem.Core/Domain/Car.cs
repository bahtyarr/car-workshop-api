namespace CarWorkshopSystem.Core.Domain
{
    public class Car : EntityCore
    {
        public int OwnerId { get; set; }
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string Color { get; set; }


        public CarOwner Owner { get; set; }
        public ICollection<Job> Jobs { get; set; }
        public ICollection<Repair> Repairs { get; set; }
    }
}