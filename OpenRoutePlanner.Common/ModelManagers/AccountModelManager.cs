using OpenRoutePlanner.DtoQuery;
using OpenRoutePlanner.Models;
using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Json;

namespace OpenRoutePlanner.ModelManagers;
public class AccountModelManager(HttpClient client) : IModelManager<BusinessAccount, AccountQuery>
{
    public async Task<HttpStatusCode> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await client.DeleteAsync($"/accounts/{id}", cancellationToken);
        return response.StatusCode;
    }

    public async Task<BusinessAccount?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await client.GetFromJsonAsync<BusinessAccount>($"/accounts/{id}", cancellationToken);
    }

    public async Task<ImmutableArray<BusinessAccount>> List(AccountQuery? query, CancellationToken cancellationToken = default)
    {
        ImmutableArray<BusinessAccount> items = ImmutableArray<BusinessAccount>.Empty;
        IAsyncEnumerable<BusinessAccount?> enumerable;

        if (query == null)
        {
            enumerable = client.GetFromJsonAsAsyncEnumerable<BusinessAccount>("/accounts", cancellationToken);
        }
        else
        {
            enumerable = client.GetFromJsonAsAsyncEnumerable<BusinessAccount>("/accounts".ToQueryString(query), cancellationToken);
        }

        await foreach (BusinessAccount? x in enumerable)
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

    public async Task<HttpStatusCode> Post(BusinessAccount model, CancellationToken cancellationToken = default)
    {
        await NormalizeModel(model, cancellationToken);
        HttpResponseMessage response = await client.PostAsJsonAsync($"/accounts", model, cancellationToken);
        return response.StatusCode;
    }

    public async Task<HttpStatusCode> Put(Guid id, BusinessAccount model, CancellationToken cancellationToken = default)
    {
        await NormalizeModel(model, cancellationToken);
        HttpResponseMessage response = await client.PutAsJsonAsync($"/accounts/{id}", model, cancellationToken);
        return response.StatusCode;
    }

    public Task NormalizeModel(BusinessAccount model, CancellationToken cancellationToken = default)
    {
        string cityState = string.Empty;

        if (model.Address != null && model.Address.IndexOf(',') != -1)
        {
            int index = model.Address.IndexOf(',') + 1;
            cityState = model.Address.Substring(index, model.Address.Length - index);
        }

        model.FullName = $"{model.ShortName} - {cityState}";
        return Task.CompletedTask;
    }
}
