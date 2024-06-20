namespace CarWorkshopSystem.Core.Domain
{
    public class CarOwner : EntityCore
    {
        public int UserId { get; set; }

        public User User { get; set; }
        public ICollection<Car> Cars { get; set; }
    }
}