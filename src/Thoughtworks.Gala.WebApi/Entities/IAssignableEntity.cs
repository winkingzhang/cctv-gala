using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Thoughtworks.Gala.WebApi.Entities
{
    public interface IAssignableEntity<TKey> : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        Task AssignFromAsync(IEntity<TKey> other);
    }
}
