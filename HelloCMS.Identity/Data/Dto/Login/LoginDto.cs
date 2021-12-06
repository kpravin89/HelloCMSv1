using System.ComponentModel.DataAnnotations;

namespace HelloCMS.Identity.Data.Dto.Login
{
    /// <summary>
    /// Login View Model
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="Password"></param>
    public record LoginDto(
                [Required] string UserName,
                [Required] string Password
        );
}
