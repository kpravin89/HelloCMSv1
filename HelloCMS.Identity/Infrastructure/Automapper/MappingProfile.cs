using AutoMapper;
using HelloCMS.Identity.Data.Dto.Users;
using HelloCMS.Identity.Data.Models;

namespace HelloCMS.Identity.Infrastructure.Automapper
{
    public class MappingProfile
        : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<RegisterUserDto, AppIdentityUser>();
            CreateMap<AppIdentityUser, RegisterUserDto>();


            CreateMap<SelectUserDto, AppIdentityUser>();
            CreateMap<AppIdentityUser, SelectUserDto>();
        }
    }
}
