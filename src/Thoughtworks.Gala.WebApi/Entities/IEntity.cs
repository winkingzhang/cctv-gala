using System;

namespace Thoughtworks.Gala.WebApi.Entities
{
    public interface IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }
}
