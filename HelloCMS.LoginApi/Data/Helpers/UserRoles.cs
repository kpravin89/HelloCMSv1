namespace HelloCMS.LoginApi.Data.Helpers
{
    public static class UserRoles
    {
        public const string Developer = "Developer";
        public const string Operation = "Operation";
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Executive = "Executive";

        public static string? FindByName(string sUserRole)
        {
            return sUserRole switch
            {
                "Developer"  => UserRoles.Developer,
                "Operation" => UserRoles.Operation,
                "Admin" => UserRoles.Admin,
                "Manager" => UserRoles.Manager,
                "Executive" => UserRoles.Executive,
                _ => null
            };
                    
        }

    }
}
