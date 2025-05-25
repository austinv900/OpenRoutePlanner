using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenRoutePlanner.Models;

public class DriverProfile : IComparable, IComparable<DriverProfile>, IEquatable<DriverProfile>
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string FullName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? AlphaCode { get; set; }

    public string? EmployeeId { get; set; }

    public string? PasswordHash { get; set; }

    public Guid? AccountId { get; set; }

    [ForeignKey(nameof(AccountId))]
    public BusinessAccount? Account { get; set; }

    public LicenseEndorsement Endorsements { get; set; } = LicenseEndorsement.None;

    #region Interface Methods
    
    public int CompareTo(DriverProfile? other)
    {
        if (other == null)
        {
            return 1;
        }

        return FullName.CompareTo(other.FullName);
    }

    public int CompareTo(object? obj)
    {
        if (obj is not DriverProfile other)
        {
            return 1;
        }

        return CompareTo(other);
    }

    public bool Equals(DriverProfile? other)
    {
        return other != null && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return obj is DriverProfile other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"({AlphaCode}) {FullName}";
    }

    #endregion
}
