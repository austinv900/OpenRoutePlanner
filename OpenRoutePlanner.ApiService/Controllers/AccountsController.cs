using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenRoutePlanner.DtoQuery;
using OpenRoutePlanner.Models;

namespace OpenRoutePlanner.Controllers;

public class AccountsController : BaseModelController<BusinessAccount, AccountQuery>
{
    protected override Func<BusinessAccount, Guid> IdCallback => (x) => x.Id;

    protected override DbSet<BusinessAccount> Table => Db.Accounts;

    public AccountsController(IServiceProvider services) : base(services)
    {
    }

    protected override Task<IQueryable<BusinessAccount>> DefaultSort(IQueryable<BusinessAccount> query)
    {
        query = query.OrderBy(x => x.CostCenter);
        return Task.FromResult(query);
    }

    protected override Task<IQueryable<BusinessAccount>> ApplyFilter(IQueryable<BusinessAccount> query, AccountQuery? filter)
    {
        if (filter != null)
        {
            if (filter.AccountId.HasValue)
            {
                query = query.Where(x => x.Id == filter.AccountId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(x => x.ShortName.ToLower().Contains(filter.Name.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.Address))
            {
                query = query.Where(x => x.Address != null && x.Address.ToLower().Contains(filter.Address.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                query = query.Where(x => x.Email != null && x.Email.ToLower().Contains(filter.Email.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
            {
                query = query.Where(x => x.PhoneNumber != null && x.PhoneNumber.ToLower().Contains(filter.PhoneNumber.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.CostCenter))
            {
                query = query.Where(x => x.CostCenter != null && x.CostCenter.ToLower().Contains(filter.CostCenter.ToLower()));
            }

            if (filter.Endorsement.HasValue)
            {
                query = query.Where(x => (x.RequiredEndorsements & filter.Endorsement.Value) == filter.Endorsement.Value);
            }
        }

        return base.ApplyFilter(query, filter);
    }
}
