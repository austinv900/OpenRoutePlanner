using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenRoutePlanner.DtoQuery;
using OpenRoutePlanner.Models;

namespace OpenRoutePlanner.Controllers;

public class DriversController : BaseModelController<DriverProfile, DriverQuery>
{
    protected override Func<DriverProfile, Guid> IdCallback => (x) => x.Id;

    protected override DbSet<DriverProfile> Table => Db.Drivers;

    public DriversController(IServiceProvider services) : base(services)
    {
    }

    public override Task<ActionResult<DriverProfile?>> Post([FromBody] DriverProfile model)
    {
        model.FullName = $"{model.FirstName} {model.LastName}";
        return base.Post(model);
    }

    public override Task<ActionResult<DriverProfile?>> Put(Guid id, [FromBody] DriverProfile model)
    {
        model.FullName = $"{model.FirstName} {model.LastName}";
        return base.Put(id, model);
    }

    protected override Task<IQueryable<DriverProfile>> DefaultSort(IQueryable<DriverProfile> query)
    {
        query = query.OrderBy(x => x.FullName);
        return Task.FromResult(query);
    }

    protected override Task<IQueryable<DriverProfile>> ApplyFilter(IQueryable<DriverProfile> query, DriverQuery? filter)
    {
        if (filter != null)
        {
            if (filter.DriverId.HasValue)
            {
                query = query.Where(x => x.Id == filter.DriverId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(x => x.FullName.ToLower().Contains(filter.Name.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.AlphaCode))
            {
                query = query.Where(x => x.AlphaCode != null && x.AlphaCode.ToLower().Contains(filter.AlphaCode.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.EmployeeId))
            {
                query = query.Where(x => x.EmployeeId != null && x.EmployeeId.ToLower().Contains(filter.EmployeeId.ToLower()));
            }

            if (filter.AccountId.HasValue)
            {
                query = query.Where(x => x.AccountId == filter.AccountId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                query = query.Where(x => x.Email != null && x.Email.ToLower().Contains(filter.Email.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
            {
                query = query.Where(x => x.PhoneNumber != null && x.PhoneNumber.ToLower().Contains(filter.PhoneNumber.ToLower()));
            }

            if (filter.Endorsements.HasValue)
            {
                query = query.Where(x => (x.Endorsements & filter.Endorsements.Value) == filter.Endorsements.Value);
            }

            if (filter.IncludeAccount.HasValue && filter.IncludeAccount.Value)
            {
                query = query.Include(x => x.Account);
            }
        }

        return base.ApplyFilter(query, filter);
    }
}
