using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenRoutePlanner.Database;
using OpenRoutePlanner.DtoQuery;
using OpenRoutePlanner.Models;

namespace OpenRoutePlanner.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private DatabaseContext Db { get; }

    private IPasswordHasher<DriverProfile> Hasher { get; }

    public AuthController(DatabaseContext db, IPasswordHasher<DriverProfile> hasher)
    {
        Db = db;
        Hasher = hasher;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterAccountModel register)
    {
        if (await Db.Drivers.AnyAsync(x => x.Email != null && x.Email.ToLower() == register.Email.ToLower(), HttpContext.RequestAborted))
        {
            return Conflict("Email already exists");
        }

        var driver = new DriverProfile
        {
            FirstName = register.FirstName,
            LastName = register.LastName,
            Email = register.Email,
            FullName = $"{register.FirstName} {register.LastName}"
        };

        driver.PasswordHash = Hasher.HashPassword(driver, register.Password);
        await Db.Drivers.AddAsync(driver, HttpContext.RequestAborted);
        await Db.SaveChangesAsync(HttpContext.RequestAborted);
        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var driver = await Db.Drivers.FirstOrDefaultAsync(x => x.Email != null && x.PasswordHash != null && x.Email.ToLower() == request.Email.ToLower());

        if (driver == null)
        {
            return Unauthorized("Invalid credentials");
        }

        var result = Hasher.VerifyHashedPassword(driver, driver.PasswordHash!, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(new { Token = string.Empty });
    }
}
