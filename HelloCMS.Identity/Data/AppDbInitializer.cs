using HelloCMS.Identity.Data.Helpers;
using HelloCMS.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace HelloCMS.Identity.Data
{
    public class AppDbInitializer
    {
        public static async Task SeedRolesToDb(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppIdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Developer))
                    await roleManager.CreateAsync(new AppIdentityRole(UserRoles.Developer));

                if (!await roleManager.RoleExistsAsync(UserRoles.Operation))
                    await roleManager.CreateAsync(new AppIdentityRole(UserRoles.Operation));

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new AppIdentityRole(UserRoles.Admin));

                if (!await roleManager.RoleExistsAsync(UserRoles.Manager))
                    await roleManager.CreateAsync(new AppIdentityRole(UserRoles.Manager));

                if (!await roleManager.RoleExistsAsync(UserRoles.Executive))
                    await roleManager.CreateAsync(new AppIdentityRole(UserRoles.Executive));
            }
        }
    }
}
