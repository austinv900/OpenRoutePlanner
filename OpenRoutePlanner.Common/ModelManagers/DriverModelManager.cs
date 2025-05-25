using OpenRoutePlanner.DtoQuery;
using OpenRoutePlanner.Models;
using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Json;

namespace OpenRoutePlanner.ModelManagers;
public class DriverModelManager(HttpClient client) : IModelManager<DriverProfile, DriverQuery>
{
    public async Task<HttpStatusCode> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await client.DeleteAsync($"/drivers/{id}", cancellationToken);
        return response.StatusCode;
    }

    public async Task<DriverProfile?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await client.GetFromJsonAsync<DriverProfile>($"/drivers/{id}", cancellationToken);
    }

    public async Task<ImmutableArray<DriverProfile>> List(DriverQuery? query, CancellationToken cancellationToken = default)
    {
        ImmutableArray<DriverProfile> items = ImmutableArray<DriverProfile>.Empty;
        IAsyncEnumerable<DriverProfile?> enumerable;
        
        if (query == null)
        {
            enumerable = client.GetFromJsonAsAsyncEnumerable<DriverProfile>("/drivers", cancellationToken);
        }
        else
        {
            enumerable = client.GetFromJsonAsAsyncEnumerable<DriverProfile>("/drivers".ToQueryString(query), cancellationToken);
        }

        await foreach (DriverProfile? x in enumerable)
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

    public async Task<HttpStatusCode> Post(DriverProfile model, CancellationToken cancellationToken = default)
    {
        await NormalizeModel(model, cancellationToken);
        HttpResponseMessage response = await client.PostAsJsonAsync($"/drivers", model, cancellationToken);
        return response.StatusCode;
    }

    public async Task<HttpStatusCode> Put(Guid id, DriverProfile model, CancellationToken cancellationToken = default)
    {
        await NormalizeModel(model, cancellationToken);
        HttpResponseMessage response = await client.PutAsJsonAsync($"/drivers/{id}", model, cancellationToken);
        return response.StatusCode;
    }

    public Task NormalizeModel(DriverProfile model, CancellationToken cancellationToken = default)
    {
        model.FullName = $"{model.FirstName} {model.LastName}";
        model.AlphaCode = model.AlphaCode?.ToUpper();
        model.Email = model.Email?.ToLower();
        return Task.CompletedTask;
    }
}
