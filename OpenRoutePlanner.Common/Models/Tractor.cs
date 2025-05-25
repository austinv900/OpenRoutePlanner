using System.ComponentModel.DataAnnotations.Schema;

namespace OpenRoutePlanner.Models;

public class Tractor : IComparable, IComparable<Tractor>, IEquatable<Tractor>
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? VinNumber { get; set; }

    public string Name { get; set; }

    public bool IsSleeper { get; set; }

    public Guid? DriverId { get; set; }

    [ForeignKey(nameof(DriverId))]
    public DriverProfile? Driver { get; set; }

    public double Odometer { get; set; }

    public string? LicensePlate { get; set; }

    public string? State { get; set; }

    public string? GeotabId { get; set; }

    #region Interface Methods

    public int CompareTo(Tractor? other)
    {
        if (other == null)
        {
            return 1;
        }

        return Odometer.CompareTo(other.Odometer);
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Tractor other)
        {
            return 1;
        }

        return CompareTo(other);
    }

    public bool Equals(Tractor? other)
    {
        return other != null && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return obj is Tractor other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"{(IsSleeper ? "Sleeper Cab" : "Day Cab")} {Name} - {Odometer} mi";
    }

    #endregion
}
