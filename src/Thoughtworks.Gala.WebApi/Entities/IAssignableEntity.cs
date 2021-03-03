using System;
using System.Threading.Tasks;

namespace Thoughtworks.Gala.WebApi.Entities
{
    public interface IAssignableEntity<TKey> : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        Task AssignFromAsync(IEntity<TKey> other);
    }
}