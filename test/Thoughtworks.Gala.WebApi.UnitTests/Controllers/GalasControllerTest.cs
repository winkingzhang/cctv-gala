using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Thoughtworks.Gala.WebApi.Controllers;
using Thoughtworks.Gala.WebApi.Pagination;
using Thoughtworks.Gala.WebApi.ValueObjects;
using Thoughtworks.Gala.WebApi.ViewModels;
using Xunit;

namespace Thoughtworks.Gala.WebApi.UnitTests.Controllers
{
    public class GalasControllerTest
    {
        private Mock<IAmazonDynamoDB> _dynamodbMock;
        private IPaginationUriService _paginationUriService;
        private Mock<ILogger<GalasController>> _logger;

        private void SetupMocks()
        {
            _dynamodbMock = new Mock<IAmazonDynamoDB>();
            _paginationUriService = new PaginationUriService("http://localhost:5000/");
            _logger = new Mock<ILogger<GalasController>>();
        }

        [Fact]
        public async Task Should_Create_Gala_WithValidInput()
        {
            SetupMocks();
            var galasController = new GalasController(_dynamodbMock.Object, _paginationUriService, _logger.Object);
            Assert.NotNull(galasController);

            var galaRequest = new Request<GalaViewModel.Creation>();
            var galas = await galasController.CreateGalaAsync(galaRequest) as CreatedAtRouteResult;
            Assert.NotNull(galas);
            Assert.Equal("GetGalaById", galas.RouteName);
            Assert.NotNull(galas.RouteValues);

            var galaResponse = galas.Value as Response<GalaViewModel>;
            Assert.NotNull(galaResponse);

            // Assert.NotNull(galaResponse.Data);
        }

        [Fact]
        public async Task Should_Get_GalaList()
        {
            SetupMocks();

            var galasController = new GalasController(_dynamodbMock.Object, _paginationUriService, _logger.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Request = {Path = new PathString("/api/galas")}
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
        public async Task Should_Get_Gala_By_Id()
        {
            SetupMocks();

            var galasController = new GalasController(_dynamodbMock.Object, _paginationUriService, _logger.Object);
            Assert.NotNull(galasController);

            var gala = await galasController.EditGalaByIdAsync(
                Guid.NewGuid(),
                new Request<GalaViewModel.Edit>()
            ) as OkObjectResult;
            Assert.NotNull(gala);
            var galaResponse = gala.Value as Response<GalaViewModel>;
            Assert.NotNull(galaResponse);
        }

        [Fact]
        public async Task Should_Get_Gala_When_EditGalaById()
        {
            SetupMocks();

            var galasController = new GalasController(_dynamodbMock.Object, _paginationUriService, _logger.Object);
            Assert.NotNull(galasController);

            var gala = await galasController.GetGalaByIdAsync(Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(gala);
            var galaResponse = gala.Value as Response<GalaViewModel>;
            Assert.NotNull(galaResponse);
        }

        [Fact]
        public async Task Should_Get_Gala_When_DeleteGalaById()
        {
            SetupMocks();

            var galasController = new GalasController(_dynamodbMock.Object, _paginationUriService, _logger.Object);
            Assert.NotNull(galasController);

            var gala = await galasController.DeleteGalaByIdAsync(Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(gala);
            var galaResponse = gala.Value as Response<GalaViewModel>;
            Assert.NotNull(galaResponse);
        }
    }
}