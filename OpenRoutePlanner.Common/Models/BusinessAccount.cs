using System.ComponentModel.DataAnnotations;

namespace OpenRoutePlanner.Models;

public class BusinessAccount : IComparable, IComparable<BusinessAccount>, IEquatable<BusinessAccount>
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FullName { get; set; }

    public string ShortName { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? CostCenter { get; set; }

    public LicenseEndorsement RequiredEndorsements { get; set; } = LicenseEndorsement.None;

    #region Interface Methods

    public int CompareTo(BusinessAccount? other)
    {
        if (other == null)
        {
            return 1;
        }

        return FullName.CompareTo(other.FullName);
    }

    public int CompareTo(object? obj)
    {
        if (obj is not BusinessAccount other)
        {
            return 1;
        }

        return CompareTo(other);
    }

    public bool Equals(BusinessAccount? other)
    {
        return other != null && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return obj is BusinessAccount other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"({CostCenter}) {ShortName} - {Address}";
    }

    #endregion
}
