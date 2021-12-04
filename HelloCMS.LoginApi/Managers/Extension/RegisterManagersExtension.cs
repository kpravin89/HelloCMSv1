namespace HelloCMS.LoginApi.Managers
{
    public static class RegisterManagersExtension
    {
        public static IServiceCollection AddManagers(this IServiceCollection services, 
                                                IConfiguration configuration)
        {
            services.AddScoped<LoginManager>();

            return services;
        }
    }
}
