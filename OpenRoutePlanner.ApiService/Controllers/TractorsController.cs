using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenRoutePlanner.DtoQuery;
using OpenRoutePlanner.Models;
using OpenRoutePlanner.Modules;

namespace OpenRoutePlanner.Controllers;

public class TractorsController : BaseModelController<Tractor, TractorQuery>
{
    protected override Func<Tractor, Guid> IdCallback => (t) => t.Id;
    protected override DbSet<Tractor> Table => Db.Tractors;

    protected GeotabModule? Geotab { get; }

    public TractorsController(IServiceProvider services) : base(services)
    {
        Geotab = services.GetService<GeotabModule>();
    }

    public override async Task<ActionResult<Tractor?>> Put(Guid id, [FromBody] Tractor model)
    {
        if (model.DriverId.HasValue)
        {
            foreach (var tractor in Table.Where(t => t.Id != id && t.DriverId == model.DriverId.Value))
            {
                tractor.DriverId = null;
                tractor.Driver = null;
                Db.Entry(tractor).State = EntityState.Modified;
            }
        }

        if (Geotab != null)
        {
            await Geotab.UpdateTractor(model, HttpContext.RequestAborted);
        }

        return await base.Put(id, model);
    }

    public override async Task<ActionResult<Tractor?>> Post([FromBody] Tractor model)
    {
        if (model.DriverId.HasValue)
        {
            foreach (var tractor in Table.Where(t => t.DriverId == model.DriverId.Value))
            {
                tractor.DriverId = null;
                tractor.Driver = null;
                Db.Entry(tractor).State = EntityState.Modified;
            }
        }

        if (Geotab != null)
        {
            await Geotab.UpdateTractor(model, HttpContext.RequestAborted);
        }

        return await base.Post(model);
    }

    protected override Task<IQueryable<Tractor>> DefaultSort(IQueryable<Tractor> query)
    {
        query = query.OrderBy(t => t.Name);
        return Task.FromResult(query);
    }

    protected override Task<IQueryable<Tractor>> ApplyFilter(IQueryable<Tractor> query, TractorQuery? filter)
    {
        if (filter != null)
        {
            if (filter.Id.HasValue)
            {
                query = query.Where(t => t.Id == filter.Id);
            }

            if (filter.IsSleeper.HasValue)
            {
                query = query.Where(t => t.IsSleeper == filter.IsSleeper);
            }

            if (filter.DriverId.HasValue)
            {
                query = query.Where(t => t.DriverId == filter.DriverId);
            }

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(t => t.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.GeotabId))
            {
                query = query.Where(x => x.GeotabId != null && x.GeotabId.ToLower().Contains(filter.GeotabId.ToLower()));
            }

            if (filter.IncludeDriver.HasValue && filter.IncludeDriver.Value)
            {
                query = query.Include(x => x.Driver);
            }
        }

        return base.ApplyFilter(query, filter);
    }
}
