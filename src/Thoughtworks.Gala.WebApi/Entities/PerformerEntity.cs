using Amazon.DynamoDBv2.DataModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

using NotSupportedException = Thoughtworks.Gala.WebApi.Exceptions.NotSupportedException;

namespace Thoughtworks.Gala.WebApi.Entities
{
    [DynamoDBTable("Performers")]
    public sealed class PerformerEntity : IEntity<Guid>, IAssignableEntity<Guid>
    {
        [DynamoDBHashKey("performerId", Converter = typeof(GuidConverter))]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Introduction { get; set; }

        public Task AssignFromAsync(IEntity<Guid> other)
        {
            if (!(other is PerformerEntity source))
            {
                throw new NotSupportedException(nameof(PerformerEntity));
            }

            // ignore id
            Name = source.Name;
            Introduction = source.Introduction;

            return Task.CompletedTask;
        }
    }
}
