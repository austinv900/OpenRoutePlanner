using Geotab.Checkmate;
using Geotab.Checkmate.ObjectModel;
using Geotab.Checkmate.ObjectModel.Engine;
using OpenRoutePlanner.Models;

namespace OpenRoutePlanner.Modules;

public class GeotabModule
{
    private API? Geotab { get; }

    public GeotabModule(IConfiguration config)
    {
        string? username = config["Geotab:Username"];
        string? password = config["Geotab:Password"];
        string? server = config["Geotab:Server"];
        string? database = config["Geotab:Database"];

        if (username != null && password != null && database != null)
        {
            Geotab = new API(username, password, null, database, server: server);
            Task.Run(() => Initialize());
        }
    }

    private async Task Initialize(CancellationToken cancellationToken = default)
    {
        await Geotab!.AuthenticateAsync(cancellationToken);
    }

    public async Task<double?> UpdateTractor(Tractor tractor, CancellationToken cancellationToken = default)
    {
        if (Geotab == null)
        {
            return null;
        }

        if (tractor == null)
        {
            return null;
        }

        Id? vehicleId;
        if (string.IsNullOrWhiteSpace(tractor.GeotabId))
        {
            if (string.IsNullOrWhiteSpace(tractor.VinNumber))
            {
                return null;
            }

            vehicleId = await GetTractorId(tractor, cancellationToken);
            if (vehicleId == null)
            {
                return null;
            }

            tractor.GeotabId = vehicleId.ToString();
        }
        else
        {
            vehicleId = Id.Create(tractor.GeotabId);
        }

        IList<StatusData>? statusData = await Geotab.CallAsync<IList<StatusData>>("Get", typeof(StatusData), new
        {
            search = new StatusDataSearch
            {
                DeviceSearch = new DeviceSearch(vehicleId),
                DiagnosticSearch = new DiagnosticSearch(KnownId.DiagnosticOdometerAdjustmentId),
                FromDate = DateTime.MaxValue
            }
        }, cancellationToken);

        if (statusData != null)
        {
            var odo = statusData[0].Data ?? 0;
            tractor.Odometer = Math.Round(Distance.ToImperial(odo / 1000), 0);
        }

        return null;
    }

    public async Task<Id?> GetTractorId(Tractor tractor, CancellationToken cancellationToken = default)
    {
        if (Geotab == null)
        {
            return null;
        }

        if (tractor == null || string.IsNullOrWhiteSpace(tractor.VinNumber))
        {
            return null;
        }

        Device? device = (await Geotab.CallAsync<IList<Device>>("Get", typeof(Device), new
        {
            search = new DeviceSearch { VehicleIdentificationNumber = tractor.VinNumber },
            propertySelector = new PropertySelector
            {
                Fields = new[] { nameof(Device.Id), nameof(Device.Name) }
            }
        }, cancellationToken))?.FirstOrDefault();

        if (device != null)
        {
            return device.Id;
        }

        return null;
    }


}
