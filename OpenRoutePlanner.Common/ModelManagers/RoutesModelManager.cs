using OpenRoutePlanner.DtoQuery;
using OpenRoutePlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OpenRoutePlanner.ModelManagers;
public class RoutesModelManager(HttpClient client) : IModelManager<RoutePlan, RoutePlanQuery>
{
    public async Task<HttpStatusCode> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await client.DeleteAsync($"/routes/{id}", cancellationToken);
        return response.StatusCode;
    }

    public async Task<RoutePlan?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await client.GetFromJsonAsync<RoutePlan>($"/routes/{id}", cancellationToken);
    }

    public async Task<ImmutableArray<RoutePlan>> List(RoutePlanQuery? query, CancellationToken cancellationToken = default)
    {
        ImmutableArray<RoutePlan> items = ImmutableArray<RoutePlan>.Empty;
        IAsyncEnumerable<RoutePlan?> enumerable;

        if (query == null)
        {
            enumerable = client.GetFromJsonAsAsyncEnumerable<RoutePlan>("/routes", cancellationToken);
        }
        else
        {
            enumerable = client.GetFromJsonAsAsyncEnumerable<RoutePlan>("/routes".ToQueryString(query), cancellationToken);
        }

        await foreach (RoutePlan? x in enumerable)
        {
            if (x == null)
            {
                continue;
            }

            if (!items.Contains(x))
            {
                items = items.Add(x);
            }
        }

        return items;
    }

    public async Task<HttpStatusCode> Post(RoutePlan model, CancellationToken cancellationToken = default)
    {
        await NormalizeModel(model, cancellationToken);
        HttpResponseMessage response = await client.PostAsJsonAsync($"/routes", model, cancellationToken);
        return response.StatusCode;
    }

    public async Task<HttpStatusCode> Put(Guid id, RoutePlan model, CancellationToken cancellationToken = default)
    {
        await NormalizeModel(model, cancellationToken);
        HttpResponseMessage response = await client.PutAsJsonAsync($"/routes/{id}", model, cancellationToken);
        return response.StatusCode;
    }

    public Task NormalizeModel(RoutePlan model, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
