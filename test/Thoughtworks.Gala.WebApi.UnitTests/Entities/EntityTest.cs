using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Thoughtworks.Gala.WebApi.Entities;
using Xunit;
using NotSupportedException = Thoughtworks.Gala.WebApi.Exceptions.NotSupportedException;

namespace Thoughtworks.Gala.WebApi.UnitTests.Entities
{
    public class EntityTest
    {
        [Fact]
        public async Task Should_Throw_NotSupportedException_When_AssignFromAsync_With_Not_ConvertToEntity()
        {
            await Assert.ThrowsAsync<NotSupportedException>(async () =>
            {
                await new GalaEntity().AssignFromAsync(new OtherEntity());
            });
            await Assert.ThrowsAsync<NotSupportedException>(async () =>
            {
                await new ProgramEntity().AssignFromAsync(new OtherEntity());
            });
            await Assert.ThrowsAsync<NotSupportedException>(async () =>
            {
                await new PerformerEntity().AssignFromAsync(new OtherEntity());
            });
        }

        [ExcludeFromCodeCoverage]
        private class OtherEntity : IEntity<Guid>
        {
            public Guid Id { get; set; }
        }
    }
}