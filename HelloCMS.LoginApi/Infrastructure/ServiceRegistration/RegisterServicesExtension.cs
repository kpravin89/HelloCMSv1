using HelloCMS.Identity.Services;

namespace HelloCMS.Identity.Infrastructure.ServiceRegistration
{
    public static class RegisterServicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services,
                                                IConfiguration configuration)
        {
            services.AddScoped<IUsersServices, UsersServices>();
            services.AddScoped<ILoginServices, LoginServices>();

            return services;
        }
    }
}
