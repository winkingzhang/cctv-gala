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
using Thoughtworks.Gala.WebApi.Pagination;
using Thoughtworks.Gala.WebApi.Repositories;
using Thoughtworks.Gala.WebApi.UnitTests.Utils;
using Thoughtworks.Gala.WebApi.ValueObjects;
using Thoughtworks.Gala.WebApi.ViewModels;
using Xunit;

namespace Thoughtworks.Gala.WebApi.UnitTests.Controllers
{
    public class ProgramsControllerTest : AutoMapperAwareTest
    {
        private Mock<IRepository<Guid, ProgramEntity>> _repoMock;
        private IPaginationUriService _paginationUriService;
        private Mock<ILogger<ProgramsController>> _logger;

        public ProgramsControllerTest(AutoMapperFixture fixture) : base(fixture)
        {
            SetupMocks();
        }

        private void SetupMocks()
        {
            _repoMock = new Mock<IRepository<Guid, ProgramEntity>>();
            _paginationUriService = new PaginationUriService("http://localhost:5000/");
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
            _repoMock.Setup(repo => repo.CreateEntityAsync(It.IsAny<ProgramEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var programsController = new ProgramsController(_repoMock.Object, Mapper, _paginationUriService, _logger.Object);
            Assert.NotNull(programsController);

            var programRequest = new Request<ProgramViewModel.Creation>()
            {
                Data = new ProgramViewModel.Creation() { Name = "test" }
            };
            var programs = await programsController.CreateProgramAsync(programRequest) as CreatedAtRouteResult;
            Assert.NotNull(programs);
            Assert.Equal("GetProgramById", programs.RouteName);
            Assert.NotNull(programs.RouteValues);

            var programResponse = programs.Value as Response<ProgramViewModel>;
            Assert.NotNull(programResponse);

            // Assert.NotNull(programResponse.Data);
        }

        [Fact]
        public async Task Should_Get_ProgramList()
        {
            var programsController = new ProgramsController(_repoMock.Object, Mapper, _paginationUriService, _logger.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Request = { Path = new PathString("/api/programs") }
                    }
                }
            };

            Assert.NotNull(programsController);

            var programs = await programsController.GetProgramsAsync(new PaginationFilter(), Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(programs);
            var programsResponse = programs.Value as Response<IEnumerable<ProgramViewModel>>;
            Assert.NotNull(programsResponse);
        }

        [Fact]
        public async Task Should_Get_Program_By_Id()
        {
            var expectedEntity = new ProgramEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock.Setup(repo => repo.ReadEntityAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);

            var programsController = new ProgramsController(_repoMock.Object, Mapper, _paginationUriService, _logger.Object);
            Assert.NotNull(programsController);

            var program = await programsController.GetProgramByIdAsync(Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(program);
            var ProgramResponse = program.Value as Response<ProgramViewModel>;
            Assert.NotNull(ProgramResponse);
        }

        [Fact]
        public async Task Should_Get_Program_When_EditProgramById()
        {
            var expectedEntity = new ProgramEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock.Setup(repo =>
                    repo.UpdateEntityAsync(It.IsAny<Guid>(), It.IsAny<ProgramEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var programsController = new ProgramsController(_repoMock.Object, Mapper, _paginationUriService, _logger.Object);
            Assert.NotNull(programsController);

            var program = await programsController.EditProgramByIdAsync(
                Guid.NewGuid(),
                new Request<ProgramViewModel.Edit>()
                {
                    Data = new ProgramViewModel.Edit()
                }
            ) as OkObjectResult;
            Assert.NotNull(program);
            var programResponse = program.Value as Response<ProgramViewModel>;
            Assert.NotNull(programResponse);
        }

        [Fact]
        public async Task Should_Get_Program_When_DeleteProgramById()
        {
            var expectedEntity = new ProgramEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName"
            };
            _repoMock.Setup(repo =>
                    repo.DeleteEntityAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);

            var programsController = new ProgramsController(_repoMock.Object, Mapper, _paginationUriService, _logger.Object);
            Assert.NotNull(programsController);

            var program = await programsController.DeleteProgramByIdAsync(Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(program);
            var programResponse = program.Value as Response<ProgramViewModel>;
            Assert.NotNull(programResponse);
        }
    }
}