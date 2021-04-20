using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
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
        : IRepository<TKey, TEntity>
        where TEntity : class, IEntity<TKey>, new()
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

        public async Task<TEntity> CreateEntityAsync([NotNull] TEntity entity,
            CancellationToken cancellationToken = default)
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
            if (original is null)
            {
                throw new NotFoundException($"{key}");
            }

            return original;
        }

        public async Task<TEntity> UpdateEntityAsync([NotNull] TKey key, [NotNull] TEntity entity,
            CancellationToken cancellationToken = default)
        {
            var original = await Context.LoadAsync<TEntity>(key, cancellationToken);
            if (original is null)
            {
                throw new NotFoundException($"{key}");
            }

            if (!(original is IAssignableEntity<TKey> assignable))
            {
                throw new NotSupportedException(entity.GetType().Name);
            }

            await assignable.AssignFromAsync(entity);

            await Context.SaveAsync(assignable as TEntity, cancellationToken);

            return assignable as TEntity;
        }

        public async Task<TEntity> DeleteEntityAsync(
            [NotNull] TKey key,
            bool hardDelete = false,
            CancellationToken cancellationToken = default
        )
        {
            var original = await Context.LoadAsync<TEntity>(key, cancellationToken);
            if (original is null)
            {
                throw new NotFoundException($"{key}");
            }

            if (hardDelete)
            {
                await Context.DeleteAsync(key, cancellationToken);
            }
            else if (original is ISoftDeletableEntity<TKey> marked)
            {
                marked.MarkAsDeleted();
                await Context.SaveAsync(marked, cancellationToken);
            }
            else
            {
                throw new NotFoundException(original.GetType().Name);
            }

            return original;
        }

        public async Task<IList<TEntity>> QueryEntitiesAsync(TKey[] keys, CancellationToken cancellationToken = default)
        {
            var batchGet = Context.CreateBatchGet<TEntity>();
            keys.ToList().ForEach(key => batchGet.AddKey(key));
            await batchGet.ExecuteAsync(cancellationToken);
            return batchGet.Results;
        }
    }
}