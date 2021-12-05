namespace HelloCMS.Identity.Data.ViewModels.Users
{
    public record SelectUserVM(int Id,
                string Salutation,
                string FirstName,
                string LastName,
                string UserName,
                string Email,
                string SecondaryEmail,
                string YearOfBirth,
                string Website);
}
