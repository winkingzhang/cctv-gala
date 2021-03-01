using Amazon.DynamoDBv2.DataModel;
using System;
using System.Threading.Tasks;

using NotSupportedException = Thoughtworks.Gala.WebApi.Exceptions.NotSupportedException;

namespace Thoughtworks.Gala.WebApi.Entities
{
    [DynamoDBTable("Galas")]
    public class GalaEntity : IEntity<Guid>, IAssignableEntity<Guid>
    {
        [DynamoDBHashKey]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public uint Year { get; set; }

        public Task AssignFromAsync(IEntity<Guid> other)
        {
            var source = other as GalaEntity;
            if(ReferenceEquals(null, source))
            {
                throw new NotSupportedException(nameof(GalaEntity));
            }

            // ignore id
            Name = source.Name;
            Year = source.Year;

            return Task.CompletedTask;
        }
    }
}
