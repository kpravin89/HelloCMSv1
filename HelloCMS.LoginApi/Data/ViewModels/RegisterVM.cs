using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace HelloCMS.LoginApi.Data.ViewModels
{
    public record RegisterVM([Required] string Salutation,
                [Required] string FirstName,
                [Required] string LastName,
                [Required] string UserName,
                [Required] string Password,
                [Required] string ConfirmPassword,
                [Required] string Email,
                string SecondaryEmail,
                [Required] string YearOfBirth,
                string Website,
                [Required] string Role
        );
}
