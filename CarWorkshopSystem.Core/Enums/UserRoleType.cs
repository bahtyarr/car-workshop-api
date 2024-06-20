namespace CarWorkshopSystem.Core.Enums
{
    public enum UserRoleType
    {
        Admin,
        CarOwner,
        Mechanic
    }

    public class UserRoleTypeList
    {
        public static Dictionary<string, string> Map()
        {
            var result = new Dictionary<string, string>
        {
            { UserRoleType.Admin.ToString(), UserRoleType.Admin.ToString()},
            { UserRoleType.CarOwner.ToString(), UserRoleType.CarOwner.ToString() },
            { UserRoleType.Mechanic.ToString(), UserRoleType.Mechanic.ToString() },
        };

            return result;
        }
    }
}