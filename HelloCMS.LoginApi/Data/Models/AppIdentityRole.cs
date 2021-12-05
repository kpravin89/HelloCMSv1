using Microsoft.AspNetCore.Identity;

namespace HelloCMS.Identity.Data.Models
{
    public class AppIdentityRole : IdentityRole<int>
    {
        public AppIdentityRole(string name)
        {
            base.Name = name;
        }

    }
}
