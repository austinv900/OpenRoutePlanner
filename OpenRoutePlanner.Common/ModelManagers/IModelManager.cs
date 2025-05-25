using System.Collections.Immutable;
using System.Net;

namespace OpenRoutePlanner.ModelManagers;

public interface IModelManager<TModel, TQuery> where TModel : class where TQuery : class
{
    public Task<ImmutableArray<TModel>> List(TQuery? query, CancellationToken cancellationToken = default);

    public Task<TModel?> Get(Guid id, CancellationToken cancellationToken = default);

    public Task<HttpStatusCode> Post(TModel model, CancellationToken cancellationToken = default);

    public Task<HttpStatusCode> Put(Guid id, TModel model, CancellationToken cancellationToken = default);

    public Task<HttpStatusCode> Delete(Guid id, CancellationToken cancellationToken = default);

    public Task NormalizeModel(TModel model, CancellationToken cancellationToken = default);
}
