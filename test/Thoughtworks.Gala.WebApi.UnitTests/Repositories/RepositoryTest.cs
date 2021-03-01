using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Moq;
using Thoughtworks.Gala.WebApi.Entities;
using Thoughtworks.Gala.WebApi.Exceptions;
using Thoughtworks.Gala.WebApi.Repositories;
using Xunit;

using NotSupportedException = Thoughtworks.Gala.WebApi.Exceptions.NotSupportedException;

namespace Thoughtworks.Gala.WebApi.UnitTests.Repositories
{
    public class RepositoryTest
    {
        [Fact]
        public async Task Should_Get_Entity_When_CreateEntityAsync()
        {
            var mockedContext = new Mock<IDynamoDBContext>();
            var mockedRepository = new MockedRepository(mockedContext);
            var entity = await mockedRepository.CreateEntityAsync(new MockedAssigableEntity(Guid.Empty));
            Assert.NotNull(entity);
        }
        
        [Fact]
        public void Should_Throw_ConflictException_When_CreateEntityAsync_WithExistId()
        {
            var mockedContext = new Mock<IDynamoDBContext>();
            var mockedRepository = new MockedSimpleRepository(mockedContext);
            Assert.Throws<ConflictException>(() =>
            {
                mockedRepository.CreateEntityAsync(new MockedSimpleEntity(MockedSimpleRepository.Id))
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            });
        }
        
        [Fact]
        public async Task Should_Get_Entity_When_ReadEntityAsync()
        {
            var mockedContext = new Mock<IDynamoDBContext>();
            var mockedRepository = new MockedRepository(mockedContext);
            var entity = await mockedRepository.ReadEntityAsync(Guid.NewGuid());
            Assert.NotNull(entity);
        }
        
        
        [Fact]
        public void Should_Throw_NotFoundException_When_ReadEntityAsync_WithNotExist()
        {
            var id = Guid.NewGuid();
            var mockedContext = new Mock<IDynamoDBContext>();
            var mockedRepository = new MockedSimpleRepository(mockedContext);
            mockedContext.Setup(dbc =>
                    dbc.LoadAsync<MockedSimpleEntity>(It.Is<Guid>(g => g == id), It.IsAny<CancellationToken>()))
                .ReturnsAsync((MockedSimpleEntity)null);
            Assert.Throws<NotFoundException>(() =>
            {
                mockedRepository.ReadEntityAsync(id)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            });
        }

        [Fact]
        public async Task Should_Get_Entity_When_UpdateEntityAsync()
        {
            var mockedContext = new Mock<IDynamoDBContext>();
            var mockedRepository = new MockedRepository(mockedContext);
            var entity = await mockedRepository.UpdateEntityAsync(Guid.NewGuid(), new MockedAssigableEntity(Guid.NewGuid()));
            Assert.NotNull(entity);
        }

        [Fact]
        public void Should_Throw_NotFoundException_When_UpdateEntityAsync_WithNotExist()
        {
            var id = Guid.NewGuid();
            var mockedContext = new Mock<IDynamoDBContext>();
            var mockedRepository = new MockedSimpleRepository(mockedContext);
            mockedContext.Setup(dbc =>
                    dbc.LoadAsync<MockedSimpleEntity>(It.Is<Guid>(g => g == id), It.IsAny<CancellationToken>()))
                .ReturnsAsync((MockedSimpleEntity)null);
            Assert.Throws<NotFoundException>(() =>
            {
                mockedRepository.UpdateEntityAsync(id, new MockedSimpleEntity(Guid.NewGuid()))
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            });
        }
        
        [Fact]
        public void Should_Throw_NotSupportedException_When_UpdateEntityAsync_WithSimpleEntity()
        {
            var id = Guid.NewGuid();
            var mockedContext = new Mock<IDynamoDBContext>();
            var mockedRepository = new MockedSimpleRepository(mockedContext);
            Assert.Throws<NotSupportedException>(() =>
            {
                mockedRepository.UpdateEntityAsync(id, new MockedSimpleEntity(Guid.NewGuid()))
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            });
        }

        [Fact]
        public async Task Should_Get_Entity_When_DeleteEntityAsync()
        {
            var mockedContext = new Mock<IDynamoDBContext>();
            var mockedRepository = new MockedRepository(mockedContext);
            var entity = await mockedRepository.DeleteEntityAsync(Guid.NewGuid());
            Assert.NotNull(entity);
        }

        [Fact]
        public void Should_Throw_NotFoundException_When_DeleteEntityAsync_WithNotExist()
        {
            var id = Guid.NewGuid();
            var mockedContext = new Mock<IDynamoDBContext>();
            var mockedRepository = new MockedSimpleRepository(mockedContext);
            mockedContext.Setup(dbc =>
                    dbc.LoadAsync<MockedSimpleEntity>(It.Is<Guid>(g => g == id), It.IsAny<CancellationToken>()))
                .ReturnsAsync((MockedSimpleEntity)null);
            Assert.Throws<NotFoundException>(() =>
            {
                mockedRepository.DeleteEntityAsync(id)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            });
        }

        private class MockedSimpleEntity : IEntity<Guid>
        {
            public MockedSimpleEntity(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; set; }
        }

        private class MockedAssigableEntity : IEntity<Guid>, IAssignableEntity<Guid>
        {
            public MockedAssigableEntity(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; set; }

            public string Name { get; set; }

            public Task AssignFromAsync(IEntity<Guid> other)
            {
                var source = other as MockedAssigableEntity;
                if (ReferenceEquals(null, source))
                {
                    throw new NotSupportedException(nameof(MockedAssigableEntity));
                }

                Name = source.Name;
                return Task.CompletedTask;
            }
        }

        private abstract class MockedBaseRepository<TKey, TEntity>
            : Repository<TKey, TEntity>
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            private Mock<IDynamoDBContext> _mockedContext;

            protected MockedBaseRepository(Mock<IDynamoDBContext> mockedContext) : base(mockedContext.Object)
            {
                _mockedContext = mockedContext;
                SetupMockedContext(mockedContext);
            }

            protected abstract TEntity CreateEntityInstance(TKey key);

            private void SetupMockedContext(Mock<IDynamoDBContext> mockedContext)
            {
                mockedContext.Setup(dbc => dbc.LoadAsync<TEntity>(It.IsAny<TKey>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(CreateEntityInstance(It.IsAny<TKey>()));
                mockedContext.Setup(dbc => dbc.SaveAsync<TEntity>(It.IsAny<TEntity>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.Delay(1));
                mockedContext.Setup(dbc => dbc.DeleteAsync<TKey>(It.IsAny<TKey>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.Delay(1));
            }
        }

        private class MockedSimpleRepository : MockedBaseRepository<Guid, MockedSimpleEntity>
        {
            internal static readonly Guid Id = Guid.NewGuid();
                
            public MockedSimpleRepository(Mock<IDynamoDBContext> mockedContext) : base(mockedContext)
            {
                mockedContext.Setup(dbc =>
                        dbc.LoadAsync<MockedSimpleEntity>(It.Is<Guid>(id => id == Id), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(CreateEntityInstance(Id));
            }

            protected override Guid NextKey() => Id;

            protected sealed override MockedSimpleEntity CreateEntityInstance(Guid key) => new MockedSimpleEntity(key);
        }

        private class MockedRepository : MockedBaseRepository<Guid, MockedAssigableEntity>
        {
            public MockedRepository(Mock<IDynamoDBContext> mockedContext) : base(mockedContext)
            {
            }

            protected override Guid NextKey() => Guid.NewGuid();
            protected override MockedAssigableEntity CreateEntityInstance(Guid key) => new MockedAssigableEntity(key);
        }
    }
}