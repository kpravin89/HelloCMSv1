﻿namespace HelloCMS.Identity.Data.Helpers
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
                "Developer" => Developer,
                "Operation" => Operation,
                "Admin" => Admin,
                "Manager" => Manager,
                "Executive" => Executive,
                _ => null
            };

        }

    }
}
