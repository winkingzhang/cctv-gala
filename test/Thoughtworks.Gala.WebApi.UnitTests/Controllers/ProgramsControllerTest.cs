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
    public class ProgramsControllerTest : AutoMapperAwareTest
    {
        private Mock<IProgramRepository>? _repoMock;
        private Mock<ILogger<ProgramsController>>? _logger;

        public ProgramsControllerTest(AutoMapperFixture fixture) : base(fixture)
        {
            SetupMocks();
        }

        private void SetupMocks()
        {
            _repoMock = new Mock<IProgramRepository>();
            _logger = new Mock<ILogger<ProgramsController>>();
        }

        [Fact]
        public async Task Should_Create_Program_WithValidInput()
        {
            var expectedEntity = new ProgramEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock!.Setup(repo => repo.CreateProgramEntityAsync(It.IsAny<ProgramEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var programsController = new ProgramsController(_repoMock.Object, Mapper, _logger!.Object);
            Assert.NotNull(programsController);

            var programRequest = new Request<ProgramViewModel.Creation>()
            {
                Data = new ProgramViewModel.Creation() {Name = "test"}
            };
            var programs = await programsController.CreateProgramAsync(programRequest) as CreatedAtRouteResult;
            Assert.NotNull(programs);
            Assert.Equal("GetProgramById", programs!.RouteName);
            Assert.NotNull(programs.RouteValues);

            var programResponse = programs.Value as Response<ProgramViewModel>;
            Assert.NotNull(programResponse);

            // Assert.NotNull(programResponse.Data);
        }

        [Fact]
        public async Task Should_BadRequest_Create_Program_WithInvalidInput()
        {
            var programsController = new ProgramsController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(programsController);
            programsController.ModelState.AddModelError("", "mocked");

            var programRequest = new Request<ProgramViewModel.Creation>()
            {
                Data = null
            };
            var badRequest = await programsController.CreateProgramAsync(programRequest) as BadRequestObjectResult;
            Assert.NotNull(badRequest);
        }

        [Fact]
        public async Task Should_Get_ProgramListByIds()
        {
            var expectedEntity = new ProgramEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock!.Setup(repo =>
                    repo.GetProgramEntityListByIdsAsync(It.IsAny<Guid[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProgramEntity>(new[] {expectedEntity}));
            var programsController = new ProgramsController(_repoMock.Object, Mapper, _logger!.Object);

            Assert.NotNull(programsController);

            var programs = await programsController.GetProgramsByIdsAsync(new[] {Guid.NewGuid()}) as OkObjectResult;
            Assert.NotNull(programs);
            var programsResponse = programs?.Value as Response<IList<ProgramViewModel>>;
            Assert.NotNull(programsResponse);
        }

        [Fact]
        public async Task Should_BadRequest_Get_ProgramListByIds_With_InvalidId()
        {
            var programsController = new ProgramsController(_repoMock!.Object, Mapper, _logger!.Object);

            Assert.NotNull(programsController);
            programsController.ModelState.AddModelError("", "mocked");

            var badRequest = await programsController.GetProgramsByIdsAsync(new Guid[0]) as BadRequestObjectResult;
            Assert.NotNull(badRequest);
        }

        [Fact]
        public async Task Should_Get_Program_By_Id()
        {
            var expectedEntity = new ProgramEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock!.Setup(repo => repo.ReadEntityAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);

            var programsController = new ProgramsController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(programsController);

            var program = await programsController.GetProgramByIdAsync(Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(program);
            var programResponse = program?.Value as Response<ProgramViewModel>;
            Assert.NotNull(programResponse);
        }

        [Fact]
        public async Task Should_NotFound_Get_Program_By_Id_If_Not_Found_From_Repo()
        {
            _repoMock!.Setup(repo => repo.ReadEntityAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProgramEntity?)null);

            var programsController = new ProgramsController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(programsController);

            var program = await programsController.GetProgramByIdAsync(Guid.NewGuid()) as NotFoundResult;
            Assert.NotNull(program);
        }

        [Fact]
        public async Task Should_Get_Program_When_EditProgramById()
        {
            var expectedEntity = new ProgramEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock!.Setup(repo =>
                    repo.UpdateEntityAsync(It.IsAny<Guid>(), It.IsAny<ProgramEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var programsController = new ProgramsController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(programsController);

            var program = await programsController.EditProgramByIdAsync(
                Guid.NewGuid(),
                new Request<ProgramViewModel.Edit>()
                {
                    Data = new ProgramViewModel.Edit()
                }
            ) as OkObjectResult;
            Assert.NotNull(program);
            var programResponse = program?.Value as Response<ProgramViewModel>;
            Assert.NotNull(programResponse);
        }

        [Fact]
        public async Task Should_BadRequest_Get_Program_When_EditProgramById_With_InvalidData()
        {
            var programsController = new ProgramsController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(programsController);
            programsController.ModelState.AddModelError("", "mocked");

            var badRequest = await programsController.EditProgramByIdAsync(
                Guid.NewGuid(),
                new Request<ProgramViewModel.Edit>()
                {
                    Data = null
                }
            ) as BadRequestObjectResult;
            Assert.NotNull(badRequest);
        }

        [Fact]
        public async Task Should_Get_Program_When_DeleteProgramById()
        {
            var expectedEntity = new ProgramEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock!.Setup(repo =>
                    repo.DeleteEntityAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);

            var programsController = new ProgramsController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(programsController);

            var program = await programsController.DeleteProgramByIdAsync(Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(program);
            var programResponse = program?.Value as Response<ProgramViewModel>;
            Assert.NotNull(programResponse);
        }
    }
}