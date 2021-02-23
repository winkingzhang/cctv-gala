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
    public class ProgramsControllerTest
    {
        private Mock<IAmazonDynamoDB> _dynamodbMock;
        private IPaginationUriService _paginationUriService;
        private Mock<ILogger<ProgramsController>> _logger;

        private void SetupMocks()
        {
            _dynamodbMock = new Mock<IAmazonDynamoDB>();
            _paginationUriService = new PaginationUriService("http://localhost:5000/");
            _logger = new Mock<ILogger<ProgramsController>>();
        }

        [Fact]
        public async Task Should_Create_Program_WithValidInput()
        {
            SetupMocks();
            var programsController = new ProgramsController(_dynamodbMock.Object, _paginationUriService, _logger.Object);
            Assert.NotNull(programsController);

            var programRequest = new Request<ProgramViewModel.Creation>();
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
            SetupMocks();

            var programsController = new ProgramsController(_dynamodbMock.Object, _paginationUriService, _logger.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Request = {Path = new PathString("/api/programs")}
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
            SetupMocks();

            var programsController = new ProgramsController(_dynamodbMock.Object, _paginationUriService, _logger.Object);
            Assert.NotNull(programsController);

            var program = await programsController.EditProgramByIdAsync(
                Guid.NewGuid(),
                new Request<ProgramViewModel.Edit>()
            ) as OkObjectResult;
            Assert.NotNull(program);
            var ProgramResponse = program.Value as Response<ProgramViewModel>;
            Assert.NotNull(ProgramResponse);
        }

        [Fact]
        public async Task Should_Get_Program_When_EditProgramById()
        {
            SetupMocks();

            var programsController = new ProgramsController(_dynamodbMock.Object, _paginationUriService, _logger.Object);
            Assert.NotNull(programsController);

            var program = await programsController.GetProgramByIdAsync(Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(program);
            var programResponse = program.Value as Response<ProgramViewModel>;
            Assert.NotNull(programResponse);
        }

        [Fact]
        public async Task Should_Get_Program_When_DeleteProgramById()
        {
            SetupMocks();

            var programsController = new ProgramsController(_dynamodbMock.Object, _paginationUriService, _logger.Object);
            Assert.NotNull(programsController);

            var program = await programsController.DeleteProgramByIdAsync(Guid.NewGuid()) as OkObjectResult;
            Assert.NotNull(program);
            var programResponse = program.Value as Response<ProgramViewModel>;
            Assert.NotNull(programResponse);
        }
    }
}