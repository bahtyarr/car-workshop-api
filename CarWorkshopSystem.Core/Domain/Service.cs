namespace CarWorkshopSystem.Core.Domain
{
    public class Service : EntityCore
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<Job> Jobs { get; set; }
    }
}