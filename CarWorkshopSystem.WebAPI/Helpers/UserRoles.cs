using CarWorkshopSystem.Core.Enums;

namespace CarWorkshopSystem.WebAPI.Helpers
{
    public static class UserRoles
    {
        public static string GenerateUserRoles(string? roles)
        {
            switch (roles)
            {
                case "CarOwner":
                    return UserRoleType.CarOwner.ToString();
                case "Mechanic":
                    return UserRoleType.Mechanic.ToString();
                default:
                    return UserRoleType.Admin.ToString();
            }
        }
    }
}