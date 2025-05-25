namespace OpenRoutePlanner.DtoQuery;

public class TractorQuery : IDtoQuery
{
    public Guid? Id { get; set; }

    public Guid? DriverId { get; set; }

    public bool? IsSleeper { get; set; }

    public string? Name { get; set; }

    public int? Limit { get; set; }

    public int? Page { get; set; }

    public bool? IncludeDriver { get; set; }

    public string? GeotabId { get; set; }
}
