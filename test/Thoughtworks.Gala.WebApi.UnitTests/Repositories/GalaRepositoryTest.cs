using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Moq;
using Thoughtworks.Gala.WebApi.Entities;
using Thoughtworks.Gala.WebApi.Repositories;
using Xunit;
using NotSupportedException = Thoughtworks.Gala.WebApi.Exceptions.NotSupportedException;

namespace Thoughtworks.Gala.WebApi.UnitTests.Repositories
{
    public class GalaRepositoryTest
    {
        [Fact]
        public void Should_GetNextId_When_HaveGalaRepository()
        {
            var mockedContext = new Mock<IDynamoDBContext>();
            var galaRepo = new GalaRepository(mockedContext.Object);
            var nextId = typeof(GalaRepository).GetMethod(
                "NextKey",
                BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.NonPublic
            );
            Assert.NotNull(nextId);
            Assert.IsType<Guid>(nextId.Invoke(galaRepo, new object[0]));
        }

        [Fact]
        public async Task Should_GetGalaEntity_When_ReadEntityAsync()
        {
            var mockedContext = new Mock<IDynamoDBContext>();
            mockedContext.Setup(dbc =>
                    dbc.LoadAsync<GalaEntity>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GalaEntity() {Id = Guid.NewGuid(), Name = "mock", Year = 2020});
            mockedContext.Setup(dbc =>
                    dbc.SaveAsync(It.IsAny<GalaEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            var galaRepo = new GalaRepository(mockedContext.Object);
            var gala = await galaRepo.UpdateEntityAsync(Guid.NewGuid(), new GalaEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mocked",
                Year = (uint)DateTime.Now.Year
            });
            Assert.NotNull(gala);
            Assert.IsType<Guid>(gala.Id);
            Assert.NotEqual<Guid>(Guid.Empty, gala.Id);
            Assert.Same("mocked", gala.Name);
            Assert.Equal((uint) DateTime.Now.Year, gala.Year);

            Assert.Throws<NotSupportedException>(() =>
            {
                gala.AssignFromAsync(new MockedSimpleEntity());
            });
        }
        
        private class MockedSimpleEntity : IEntity<Guid>
        {
            public Guid Id { get; set; }
        }
    }
}