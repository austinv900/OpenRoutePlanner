
using OpenRoutePlanner.ModelManagers;
using OpenRoutePlanner.Models;
using System.Collections.Immutable;
using System.Security.Claims;

namespace OpenRoutePlanner.Authentication;

public class AuthService : IAuthService
{
    protected DriverModelManager DriverManager { get; }

    public AuthService(DriverModelManager dmm)
    {
        DriverManager = dmm;
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> LoginAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return new(false, "Missing email address");
        }

        email = email.ToLower();

        ImmutableArray<DriverProfile> locatedDrivers = await DriverManager.List(new()
        {
            Email = email
        });

        if (locatedDrivers.IsDefaultOrEmpty)
        {
            return new(false, "No profiles found with provided email");
        }

        DriverProfile? selectedDriver = locatedDrivers.FirstOrDefault(x => x.Email != null && x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (selectedDriver == null)
        {
            return new(false, "No profiles found with provided email");
        }

        ImmutableArray<Claim> claims = [
            new Claim(ClaimTypes.PrimarySid, selectedDriver.Id.ToString()),
            new Claim(ClaimTypes.Email, selectedDriver.Email!),
            new Claim(ClaimTypes.GivenName, selectedDriver.FirstName),
            new Claim(ClaimTypes.Surname, selectedDriver.LastName),
            new Claim(ClaimTypes.Name, selectedDriver.FullName)
            ];

        if (selectedDriver.PhoneNumber != null)
        {
            claims = claims.Add(new Claim(ClaimTypes.MobilePhone, selectedDriver.PhoneNumber));
        }

        // TODO: Validate Password
        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);

        return new(true, null);
    }

    public Task LogoutAsync()
    {
        throw new NotImplementedException();
    }
}
