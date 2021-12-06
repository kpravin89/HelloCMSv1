namespace HelloCMS.Identity.Infrastructure.ServiceRegistration
{
    public static class RegisterRepositoriesExtension
    {
        public static IServiceCollection Repositories(this IServiceCollection services,
                                                IConfiguration configuration)
        {

            return services;
        }
    }
}
