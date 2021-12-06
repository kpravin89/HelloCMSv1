namespace HelloCMS.Identity.Data.Dto.Login
{
    public record TokenResultDto(string? Token, string? RefreshToken, DateTime ExpiresAt);

}