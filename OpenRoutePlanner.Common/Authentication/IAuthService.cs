namespace OpenRoutePlanner.Authentication;
public interface IAuthService
{
    Task<(bool Succeeded, string? ErrorMessage)> LoginAsync(string email, string password);

    Task LogoutAsync();
}
