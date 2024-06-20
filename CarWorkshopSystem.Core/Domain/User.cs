namespace CarWorkshopSystem.Core.Domain
{
    public class User : EntityCore
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
    }
}