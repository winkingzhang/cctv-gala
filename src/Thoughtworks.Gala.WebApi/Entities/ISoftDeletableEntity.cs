using System;

namespace Thoughtworks.Gala.WebApi.Entities
{
    public interface ISoftDeletableEntity<TKey> : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        void MarkAsDeleted();
    }
}