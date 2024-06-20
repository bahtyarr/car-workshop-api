namespace CarWorkshopSystem.Core.Domain
{
    public class Mechanic : EntityCore
    {
        public int UserId { get; set; }

        public User User { get; set; }
        public ICollection<Job> Jobs { get; set; }
    }
}