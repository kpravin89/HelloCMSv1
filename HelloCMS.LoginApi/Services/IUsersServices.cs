using HelloCMS.Identity.Data.ViewModels.Users;

namespace HelloCMS.Identity.Services
{
    public interface IUsersServices
    {
        Task<SelectUserVM> RegisterAsync(RegisterUserVM registerVM);
        Task<SelectUserVM?> GetUser(int Id);
        Task<SelectUserVM?> GetUser(string usernameOrEmail);
        Task<List<SelectUserVM>?> GetUsers(string? Roles);
        Task Update(int Id, UpdateUserVM updateVM);
        Task Delete(int Id);

    }
}
