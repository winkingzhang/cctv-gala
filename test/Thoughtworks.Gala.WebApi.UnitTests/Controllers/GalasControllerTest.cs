using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    public class GalasControllerTest : AutoMapperAwareTest
    {
        private Mock<IRepository<Guid, GalaEntity>> _repoMock;
        private IPaginationUriService _paginationUriService;
        private Mock<ILogger<GalasController>> _logger;

        public GalasControllerTest(AutoMapperFixture fixture) : base(fixture)
        {
            SetupMocks();
        }

        private void SetupMocks()
        {
            _repoMock = new Mock<IRepository<Guid, GalaEntity>>();
            _paginationUriService = new PaginationUriService("http://localhost:5000/");
            _logger = new Mock<ILogger<GalasController>>();
        }

        [Fact]
        public async Task Should_Get_Gala_When_CreateGala_WithValidInput()
        {
            var expectedEntity = new GalaEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName",
                Year = 2020,
                ProgramIds = new[] { Guid.NewGuid(), Guid.NewGuid() }
            };
            _repoMock.Setup(repo => repo.CreateEntityAsync(It.IsAny<GalaEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var galasController = new GalasController(_repoMock.Object, Mapper, _paginationUriService, _logger.Object);
            Assert.NotNull(galasController);

            var galaRequest = new Request<GalaViewModel.Creation>
            {
                Data = new GalaViewModel.Creation()
                {
                    Name = expectedEntity.Name,
                    Year = expectedEntity.Year,
                    ProgramIds = expectedEntity.ProgramIds.ToImmutableList()
                }
            };
            var galas = await galasController.CreateGalaAsync(galaRequest) as CreatedAtRouteResult;
            Assert.NotNull(galas);
            Assert.Equal("GetGalaById", galas.RouteName);
            Assert.NotNull(galas.RouteValues);

            var galaResponse = galas.Value as Response<GalaViewModel>;
            Assert.NotNull(galaResponse);

            Assert.NotNull(galaResponse.Data);
        }

        [Fact]
        public async Task Should_Get_BadRequest_When_CreateGala_WithInvalidInput()
        {
            var expectedEntity = new GalaEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName",
                Year = 2020,
                ProgramIds = new[] { Guid.NewGuid(), Guid.NewGuid() }
            };
            var galasController = new GalasController(_repoMock.Object, Mapper, _paginationUriService, _logger.Object)
            {
                ControllerContext = new ControllerContext()
            };
            galasController.ModelState.AddModelError("name", "missing name");

            var galaRequest = new Request<GalaViewModel.Creation>
            {
                Data = new GalaViewModel.Creation()
                {
                    Name = expectedEntity.Name,
                    Year = expectedEntity.Year,
                    ProgramIds = expectedEntity.ProgramIds
                }
            };
            var galas = await galasController.CreateGalaAsync(galaRequest) as BadRequestObjectResult;
            Assert.NotNull(galas);
            var galaResponse = galas.Value as ErrorResponse.BadRequest;
            Assert.NotNull(galaResponse);
            Assert.False(galaResponse.Succeeded);
            Assert.NotNull(galaResponse.Errors);
        }

        [Fact]
        public async Task Should_Get_GalaList()
        {
            var galasController = new GalasController(_repoMock.Object, Mapper, _paginationUriService, _logger.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Request = { Path = new PathString("/api/galas") }
                    }
                }
            };

            Assert.NotNull(galasController);

            var galas = await galasController.GetGalasAsync(new PaginationFilter(), new uint[0]) as OkObjectResult;
            Assert.NotNull(galas);
            var galasResponse = galas.Value as Response<IEnumerable<GalaViewModel>>;
            Assert.NotNull(galasResponse);
        }

        [Fact]
        public async Task Should_Get_ProgramListByGalaId()
        {
            var galasController = new GalasController(_repoMock.Object, Mapper, _paginationUriService, _logger.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Request = { Path = new PathString($"/api/galas/{Guid.NewGuid()}/programs") }
                    }
                }
            };

            Assert.NotNull(galasController);

            var galaPrograms =
                await galasController.GetGalaProgramsAsync(Guid.NewGuid(), new PaginationFilter()) as OkObjectResult;
            Assert.NotNull(galaPrograms);
            var galasResponse = galaPrograms.Value as Response<IEnumerable<ProgramViewModel>>;
            Assert.NotNull(galasResponse);
        }

        [Fact]
        public async Task Should_Get_Gala_By_Id()
        {
            var expectedEntity = new GalaEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName",
                Year = 2020,
                ProgramIds = new[] { Guid.NewGuid(), Guid.NewGuid() }
            };
            _repoMock.Setup(repo => repo.ReadEntityAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var galasController = new GalasController(_repoMock.Object, Mapper, _paginationUriService, _logger.Object);
            Assert.NotNull(galasController);

            var gala = await galasController.GetGalaByIdAsync(Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(gala);
            var galaResponse = gala.Value as Response<GalaViewModel>;
            Assert.NotNull(galaResponse);
            Assert.NotNull(galaResponse.Data);
        }

        [Fact]
        public async Task Should_Get_Gala_When_EditGalaById()
        {
            var expectedEntity = new GalaEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName",
                Year = 2020,
                ProgramIds = new[] { Guid.NewGuid(), Guid.NewGuid() }
            };
            _repoMock.Setup(repo =>
                    repo.UpdateEntityAsync(It.IsAny<Guid>(), It.IsAny<GalaEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var galasController = new GalasController(_repoMock.Object, Mapper, _paginationUriService, _logger.Object);
            Assert.NotNull(galasController);

            var gala = await galasController.EditGalaByIdAsync(Guid.NewGuid(), new Request<GalaViewModel.Edit>()
            {
                Data = new GalaViewModel.Edit()
                {
                    GalaId = Guid.NewGuid(),
                    Name = "mock",
                    ProgramIds = new[] { Guid.NewGuid() },
                    Year = 2020
                }
            }) as OkObjectResult;
            Assert.NotNull(gala);
            var galaResponse = gala.Value as Response<GalaViewModel>;
            Assert.NotNull(galaResponse);
            Assert.NotNull(galaResponse.Data);
        }

        [Fact]
        public async Task Should_Get_Gala_When_DeleteGalaById()
        {
            var expectedEntity = new GalaEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName",
                Year = 2020,
                ProgramIds = new[] { Guid.NewGuid(), Guid.NewGuid() }
            };
            _repoMock.Setup(repo =>
                    repo.DeleteEntityAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var galasController = new GalasController(_repoMock.Object, Mapper, _paginationUriService, _logger.Object);
            Assert.NotNull(galasController);

            var gala = await galasController.DeleteGalaByIdAsync(Guid.NewGuid(), true) as OkObjectResult;
            Assert.NotNull(gala);
            var galaResponse = gala.Value as Response<GalaViewModel>;
            Assert.NotNull(galaResponse);
            Assert.NotNull(galaResponse.Data);
        }
    }
}