using HelloCMS.Identity.Data.Dto.Users;

namespace HelloCMS.Identity.Services
{
    public interface IUsersServices
    {
        Task<SelectUserDto> RegisterAsync(RegisterUserDto registerVM);
        Task<SelectUserDto?> GetUser(int Id);
        Task<SelectUserDto?> GetUser(string usernameOrEmail);
        Task<List<SelectUserDto>?> GetUsers(string? Roles);
        Task Update(int Id, UpdateUserDto updateVM);
        Task Delete(int Id);

    }
}
