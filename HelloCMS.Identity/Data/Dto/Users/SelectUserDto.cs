namespace HelloCMS.Identity.Data.Dto.Users
{
    public record SelectUserDto(int Id,
                string Salutation,
                string FirstName,
                string LastName,
                string UserName,
                string Email,
                string SecondaryEmail,
                string YearOfBirth,
                string Website);
}
