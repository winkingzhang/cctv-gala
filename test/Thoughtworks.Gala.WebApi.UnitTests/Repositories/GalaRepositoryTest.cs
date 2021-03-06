using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Moq;
using Thoughtworks.Gala.WebApi.Entities;
using Thoughtworks.Gala.WebApi.Repositories;
using Xunit;

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
            Assert.IsType<Guid>(nextId?.Invoke(galaRepo, new object[0]));
        }

        [Fact]
        public async Task Should_Get_Entity_When_ReadEntityAsync()
        {
            var galaId = Guid.NewGuid();
            
            var mockedContextMock = new Mock<IDynamoDBContext>();
            mockedContextMock.Setup(ctx =>
                    ctx.LoadAsync<GalaEntity>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GalaEntity()
                {
                    Id = galaId,
                    Name = "Origin",
                    IsDeleted = false,
                    Year = 2020,
                    CreatedAt = DateTime.Today.AddDays(-2),
                    UpdatedAt = DateTime.Today.AddDays(-1),
                    ProgramIds = new []{ Guid.NewGuid() },
                    VersionNumber = 2
                });
            mockedContextMock.Setup(ctx =>
                ctx.SaveAsync(It.IsAny<GalaEntity>(), It.IsAny<CancellationToken>()))
                .Verifiable();
            var mockedRepository = new GalaRepository(mockedContextMock.Object);
            var entity = await mockedRepository.UpdateEntityAsync(Guid.NewGuid(), new GalaEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Mocked",
                IsDeleted = false,
                Year = 2019u,
                CreatedAt = DateTime.Today.AddDays(-1),
                UpdatedAt = DateTime.Today,
                ProgramIds = new []{ Guid.NewGuid(), Guid.NewGuid() },
                VersionNumber = 2
            });
            
            mockedContextMock.Verify();
            Assert.NotNull(entity);
            Assert.Equal(galaId, entity.Id);
            Assert.Equal("Mocked", entity.Name);
            Assert.Equal(2019u, entity.Year);
            Assert.Equal(DateTime.Today.AddDays(-1), entity.CreatedAt);
            Assert.InRange(entity.UpdatedAt, DateTime.Today.ToUniversalTime(), DateTime.Now);
            Assert.NotNull(entity.ProgramIds);
            Assert.NotEmpty(entity.ProgramIds!);
            Assert.Equal(2, entity.VersionNumber);

            var entity2 = await mockedRepository.DeleteEntityAsync(galaId, false);
            mockedContextMock.Verify();
            Assert.NotNull(entity2);
            Assert.True(entity2.IsDeleted);
            Assert.InRange(entity.UpdatedAt, DateTime.Today.ToUniversalTime(), DateTime.Now);
        }
    }
}