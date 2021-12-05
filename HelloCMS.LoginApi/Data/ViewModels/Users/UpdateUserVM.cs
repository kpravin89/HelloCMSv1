namespace HelloCMS.Identity.Data.ViewModels.Users
{
    public record UpdateUserVM(string Salutation,
                string FirstName,
                string LastName,
                string SecondaryEmail,
                string YearOfBirth,
                string Website);
}
