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
        private Mock<IPerformerRepository>? _repoMock;
        private Mock<ILogger<PerformersController>>? _logger;

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
            _repoMock!.Setup(repo => repo.CreatePerformerEntityAsync(It.IsAny<PerformerEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var performersController = new PerformersController(_repoMock.Object, Mapper, _logger!.Object);
            Assert.NotNull(performersController);

            var performerRequest = new Request<PerformerViewModel.Creation>()
            {
                Data = new PerformerViewModel.Creation {Name = "mock"}
            };
            var performers = await performersController.CreatePerformerAsync(performerRequest) as CreatedAtRouteResult;
            Assert.NotNull(performers);
            Assert.Equal("GetPerformerById", performers!.RouteName);
            Assert.NotNull(performers.RouteValues);

            var performerResponse = performers.Value as Response<PerformerViewModel>;
            Assert.NotNull(performerResponse);

            // Assert.NotNull(performerResponse.Data);
        }

        [Fact]
        public async Task Should_Get_BadRequest_When_Create_Performer_WithInvalidInput()
        {
            var performersController = new PerformersController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(performersController);

            var performerRequest = new Request<PerformerViewModel.Creation>()
            {
                Data = null
            };
            performersController.ModelState.TryAddModelError("", "mock");
            var performersResult =
                await performersController.CreatePerformerAsync(performerRequest) as BadRequestObjectResult;
            Assert.NotNull(performersResult);
            var performerResponse = performersResult!.Value as ErrorResponse.BadRequest;
            Assert.NotNull(performerResponse);
        }

        [Fact]
        public async Task Should_Get_PerformerList()
        {
            var expectedEntity = new PerformerEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock!.Setup(repo =>
                    repo.GetPerformerEntityListByIdsAsync(It.IsAny<Guid[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PerformerEntity>() {expectedEntity});
            var performersController = new PerformersController(_repoMock.Object, Mapper, _logger!.Object);

            Assert.NotNull(performersController);

            var performersResult = await performersController.GetPerformersAsync(
                new Guid[] {Guid.NewGuid()}) as OkObjectResult;
            Assert.NotNull(performersResult);
            var performersResponse = performersResult?.Value as Response<IList<PerformerViewModel>>;
            Assert.NotNull(performersResponse);
        }

        [Fact]
        public async Task Should_Get_BadRequest_When_GetPerformerListById_With_Empty()
        {
            var performersController = new PerformersController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(performersController);

            var performersResult = await performersController.GetPerformersAsync(
                new Guid[0]) as BadRequestObjectResult;
            Assert.NotNull(performersResult);
            var performerResponse = performersResult?.Value as ErrorResponse.BadRequest;
            Assert.NotNull(performerResponse);
        }

        [Fact]
        public async Task Should_Get_BadRequest_When_GetPerformerListById()
        {
            var performersController = new PerformersController(_repoMock!.Object, Mapper, _logger!.Object);
            performersController.ModelState.TryAddModelError("", "mock");
            Assert.NotNull(performersController);

            var performersResult = await performersController.GetPerformersAsync(
                new Guid[] {Guid.NewGuid()}) as BadRequestObjectResult;
            Assert.NotNull(performersResult);
            var performerResponse = performersResult?.Value as ErrorResponse.BadRequest;
            Assert.NotNull(performerResponse);
        }

        [Fact]
        public async Task Should_Get_Performer_By_Id()
        {
            var expectedEntity = new PerformerEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock!.Setup(repo => repo.ReadEntityAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);

            var performersController = new PerformersController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(performersController);

            var performer = await performersController.GetPerformerByIdAsync(Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(performer);
            var performerResponse = performer?.Value as Response<PerformerViewModel>;
            Assert.NotNull(performerResponse);
        }

        [Fact]
        public async Task Should_Get_Performer_When_EditPerformerById()
        {
            var expectedEntity = new PerformerEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock!.Setup(repo =>
                    repo.UpdateEntityAsync(It.IsAny<Guid>(), It.IsAny<PerformerEntity>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var performersController = new PerformersController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(performersController);

            var performer = await performersController.EditPerformerByIdAsync(
                Guid.NewGuid(),
                new Request<PerformerViewModel.Edit>()
                {
                    Data = new PerformerViewModel.Edit()
                }
            ) as OkObjectResult;
            Assert.NotNull(performer);
            var performerResponse = performer?.Value as Response<PerformerViewModel>;
            Assert.NotNull(performerResponse);
        }

        [Fact]
        public async Task Should_Get_BadRequest_When_EditPerformerById_With_Invalid()
        {
            var performersController = new PerformersController(_repoMock!.Object, Mapper, _logger!.Object);
            performersController.ModelState.TryAddModelError("", "mock");
            Assert.NotNull(performersController);

            var performer = await performersController.EditPerformerByIdAsync(
                Guid.NewGuid(),
                new Request<PerformerViewModel.Edit>()
                {
                    Data = new PerformerViewModel.Edit()
                }
            ) as BadRequestObjectResult;
            Assert.NotNull(performer);
            var performerResponse = performer?.Value as ErrorResponse.BadRequest;
            Assert.NotNull(performerResponse);
        }

        [Fact]
        public async Task Should_Get_Performer_When_MarkPerformerAsDeletedById()
        {
            var expectedEntity = new PerformerEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock!.Setup(repo =>
                    repo.MarkPerformerEntityAsDeletedAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var performersController = new PerformersController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(performersController);

            var performer = await performersController.DeletePerformerByIdAsync(Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(performer);
            var performerResponse = performer?.Value as Response<PerformerViewModel>;
            Assert.NotNull(performerResponse);
        }

        [Fact]
        public async Task Should_Get_Performer_When_HardDeletePerformerById()
        {
            var expectedEntity = new PerformerEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock!.Setup(repo =>
                    repo.DeletePerformerEntityByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var performersController = new PerformersController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(performersController);

            var performer = await performersController.DeletePerformerByIdAsync(Guid.NewGuid(), true) as OkObjectResult;
            Assert.NotNull(performer);
            var performerResponse = performer?.Value as Response<PerformerViewModel>;
            Assert.NotNull(performerResponse);
        }
    }
}