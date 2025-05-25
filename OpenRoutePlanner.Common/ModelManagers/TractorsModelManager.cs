using OpenRoutePlanner.DtoQuery;
using OpenRoutePlanner.Models;
using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Json;

namespace OpenRoutePlanner.ModelManagers;
public class TractorsModelManager(HttpClient client) : IModelManager<Tractor, TractorQuery>
{
    public async Task<HttpStatusCode> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await client.DeleteAsync($"/tractors/{id}", cancellationToken);
        return response.StatusCode;
    }

    public async Task<Tractor?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await client.GetFromJsonAsync<Tractor>($"/tractors/{id}", cancellationToken);
    }

    public async Task<ImmutableArray<Tractor>> List(TractorQuery? query, CancellationToken cancellationToken = default)
    {
        ImmutableArray<Tractor> items = ImmutableArray<Tractor>.Empty;
        IAsyncEnumerable<Tractor?> enumerable;

        if (query == null)
        {
            enumerable = client.GetFromJsonAsAsyncEnumerable<Tractor>("/tractors", cancellationToken);
        }
        else
        {
            enumerable = client.GetFromJsonAsAsyncEnumerable<Tractor>("/tractors".ToQueryString(query), cancellationToken);
        }

        await foreach (Tractor? x in enumerable)
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

    

    public async Task<HttpStatusCode> Post(Tractor model, CancellationToken cancellationToken = default)
    {
        await NormalizeModel(model, cancellationToken);
        HttpResponseMessage response = await client.PostAsJsonAsync($"/tractors", model, cancellationToken);
        return response.StatusCode;
    }

    public async Task<HttpStatusCode> Put(Guid id, Tractor model, CancellationToken cancellationToken = default)
    {
        await NormalizeModel(model, cancellationToken);
        HttpResponseMessage response = await client.PutAsJsonAsync($"/tractors/{id}", model, cancellationToken);
        return response.StatusCode;
    }

    public Task NormalizeModel(Tractor model, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
