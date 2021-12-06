using HelloCMS.Identity.Data.Dto.Users;

namespace HelloCMS.Identity.Services
{
    public interface IUsersServices
    {
        Task<SelectUserDto> RegisterAsync(RegisterUserDto registerVM);
        Task<SelectUserDto?> GetUserAsync(int Id);
        Task<SelectUserDto?> GetUserAsync(string usernameOrEmail);
        Task<List<SelectUserDto>?> GetUsersAsync(string? Roles);
        Task UpdateAsync(int Id, UpdateUserDto updateVM);
        Task DeleteAsync(int Id);

    }
}
