using Amazon.DynamoDBv2.DataModel;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using NotSupportedException = Thoughtworks.Gala.WebApi.Exceptions.NotSupportedException;

namespace Thoughtworks.Gala.WebApi.Entities
{
    [DynamoDBTable("Programs")]
    public sealed class ProgramEntity : IEntity<Guid>, IAssignableEntity<Guid>, ISoftDeletableEntity<Guid>
    {
        [DynamoDBHashKey("programId", Converter = typeof(GuidConverter))]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Introduction { get; set; }

        [DynamoDBProperty("Performers")] public Guid[] PerformerIds { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [DynamoDBVersion]
        public int? VersionNumber { get; set; }

        public Task AssignFromAsync(IEntity<Guid> other)
        {
            if (!(other is ProgramEntity source))
            {
                throw new NotSupportedException(nameof(ProgramEntity));
            }

            // ignore id
            Name = source.Name;
            Introduction = source.Introduction;
            PerformerIds = source.PerformerIds.ToArray();
            IsDeleted = source.IsDeleted;
            CreatedAt = source.CreatedAt;
            UpdatedAt = DateTime.UtcNow;

            return Task.CompletedTask;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}