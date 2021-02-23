using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Thoughtworks.Gala.WebApi.Pagination;
using Thoughtworks.Gala.WebApi.ValueObjects;
using Thoughtworks.Gala.WebApi.ViewModels;

namespace Thoughtworks.Gala.WebApi.Controllers
{
    /// <summary>
    /// Represent group of apis for performer
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
#if !DEBUG
    [Microsoft.AspNetCore.Authorization.Authorize]
#endif
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class PerformersController : ControllerBase
    {
        private readonly IAmazonDynamoDB _amazonDynamoDb;
        private readonly IPaginationUriService _paginationUriService;
        private readonly ILogger<PerformersController> _logger;

        public PerformersController(
            IAmazonDynamoDB amazonDynamoDb,
            IPaginationUriService paginationUriService,
            ILogger<PerformersController> logger
        )
        {
            _amazonDynamoDb = amazonDynamoDb;
            _paginationUriService = paginationUriService;
            _logger = logger;
        }

        // POST api/performers
        [HttpPost]
        [ProducesResponseType(typeof(Response<PerformerViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse.BadRequest), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePerformerAsync(
            [FromBody] Request<PerformerViewModel.Creation> performerRequest
        )
        {
            _logger.LogDebug("CreatePerformerAsync");
            await Task.Delay(0);
            return CreatedAtRoute("GetPerformerById",
                new {performerId = 0},
                new Response<PerformerViewModel>(default));
        }

        // GET api/performers?pageNumber=&pageSize=
        /// <summary>
        /// Get CCTV Performer list which ordered by year (most recent first)
        /// </summary>
        /// <returns>
        /// A enumerator indicate the collection of performers
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<PerformerViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPerformersAsync(
            [FromQuery] PaginationFilter filter
        )
        {
            _logger.LogDebug("GetPerformersAsync");
            await Task.Delay(0);
            var data = Enumerable.Empty<PerformerViewModel>();
            return Ok(data.ToPagedReponse(filter, data.Count(), _paginationUriService, Request.Path.Value));
        }

        // GET api/performers/{performerId}
        [HttpGet("{performerId}", Name = "GetPerformerById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<PerformerViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPerformerByIdAsync(
            [FromRoute] Guid performerId
        )
        {
            _logger.LogDebug("GetPerformerByIdAsync");
            await Task.Delay(0);
            return Ok(new Response<PerformerViewModel>(default));
        }

        // PUT api/performers/{performerId}
        [HttpPut("{performerId}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<PerformerViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse.BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditPerformerByIdAsync(
            [FromRoute] Guid performerId,
            [FromBody] Request<PerformerViewModel.Edit> performerRequest
        )
        {
            _logger.LogDebug("EditPerformerByIdAsync");
            await Task.Delay(0);
            return Ok(new Response<PerformerViewModel>(default));
        }

        // DELETE api/performers/{performerId}?hardDelete=true|false
        [HttpDelete("{performerId}")]
        [ProducesResponseType(typeof(Response<PerformerViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePerformerByIdAsync(
            [FromRoute] Guid performerId,
            bool hardDelete = false
        )
        {
            _logger.LogDebug("EditPerformerByIdAsync");
            await Task.Delay(0);
            return Ok(new Response<PerformerViewModel>(default));
        }
    }
}