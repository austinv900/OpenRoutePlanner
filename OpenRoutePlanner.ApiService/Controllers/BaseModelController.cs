using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenRoutePlanner.Database;
using OpenRoutePlanner.DtoQuery;

namespace OpenRoutePlanner.Controllers;

[Route("[controller]")]
[ApiController]
public abstract class BaseModelController<TModel, TDtoModel> : ControllerBase where TModel : class where TDtoModel : IDtoQuery
{
    protected DatabaseContext Db { get; }
    protected IMapper Mapper { get; }
    protected ILogger Logger { get; }
    protected abstract Func<TModel, Guid> IdCallback { get; }
    protected abstract DbSet<TModel> Table { get; }


    protected BaseModelController(IServiceProvider services)
    {
        Db = services.GetRequiredService<DatabaseContext>();
        Mapper = services.GetRequiredService<IMapper>();

        Type type = typeof(ILogger<>)
            .MakeGenericType(GetType());
        Logger = (ILogger)services.GetRequiredService(type);
    }

    [HttpGet]
    public virtual async IAsyncEnumerable<TModel> Get([FromQuery] TDtoModel? filter)
    {
        IQueryable<TModel> query = Table;
        query = await DefaultSort(query);
        query = await ApplyFilter(query, filter);
        query = await ApplyLimits(query, filter);

        await foreach (TModel item in query.AsAsyncEnumerable())
        {
            if (HttpContext.RequestAborted.IsCancellationRequested)
            {
                yield break;
            }

            yield return item;
        }
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TModel?>> Get(Guid id)
    {
        if (id == Guid.Empty)
        {
            Logger.LogDebug("Tried looking up model with Empty Guid");
            return BadRequest();
        }

        TModel? item = await Table.FindAsync([id], HttpContext.RequestAborted);

        if (item == null)
        {
            Logger.LogDebug("Unable to find row with ID {ID}", id);
            return NotFound();
        }

        Logger.LogInformation("Returning row with ID {ID}", id);
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        TModel? x = await Table.FindAsync([id], HttpContext.RequestAborted);

        if (x == null)
        {
            Logger.LogDebug("Unable to find row with ID {ID}", id);
            return NotFound();
        }

        Table.Remove(x);
        int i = await Db.SaveChangesAsync(HttpContext.RequestAborted);

        if (i > 0)
        {
            Logger.LogInformation("Removed {I} rows from the table", i);
            return Ok();
        }

        Logger.LogError("Tried to remove {ID} from Table but execution returned {COUNT}", id, i);
        return Problem();
    }

    [HttpPost]
    public virtual async Task<ActionResult<TModel?>> Post([FromBody] TModel model)
    {
        Guid id = IdCallback(model);
        await Table.AddAsync(model, HttpContext.RequestAborted);
        await Db.SaveChangesAsync(HttpContext.RequestAborted);
        return CreatedAtAction(nameof(Get), id);
    }

    [HttpPut("{id}")]
    public virtual async Task<ActionResult<TModel?>> Put(Guid id, [FromBody] TModel model)
    {
        TModel? lookup = await Table.FindAsync([id], HttpContext.RequestAborted);

        if (lookup == null)
        {
            return NotFound();
        }
        Mapper.Map(model, lookup);
        Db.Entry(lookup).State = EntityState.Modified;
        await Db.SaveChangesAsync(HttpContext.RequestAborted);
        return AcceptedAtAction(nameof(Get), id);
    }


    protected virtual Task<IQueryable<TModel>> ApplyFilter(IQueryable<TModel> query, TDtoModel? filter)
    {
        return Task.FromResult(query);
    }

    protected virtual async Task<IQueryable<TModel>> ApplyLimits(IQueryable<TModel> query, TDtoModel? filter)
    {
        int total = await query.CountAsync(HttpContext.RequestAborted);
        int count = 20;
        int page = 1;

        if (filter != null)
        {
            if (filter.Limit.HasValue)
            {
                count = filter.Limit.Value > 50 ? 50 : filter.Limit.Value < 1 ? 1 : filter.Limit.Value;
            }

            if (filter.Page.HasValue)
            {
                page = filter.Page.Value < 1 ? 1 : filter.Page.Value;
            }
        }

        Response.Headers["X-Total-Count"] = total.ToString();
        Response.Headers["X-Page"] = page.ToString();
        Response.Headers["X-Page-Size"] = count.ToString();
        query = query.Skip((page - 1) * count)
            .Take(count);

        return query;
    }

    protected abstract Task<IQueryable<TModel>> DefaultSort(IQueryable<TModel> query);
}
