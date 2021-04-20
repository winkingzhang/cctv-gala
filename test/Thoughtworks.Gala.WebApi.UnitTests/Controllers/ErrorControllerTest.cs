using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Thoughtworks.Gala.WebApi.Controllers;
using Thoughtworks.Gala.WebApi.Exceptions;
using Xunit;
using NotSupportedException = Thoughtworks.Gala.WebApi.Exceptions.NotSupportedException;

namespace Thoughtworks.Gala.WebApi.UnitTests.Controllers
{
    public class ErrorControllerTest
    {
        private static ErrorController Setup(Exception e)
        {
            var logger = Mock.Of<ILogger<ErrorController>>();
            var controller = new ErrorController(logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Response = {StatusCode = StatusCodes.Status500InternalServerError}
                    }
                }
            };
            controller.HttpContext.Features.Set<IExceptionHandlerFeature>(
                new ExceptionHandlerFeature
                {
                    Error = e
                });
            return controller;
        }

        [Fact]
        private async Task Should_Get_NotImplemented_When_GetErrorResponse()
        {
            var errorController = Setup(new NotSupportedException("mock"));
            await errorController.GetErrorResponseAsync();

            Assert.Equal(StatusCodes.Status501NotImplemented,
                errorController.HttpContext.Response.StatusCode);
        }

        [Fact]
        private async Task Should_Get_BadRequest_When_GetErrorResponse()
        {
            var errorController = Setup(new ConflictException("mock"));
            await errorController.GetErrorResponseAsync();

            Assert.Equal(StatusCodes.Status400BadRequest,
                errorController.HttpContext.Response.StatusCode);
        }

        [Fact]
        private async Task Should_Get_NotFound_When_GetErrorResponse()
        {
            var errorController = Setup(new NotFoundException("mock"));
            await errorController.GetErrorResponseAsync();

            Assert.Equal(StatusCodes.Status404NotFound,
                errorController.HttpContext.Response.StatusCode);
        }

        [Fact]
        private async Task Should_Get_InternalServerError_When_GetErrorResponse()
        {
            var errorController = Setup(new Exception("mock"));
            await errorController.GetErrorResponseAsync();

            Assert.Equal(StatusCodes.Status500InternalServerError,
                errorController.HttpContext.Response.StatusCode);
        }
    }
}