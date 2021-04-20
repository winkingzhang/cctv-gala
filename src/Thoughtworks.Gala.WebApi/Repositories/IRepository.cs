using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Thoughtworks.Gala.WebApi.Entities;

namespace Thoughtworks.Gala.WebApi.Repositories
{
    public interface IRepository<TKey, TEntity>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<TEntity> CreateEntityAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<TEntity> ReadEntityAsync(TKey key, CancellationToken cancellationToken = default);

        Task<IList<TEntity>> QueryEntitiesAsync(TKey[] keys, CancellationToken cancellationToken = default);

        Task<TEntity> UpdateEntityAsync(TKey key, TEntity entity, CancellationToken cancellationToken = default);

        Task<TEntity> DeleteEntityAsync(TKey key, bool hardDelete = false, CancellationToken cancellationToken = default);
    }
}
