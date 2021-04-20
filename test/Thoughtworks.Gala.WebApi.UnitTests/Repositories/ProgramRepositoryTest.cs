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
    public class ProgramRepositoryTest
    {
        [Fact]
        public void Should_GetNextId_When_HaveProgramRepository()
        {
            var mockedContext = new Mock<IDynamoDBContext>();
            var programRepo = new ProgramRepository(mockedContext.Object);
            var nextId = typeof(ProgramRepository).GetMethod(
                "NextKey",
                BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.NonPublic
            );
            Assert.NotNull(nextId);
            Assert.IsType<Guid>(nextId?.Invoke(programRepo, new object[0]));
        }

        [Fact]
        public async Task Should_Get_Entity_When_ReadEntityAsync()
        {
            var programId = Guid.NewGuid();

            var mockedContextMock = new Mock<IDynamoDBContext>();
            mockedContextMock.Setup(ctx =>
                    ctx.LoadAsync<ProgramEntity>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProgramEntity()
                {
                    Id = programId,
                    Name = "Origin",
                    Introduction = "Origin",
                    PerformerIds = new[] {Guid.NewGuid()},
                    IsDeleted = false,
                    CreatedAt = DateTime.Today.AddDays(-2),
                    UpdatedAt = DateTime.Today.AddDays(-1),
                    VersionNumber = 2
                });
            mockedContextMock.Setup(ctx =>
                    ctx.SaveAsync(It.IsAny<ProgramEntity>(), It.IsAny<CancellationToken>()))
                .Verifiable();
            var mockedRepository = new ProgramRepository(mockedContextMock.Object);
            var entity = await mockedRepository.UpdateEntityAsync(Guid.NewGuid(), new ProgramEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Mocked",
                Introduction = "Mocked",
                IsDeleted = false,
                PerformerIds = new[] {Guid.NewGuid(), Guid.NewGuid()},
                CreatedAt = DateTime.Today.AddDays(-1),
                UpdatedAt = DateTime.Today,
                VersionNumber = 2
            });

            mockedContextMock.Verify();
            Assert.NotNull(entity);
            Assert.Equal(programId, entity.Id);
            Assert.Equal("Mocked", entity.Name);
            Assert.Equal("Mocked", entity.Introduction);
            Assert.Equal(DateTime.Today.AddDays(-1), entity.CreatedAt);
            Assert.InRange(entity.UpdatedAt, DateTime.Today.ToUniversalTime(), DateTime.Now);
            Assert.Equal(2, entity.VersionNumber);
            Assert.NotNull(entity.PerformerIds);
            Assert.NotEmpty(entity.PerformerIds!);

            var entity2 = await mockedRepository.DeleteEntityAsync(programId, false);
            mockedContextMock.Verify();
            Assert.NotNull(entity2);
            Assert.True(entity2.IsDeleted);
            Assert.InRange(entity.UpdatedAt, DateTime.Today.ToUniversalTime(), DateTime.Now);
        }
    }
}