namespace HelloCMS.Identity.Data.ViewModels
{
    public record TokenResultVM(string? Token, string? RefreshToken, DateTime ExpiresAt);

}