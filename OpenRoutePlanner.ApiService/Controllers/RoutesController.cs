using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenRoutePlanner.DtoQuery;
using OpenRoutePlanner.Models;
using OpenRoutePlanner.Modules;

namespace OpenRoutePlanner.Controllers;

public class RoutesController : BaseModelController<RoutePlan, RoutePlanQuery>
{
    protected override Func<RoutePlan, Guid> IdCallback => (r) => r.Id;

    protected override DbSet<RoutePlan> Table => Db.Routes;

    protected GeotabModule? Geotab { get; }

    public RoutesController(IServiceProvider services) : base(services)
    {
        Geotab = services.GetService<GeotabModule>();
    }

    protected override Task<IQueryable<RoutePlan>> DefaultSort(IQueryable<RoutePlan> query)
    {
        query = query.OrderBy(x => x.CreatedTime);
        return Task.FromResult(query);
    }

    public override Task<ActionResult<RoutePlan?>> Post([FromBody] RoutePlan model)
    {
        model.CreatedTime = DateTimeOffset.Now;
        return base.Post(model);
    }

    public override async Task<ActionResult<RoutePlan?>> Put(Guid id, [FromBody] RoutePlan model)
    {
        if (model.TractorId.HasValue && Geotab != null)
        {
            Tractor? tractor = await Db.Tractors.FindAsync([model.TractorId.Value], HttpContext.RequestAborted);

            if (tractor != null)
            {
                await Geotab.UpdateTractor(tractor, HttpContext.RequestAborted);
            }
        }

        return await base.Put(id, model);
    }

    protected override Task<IQueryable<RoutePlan>> ApplyFilter(IQueryable<RoutePlan> query, RoutePlanQuery? filter)
    {
        if (filter != null)
        {
            if (filter.RouteId.HasValue)
            {
                query = query.Where(x => x.Id == filter.RouteId.Value);
            }

            if (filter.AccountId.HasValue)
            {
                query = query.Where(x => x.AccountId == filter.AccountId.Value);
            }

            if (filter.DriverId.HasValue)
            {
                query = query.Where(x => x.DriverId == filter.DriverId.Value);
            }

            if (filter.TractorId.HasValue)
            {
                query = query.Where(x => x.TractorId == filter.TractorId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.RouteName))
            {
                query = query.Where(x => x.Name.ToLower().Contains(filter.RouteName.ToLower()));
            }

            if (filter.MinimumStops.HasValue)
            {
                query = query.Where(x => x.Stops >= filter.MinimumStops.Value);
            }

            if (filter.MaximumStops.HasValue)
            {
                query = query.Where(x => x.Stops <= filter.MaximumStops.Value);
            }

            if (filter.MinimumMiles.HasValue)
            {
                query = query.Where(x => x.Miles >= filter.MinimumMiles.Value);
            }

            if (filter.MaximumMiles.HasValue)
            {
                query = query.Where(x => x.Miles <= filter.MaximumMiles.Value);
            }

            if (filter.BeginCreateTime.HasValue)
            {
                query = query.Where(x => x.CreatedTime >= filter.BeginCreateTime.Value);
            }

            if (filter.EndCreateTime.HasValue)
            {
                query = query.Where(x => x.CreatedTime <= filter.EndCreateTime.Value);
            }

            if (filter.BeginStartTime.HasValue)
            {
                query = query.Where(x => x.StartTime >= filter.BeginStartTime.Value);
            }

            if (filter.EndStartTime.HasValue)
            {
                query = query.Where(x => x.StartTime <= filter.EndStartTime.Value);
            }

            if (filter.BeginEndTime.HasValue)
            {
                query = query.Where(x => x.EndTime >= filter.BeginEndTime.Value);
            }

            if (filter.EndEndTime.HasValue)
            {
                query = query.Where(x => x.EndTime <= filter.EndEndTime.Value);
            }

            if (filter.Endorsements.HasValue)
            {
                query = query.Where(x => (x.RequiredEndorsements & filter.Endorsements.Value) == filter.Endorsements.Value);
            }

            if (filter.IsCompleted.HasValue)
            {
                query = query.Where(x => x.IsCompleted == filter.IsCompleted.Value);
            }

            if (filter.IncludeAccount.HasValue && filter.IncludeAccount.Value)
            {
                query = query.Include(x => x.Account);
            }

            if (filter.IncludeDriver.HasValue && filter.IncludeDriver.Value)
            {
                query = query.Include(x => x.Driver);
            }

            if (filter.IncludeTractor.HasValue && filter.IncludeTractor.Value)
            {
                query = query.Include(x => x.Tractor);
            }

            if (filter.Trailers.HasValue && filter.Trailers.Value.Length > 0)
            {
                var requestedTrailers = filter.Trailers.Value.ToHashSet(StringComparer.OrdinalIgnoreCase);
                query = query.AsEnumerable()
                    .Where(x => requestedTrailers.IsSubsetOf(x.Trailers))
                    .AsQueryable();
            }
        }

        return base.ApplyFilter(query, filter);
    }
}
