using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenRoutePlanner.Models;

public class RoutePlan : IComparable, IComparable<RoutePlan>, IEquatable<RoutePlan>
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; }

    public int Stops { get; set; }

    public double Miles { get; set; }

    public Guid? TractorId { get; set; }

    [ForeignKey(nameof(TractorId))]
    public Tractor? Tractor { get; set; }

    public Guid? AccountId { get; set; }

    [ForeignKey(nameof(AccountId))]
    public BusinessAccount? Account { get; set; }

    public Guid? DriverId { get; set; }

    [ForeignKey(nameof(DriverId))]
    public DriverProfile? Driver { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public DateTimeOffset CreatedTime { get; set; }

    public List<string> Trailers { get; set; } = [];

    public bool IsCompleted { get; set; } = false;

    public LicenseEndorsement RequiredEndorsements { get; set; } = LicenseEndorsement.None;

    #region Interface Methods

    public int CompareTo(RoutePlan? other)
    {
        if (other == null)
        {
            return 1;
        }

        return CreatedTime.CompareTo(other.CreatedTime);
    }

    public int CompareTo(object? obj)
    {
        if (obj is not RoutePlan other)
        {
            return 1;
        }

        return CompareTo(other);
    }

    public bool Equals(RoutePlan? other)
    {
        return other != null && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return obj is RoutePlan other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"{Name} | Stops: {Stops}, Miles: {Miles}";
    }

    #endregion
}
