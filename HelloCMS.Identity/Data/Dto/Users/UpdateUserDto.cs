namespace HelloCMS.Identity.Data.Dto.Users
{
    public record UpdateUserDto(string Salutation,
                string FirstName,
                string LastName,
                string SecondaryEmail,
                string YearOfBirth,
                string Website);
}
