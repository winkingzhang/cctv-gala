using Amazon.DynamoDBv2.DataModel;
using System;
using System.Threading.Tasks;
using NotSupportedException = Thoughtworks.Gala.WebApi.Exceptions.NotSupportedException;

namespace Thoughtworks.Gala.WebApi.Entities
{
    [DynamoDBTable("Galas")]
    public class GalaEntity : IEntity<Guid>, IAssignableEntity<Guid>, ISoftDeletableEntity<Guid>
    {
        [DynamoDBHashKey("GalaId")] public Guid Id { get; set; }

        public string Name { get; set; }

        public uint Year { get; set; }

        [DynamoDBProperty("Programs")] public Guid[] ProgramIds { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [DynamoDBVersion]
        public int? VersionNumber { get; set; }

        public Task AssignFromAsync(IEntity<Guid> other)
        {
            if (!(other is GalaEntity source))
            {
                throw new NotSupportedException(nameof(GalaEntity));
            }

            // ignore id
            Name = source.Name;
            Year = source.Year;
            ProgramIds = source.ProgramIds;
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