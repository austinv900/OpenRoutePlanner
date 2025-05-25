using OpenRoutePlanner.Models;
using System.Collections.Immutable;

namespace OpenRoutePlanner.DtoQuery;
public class RoutePlanQuery : IDtoQuery
{
    public int? Limit { get; set; }
    public int? Page { get; set; }
    public Guid? RouteId { get; set; }
    public string? RouteName { get; set; }
    public int? MinimumStops { get; set; }
    public int? MaximumStops { get; set; }
    public double? MinimumMiles { get; set; }
    public double? MaximumMiles { get; set; }
    public Guid? TractorId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid? AccountId { get; set; }
    public DateTimeOffset? BeginStartTime { get; set; }
    public DateTimeOffset? EndStartTime { get; set; }
    public DateTimeOffset? BeginEndTime { get; set; }
    public DateTimeOffset? EndEndTime { get; set; }
    public DateTimeOffset? BeginCreateTime { get; set; }
    public DateTimeOffset? EndCreateTime { get; set; }
    public LicenseEndorsement? Endorsements { get; set; }
    public ImmutableArray<string>? Trailers { get; set; }
    public bool? IsCompleted { get; set; }
    public bool? IncludeDriver { get; set; }
    public bool? IncludeAccount { get; set; }
    public bool? IncludeTractor { get; set; }
}
