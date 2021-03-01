using Amazon.DynamoDBv2.DataModel;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Thoughtworks.Gala.WebApi.Entities;
using Thoughtworks.Gala.WebApi.Exceptions;

using NotSupportedException = Thoughtworks.Gala.WebApi.Exceptions.NotSupportedException;

namespace Thoughtworks.Gala.WebApi.Repositories
{
    public abstract class Repository<TKey, TEntity>
        : IRepository<TKey, TEntity>, IQueryableRepository<TKey, TEntity>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        [NotNull]
        // ReSharper disable once MemberCanBePrivate.Global
        protected IDynamoDBContext Context { get; }

        protected Repository([NotNull] IDynamoDBContext context)
        {
            Context = context;
        }

        protected abstract TKey NextKey();

        public async Task<TEntity> CreateEntityAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default)
        {
            var key = NextKey();
            if (key.Equals(entity.Id))
            {
                throw new ConflictException($"{key}");
            }
            entity.Id = key;
            await Context.SaveAsync(entity, cancellationToken);
            return await Context.LoadAsync<TEntity>(key, cancellationToken);
        }

        public async Task<TEntity> ReadEntityAsync([NotNull] TKey key, CancellationToken cancellationToken = default)
        {
            var original = await Context.LoadAsync<TEntity>(key, cancellationToken);
            if (ReferenceEquals(null, original))
            {
                throw new NotFoundException($"{key}");
            }
            return original;
        }

        public async Task<TEntity> UpdateEntityAsync([NotNull] TKey key, [NotNull] TEntity entity, CancellationToken cancellationToken = default)
        {
            var original = await Context.LoadAsync<TEntity>(key, cancellationToken);
            if (ReferenceEquals(null, original))
            {
                throw new NotFoundException($"{key}");
            }

            var assignable = original as IAssignableEntity<TKey>;
            if (ReferenceEquals(null, assignable))
            {
                throw new NotSupportedException(entity.GetType().Name);
            }

            await assignable.AssignFromAsync(entity);

            await Context.SaveAsync(assignable, cancellationToken);

            return assignable as TEntity;
        }

        public async Task<TEntity> DeleteEntityAsync([NotNull] TKey key, CancellationToken cancellationToken = default)
        {
            var original = await Context.LoadAsync<TEntity>(key, cancellationToken);
            if (ReferenceEquals(null, original))
            {
                throw new NotFoundException($"{key}");
            }
            await Context.DeleteAsync(key, cancellationToken);
            return original;
        }
    }
}
