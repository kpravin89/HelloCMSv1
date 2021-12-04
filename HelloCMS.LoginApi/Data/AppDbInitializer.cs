using HelloCMS.LoginApi.Data.Helpers;
using Microsoft.AspNetCore.Identity;

namespace HelloCMS.LoginApi.Data
{
    public class AppDbInitializer
    {
        public static async Task SeedRolesToDb(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Developer))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Developer));

                if (!await roleManager.RoleExistsAsync(UserRoles.Operation))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Operation));

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await roleManager.RoleExistsAsync(UserRoles.Manager))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));

                if (!await roleManager.RoleExistsAsync(UserRoles.Executive))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Executive));
            }
        }
    }
}
