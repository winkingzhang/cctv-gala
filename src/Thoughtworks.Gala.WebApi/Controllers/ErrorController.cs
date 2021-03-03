using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Thoughtworks.Gala.WebApi.Exceptions;

namespace Thoughtworks.Gala.WebApi.Controllers
{
    /// <summary>
    /// Represent group of apis for galas
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("error")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        public Task GetErrorResponse()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;

            _logger.LogError($"internal error: \r\n{exception}");

            Response.StatusCode = exception switch
            {
                NotSupportedException _ => StatusCodes.Status501NotImplemented,
                ConflictException _ => StatusCodes.Status400BadRequest,
                NotFoundException _ => StatusCodes.Status404NotFound,
                _ => Response.StatusCode
            };

            return Task.CompletedTask;
        }
    }
}