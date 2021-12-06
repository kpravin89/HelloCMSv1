using HelloCMS.Identity.Data.Dto.Login;
using HelloCMS.Identity.Data.Models;

namespace HelloCMS.Identity.Services
{
    public interface ILoginServices
    {
        Task<TokenResultDto?> LoginAsync(LoginDto loginVM);
        Task<TokenResultDto> VerifyAndGenerateTokenAsync(TokenRequestDto tokenRequestVM);
    }
}
