using AutoMapper;
using HelloCMS.Identity.Data.Models;
using HelloCMS.Identity.Data.ViewModels.Users;

namespace HelloCMS.Identity.Infrastructure.Automapper
{
    public class MappingProfile
        : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<RegisterUserVM, AppIdentityUser>();
            CreateMap<AppIdentityUser, RegisterUserVM>();


            CreateMap<SelectUserVM, AppIdentityUser>();
            CreateMap<AppIdentityUser, SelectUserVM>();
        }
    }
}
