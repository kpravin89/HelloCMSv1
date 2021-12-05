using HelloCMS.Identity.Data.Models;
using HelloCMS.Identity.Data.ViewModels;

namespace HelloCMS.Identity.Services
{
    public interface ILoginServices
    {
        Task<TokenResultVM?> LoginAsync(LoginVM loginVM);
        Task<TokenResultVM> VerifyAndGenerateTokenAsync(TokenRequestVM tokenRequestVM);
    }
}
