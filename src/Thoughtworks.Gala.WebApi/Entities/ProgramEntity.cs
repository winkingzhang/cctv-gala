using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using NotSupportedException = Thoughtworks.Gala.WebApi.Exceptions.NotSupportedException;

namespace Thoughtworks.Gala.WebApi.Entities
{
    [DynamoDBTable("Programs")]
    public sealed class ProgramEntity : IEntity<Guid>, IAssignableEntity<Guid>
    {
        [DynamoDBHashKey("programId", Converter = typeof(GuidConverter))]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Introduction { get; set; }

        [DynamoDBProperty("Performers")]
        public IReadOnlyList<Guid> PerformerIds { get; set; }

        public Task AssignFromAsync(IEntity<Guid> other)
        {
            if (!(other is ProgramEntity source))
            {
                throw new NotSupportedException(nameof(ProgramEntity));
            }

            // ignore id
            Name = source.Name;
            Introduction = source.Introduction;
            PerformerIds = source.PerformerIds?.ToList().AsReadOnly();

            return Task.CompletedTask;
        }
    }
}
