using System;
using System.Linq;
using Thoughtworks.Gala.WebApi.Entities;

namespace Thoughtworks.Gala.WebApi.Repositories
{
    public interface IQueryableRepository<TKey, TEntity>
        : IRepository<TKey, TEntity>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
    }
}
