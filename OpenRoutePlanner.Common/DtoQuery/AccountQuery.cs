using OpenRoutePlanner.Models;

namespace OpenRoutePlanner.DtoQuery;
public class AccountQuery : IDtoQuery
{
    public int? Limit { get; set; }
    public int? Page { get; set; }
    public Guid? AccountId { get; set; }
    public string? Name { get; set; }
    public string? CostCenter { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public LicenseEndorsement? Endorsement { get; set; }
}
