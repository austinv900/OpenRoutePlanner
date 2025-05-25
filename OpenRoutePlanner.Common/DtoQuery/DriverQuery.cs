using OpenRoutePlanner.Models;

namespace OpenRoutePlanner.DtoQuery;
public class DriverQuery : IDtoQuery
{
    public int? Limit { get; set; }
    public int? Page { get; set; }
    public Guid? DriverId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AlphaCode { get; set; }
    public string? EmployeeId { get; set; }
    public Guid? AccountId { get; set; }
    public LicenseEndorsement? Endorsements { get; set; }

    public bool? IncludeAccount { get; set; }
}
