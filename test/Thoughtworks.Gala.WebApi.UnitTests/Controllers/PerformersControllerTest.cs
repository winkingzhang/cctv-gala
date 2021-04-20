using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Thoughtworks.Gala.WebApi.Controllers;
using Thoughtworks.Gala.WebApi.Entities;
using Thoughtworks.Gala.WebApi.Repositories;
using Thoughtworks.Gala.WebApi.UnitTests.Utils;
using Thoughtworks.Gala.WebApi.ValueObjects;
using Thoughtworks.Gala.WebApi.ViewModels;
using Xunit;

namespace Thoughtworks.Gala.WebApi.UnitTests.Controllers
{
    public class PerformersControllerTest : AutoMapperAwareTest
    {
        private Mock<IPerformerRepository> _repoMock;
        private Mock<ILogger<PerformersController>> _logger;

        public PerformersControllerTest(AutoMapperFixture fixture) : base(fixture)
        {
            SetupMocks();
        }

        private void SetupMocks()
        {
            _repoMock = new Mock<IPerformerRepository>();
            _logger = new Mock<ILogger<PerformersController>>();
        }

        [Fact]
        public async Task Should_Create_Performer_WithValidInput()
        {
            var expectedEntity = new PerformerEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock.Setup(repo => repo.CreateEntityAsync(It.IsAny<PerformerEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var performersController = new PerformersController(_repoMock.Object, Mapper, _logger.Object);
            Assert.NotNull(performersController);

            var performerRequest = new Request<PerformerViewModel.Creation>()
            {
                Data = new PerformerViewModel.Creation { Name = "mock" }
            };
            var performers = await performersController.CreatePerformerAsync(performerRequest) as CreatedAtRouteResult;
            Assert.NotNull(performers);
            Assert.Equal("GetPerformerById", performers.RouteName);
            Assert.NotNull(performers.RouteValues);

            var performerResponse = performers.Value as Response<PerformerViewModel>;
            Assert.NotNull(performerResponse);

            // Assert.NotNull(performerResponse.Data);
        }

        [Fact]
        public async Task Should_Get_PerformerList()
        {
            var performersController = new PerformersController(_repoMock.Object, Mapper, _logger.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Request = { Path = new PathString("/api/performers") }
                    }
                }
            };

            Assert.NotNull(performersController);

            var performers = await performersController.GetPerformersAsync() as OkObjectResult;
            Assert.NotNull(performers);
            var performersResponse = performers.Value as Response<IEnumerable<PerformerViewModel>>;
            Assert.NotNull(performersResponse);
        }

        [Fact]
        public async Task Should_Get_Performer_By_Id()
        {
            var expectedEntity = new PerformerEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock.Setup(repo => repo.ReadEntityAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);

            var performersController = new PerformersController(_repoMock.Object, Mapper, _logger.Object);
            Assert.NotNull(performersController);

            var performer = await performersController.GetPerformerByIdAsync(Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(performer);
            var PerformerResponse = performer.Value as Response<PerformerViewModel>;
            Assert.NotNull(PerformerResponse);
        }

        [Fact]
        public async Task Should_Get_Performer_When_EditPerformerById()
        {
            var expectedEntity = new PerformerEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock.Setup(repo =>
                    repo.UpdateEntityAsync(It.IsAny<Guid>(), It.IsAny<PerformerEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var performersController = new PerformersController(_repoMock.Object, Mapper, _logger.Object);
            Assert.NotNull(performersController);

            var performer = await performersController.EditPerformerByIdAsync(
                Guid.NewGuid(),
                new Request<PerformerViewModel.Edit>()
                {
                    Data = new PerformerViewModel.Edit()
                }
            ) as OkObjectResult;
            Assert.NotNull(performer);
            var performerResponse = performer.Value as Response<PerformerViewModel>;
            Assert.NotNull(performerResponse);
        }

        [Fact]
        public async Task Should_Get_Performer_When_DeletePerformerById()
        {
            var expectedEntity = new PerformerEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock.Setup(repo =>
                    repo.DeleteEntityAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var performersController = new PerformersController(_repoMock.Object, Mapper, _logger.Object);
            Assert.NotNull(performersController);

            var performer = await performersController.DeletePerformerByIdAsync(Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(performer);
            var performerResponse = performer.Value as Response<PerformerViewModel>;
            Assert.NotNull(performerResponse);
        }
    }
}