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
using Thoughtworks.Gala.WebApi.Repositories;
using Thoughtworks.Gala.WebApi.UnitTests.Utils;
using Thoughtworks.Gala.WebApi.ValueObjects;
using Thoughtworks.Gala.WebApi.ViewModels;
using Xunit;

namespace Thoughtworks.Gala.WebApi.UnitTests.Controllers
{
    public class GalasControllerTest : AutoMapperAwareTest
    {
        private Mock<IGalaRepository>? _repoMock;
        private Mock<ILogger<GalasController>>? _logger;

        public GalasControllerTest(AutoMapperFixture fixture) : base(fixture)
        {
            SetupMocks();
        }

        private void SetupMocks()
        {
            _repoMock = new Mock<IGalaRepository>();
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
                ProgramIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
            };
            _repoMock!.Setup(repo => repo.CreateGalaEntityAsync(It.IsAny<GalaEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var galasController = new GalasController(_repoMock.Object, Mapper, _logger!.Object);
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
            var result = await galasController.CreateGalaAsync(galaRequest) as CreatedAtRouteResult;
            Assert.NotNull(result);
            Assert.Equal("GetGalaById", result!.RouteName);
            Assert.NotNull(result.RouteValues);

            var galaResponse = result.Value as Response<GalaViewModel>;
            Assert.NotNull(galaResponse);

            Assert.NotNull(galaResponse!.Data);
            Assert.Empty(galaResponse!.Message!);
        }

        [Fact]
        public async Task Should_Get_BadRequest_When_CreateGala_WithInvalidInput()
        {
            var expectedEntity = new GalaEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName",
                Year = 2020,
                ProgramIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
            };
            var galasController = new GalasController(_repoMock!.Object, Mapper, _logger!.Object);
            galasController.ModelState.AddModelError("name", "missing name");

            var galaRequest = new Request<GalaViewModel.Creation>
            {
                Data = null
            };
            var result = await galasController.CreateGalaAsync(galaRequest) as BadRequestObjectResult;
            Assert.NotNull(result);
            var galaResponse = result?.Value as ErrorResponse.BadRequest;
            Assert.NotNull(galaResponse);
            Assert.False(galaResponse?.Succeeded);
            Assert.NotNull(galaResponse?.Errors);
        }

        [Fact]
        public async Task Should_Get_GalaListByIds()
        {
            var galasController = new GalasController(_repoMock!.Object, Mapper, _logger!.Object);

            Assert.NotNull(galasController);
            _repoMock.Setup(repo => repo.GetGalaEntityListByIdsAsync(It.IsAny<Guid[]>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<IList<GalaEntity>>(new List<GalaEntity>() {new GalaEntity()}));
            var galas = await galasController.GetGalasByIdsAsync(new Guid[] {Guid.NewGuid()}) as OkObjectResult;
            Assert.NotNull(galas);
            var galasResponse = galas?.Value as Response<IList<GalaViewModel>>;
            Assert.NotNull(galasResponse);
        }

        [Fact]
        public async Task Should_BadRequest_Get_GalaListByIds_WithInvalidData()
        {
            var galasController = new GalasController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(galasController);
            var badRequest = await galasController.GetGalasByIdsAsync(new Guid[0]) as BadRequestObjectResult;
            Assert.NotNull(badRequest);
        }

        [Fact]
        public async Task Should_Get_GalaListByYears()
        {
            var galasController = new GalasController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(galasController);
            _repoMock.Setup(
                    repo => repo.GetGalaEntityListByYearsAsync(It.IsAny<int[]>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<IList<GalaEntity>>(new List<GalaEntity>() {new GalaEntity()}));
            var galas = await galasController.GetGalasByYearsAsync(new int[] {1999, 2005}) as OkObjectResult;
            Assert.NotNull(galas);
            var galasResponse = galas?.Value as Response<IList<GalaViewModel>>;
            Assert.NotNull(galasResponse);
        }

        [Fact]
        public async Task Should_BadRequest_Get_GalaListByYears()
        {
            var galasController = new GalasController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(galasController);
            var badRequest1 = await galasController.GetGalasByYearsAsync(new int[0]) as BadRequestObjectResult;
            Assert.NotNull(badRequest1);

            var badRequest2 = await galasController.GetGalasByYearsAsync(new int[] {1980}) as BadRequestObjectResult;
            Assert.NotNull(badRequest2);
        }

        [Fact]
        public async Task Should_Get_GalaListByZodiac()
        {
            var galasController = new GalasController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(galasController);
            _repoMock.Setup(repo =>
                    repo.GetGalaEntityListByZodiacAsync(It.IsAny<ChineseZodiac>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<IList<GalaEntity>>(new List<GalaEntity>() {new GalaEntity()}));
            var galas = await galasController.GetGalasByZodiacAsync(ChineseZodiac.Ox) as OkObjectResult;
            Assert.NotNull(galas);
            var galasResponse = galas?.Value as Response<IList<GalaViewModel>>;
            Assert.NotNull(galasResponse);
        }

        [Fact]
        public async Task Should_BadRequest_Get_GalaListByZodiac()
        {
            var galasController = new GalasController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(galasController);
            galasController.ModelState.AddModelError("", "mocked");
            var badRequest = await galasController.GetGalasByZodiacAsync(ChineseZodiac.Ox) as BadRequestObjectResult;
            Assert.NotNull(badRequest);
        }

        [Fact]
        public async Task Should_Get_Gala_By_Id()
        {
            var expectedEntity = new GalaEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName",
                Year = 2020,
                ProgramIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
            };
            _repoMock!.Setup(repo => repo.GetGalaEntityByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var galasController = new GalasController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(galasController);

            var gala = await galasController.GetGalaByIdAsync(Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(gala);
            var galaResponse = gala?.Value as Response<GalaViewModel>;
            Assert.NotNull(galaResponse);
            Assert.NotNull(galaResponse?.Data);
        }

        [Fact]
        public async Task Should_BadRequest_Get_Gala_By_Id_If_Not_Found_In_Repo()
        {
            _repoMock!.Setup(repo =>
                    repo.GetGalaEntityByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GalaEntity?) null);
            var galasController = new GalasController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(galasController);

            var notFound = await galasController.GetGalaByIdAsync(Guid.NewGuid()) as NotFoundResult;
            Assert.NotNull(notFound);
        }

        [Fact]
        public async Task Should_Get_Gala_When_EditGalaById()
        {
            var expectedEntity = new GalaEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName",
                Year = 2020,
                ProgramIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
            };
            _repoMock!.Setup(repo =>
                    repo.UpdateGalaEntityByIdAsync(It.IsAny<Guid>(), It.IsAny<GalaEntity>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var galasController = new GalasController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(galasController);

            var gala = await galasController.EditGalaByIdAsync(Guid.NewGuid(), new Request<GalaViewModel.Edit>()
            {
                Data = new GalaViewModel.Edit()
                {
                    GalaId = Guid.NewGuid(),
                    Name = "mock",
                    ProgramIds = new[] {Guid.NewGuid()},
                    Year = 2020
                }
            }) as OkObjectResult;
            Assert.NotNull(gala);
            var galaResponse = gala?.Value as Response<GalaViewModel>;
            Assert.NotNull(galaResponse);
            Assert.NotNull(galaResponse?.Data);
        }

        [Fact]
        public async Task Should_BadRequest_Get_Gala_When_EditGalaById_With_InvalidData()
        {
            var galasController = new GalasController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(galasController);
            galasController.ModelState.AddModelError("", "mocked");

            var badRequest = await galasController.EditGalaByIdAsync(Guid.NewGuid(), new Request<GalaViewModel.Edit>()
            {
                Data = null
            }) as BadRequestObjectResult;
            Assert.NotNull(badRequest);
        }

        [Fact]
        public async Task Should_Get_Gala_When_HardDeleteGalaById()
        {
            var expectedEntity = new GalaEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName",
                Year = 2020,
                ProgramIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
            };
            _repoMock!.Setup(repo =>
                    repo.DeleteGalaEntityByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var galasController = new GalasController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(galasController);

            var gala = await galasController.DeleteGalaByIdAsync(Guid.NewGuid(), true) as OkObjectResult;
            Assert.NotNull(gala);
            var galaResponse = gala?.Value as Response<GalaViewModel>;
            Assert.NotNull(galaResponse);
            Assert.NotNull(galaResponse?.Data);
        }

        [Fact]
        public async Task Should_Get_Gala_When_MarkAsDeletedGalaById()
        {
            var expectedEntity = new GalaEntity()
            {
                Id = Guid.NewGuid(),
                Name = "mockName",
                Year = 2020,
                ProgramIds = new[] {Guid.NewGuid(), Guid.NewGuid()}
            };
            _repoMock!.Setup(repo =>
                    repo.MarkGalaEntityAsDeletedAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEntity);
            var galasController = new GalasController(_repoMock!.Object, Mapper, _logger!.Object);
            Assert.NotNull(galasController);

            var gala = await galasController.DeleteGalaByIdAsync(Guid.NewGuid(), false) as OkObjectResult;
            Assert.NotNull(gala);
            var galaResponse = gala?.Value as Response<GalaViewModel>;
            Assert.NotNull(galaResponse);
            Assert.NotNull(galaResponse?.Data);
        }
    }
}