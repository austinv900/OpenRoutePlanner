using System.ComponentModel.DataAnnotations;

namespace OpenRoutePlanner.Models;

[Flags]
public enum LicenseEndorsement
{
    None = 0,

    /// <summary>
    /// H - Hazmat
    /// </summary>
    [Display(Name = "Hazmat", ShortName = "H")]
    Hazmat = 1 << 0,

    /// <summary>
    /// N - Tank Vehicles
    /// </summary>
    [Display(Name = "Tanker", ShortName = "N")]
    Tanker = 1 << 1,

    /// <summary>
    /// T - Double/Triple Trailers
    /// </summary>
    [Display(Name = "Doubles/Triples", ShortName = "T")]
    DoubleTripleTrailers = 1 << 2,

    /// <summary>
    /// P - Passenger Vehicles
    /// </summary>
    [Display(Name = "Passenger", ShortName = "P")]
    Passenger = 1 << 3,

    /// <summary>
    /// X - Hazmat and tanker combo
    /// </summary>
    [Display(Name = "Hazmat/Tanker", ShortName = "X")]
    HazmatTanker = Hazmat | Tanker,

    /// <summary>
    /// S - School Bus
    /// </summary>
    /// <remarks>
    /// Requires Passenger
    /// </remarks>
    [Display(Name = "School Bus", ShortName = "S")]
    SchoolBus = 1 << 4
}
