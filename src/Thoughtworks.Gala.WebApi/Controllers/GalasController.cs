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
    /// Represent group of apis for galas
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
    public class GalasController : ControllerBase
    {
        private readonly IAmazonDynamoDB _amazonDynamoDb;
        private readonly IPaginationUriService _paginationUriService;
        private readonly ILogger<GalasController> _logger;

        public GalasController(
            IAmazonDynamoDB amazonDynamoDb,
            IPaginationUriService paginationUriService,
            ILogger<GalasController> logger)
        {
            _amazonDynamoDb = amazonDynamoDb;
            _paginationUriService = paginationUriService;
            _logger = logger;
        }

        // POST api/galas
        [HttpPost]
        [ProducesResponseType(typeof(Response<GalaViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse.BadRequest), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateGalaAsync(
            [FromBody] Request<GalaViewModel.Creation> galaRequest
        )
        {
            _logger.LogDebug("CreateGalaAsync");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            await Task.Delay(0);
            return CreatedAtRoute("GetGalaById",
                new { galaId = 0 },
                new Response<GalaViewModel>(default));
        }

        // GET api/galas?pageNumber=&pageSize=&years=2020&years=2019
        /// <summary>
        /// Get CCTV Gala list which ordered by year (most recent first)
        /// </summary>
        /// <returns>
        /// A enumerator indicate the collection of CCTV Galas
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<GalaViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGalasAsync(
            [FromQuery] PaginationFilter filter,
            [FromQuery] uint[] years
        )
        {
            _logger.LogDebug("GetGalasAsync");
            await Task.Delay(0);
            var data = Enumerable.Empty<GalaViewModel>();
            return Ok(data.ToPagedReponse(filter, data.Count(), _paginationUriService, Request.Path.Value));
        }

        // GET api/galas/{galaId}
        [HttpGet("{galaId}", Name = "GetGalaById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<GalaViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGalaByIdAsync(
            [FromRoute] Guid galaId
        )
        {
            _logger.LogDebug("GetGalaByIdAsync");
            await Task.Delay(0);
            return Ok(new Response<GalaViewModel>(default));
        }

        // GET api/galas/{galaId}/programs?pageNumber=&pageSize=
        [HttpGet("{galaId}/Programs")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<ProgramViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGalaProgramsAsync(
            [FromRoute] Guid galaId,
            [FromQuery] PaginationFilter filter
        )
        {
            _logger.LogDebug("GetGalaProgramsAsync");
            await Task.Delay(0);
            var data = Enumerable.Empty<ProgramViewModel>();
            return Ok(data.ToPagedReponse(filter, data.Count(), _paginationUriService, Request.Path.Value));
        }

        // PUT api/galas/{galaId}
        [HttpPut("{galaId}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<GalaViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse.BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditGalaByIdAsync(
            [FromRoute] Guid galaId,
            [FromBody] Request<GalaViewModel.Edit> galaRequest
        )
        {
            _logger.LogDebug("EditGalaByIdAsync");
            await Task.Delay(0);
            return Ok(new Response<GalaViewModel>(default));
        }

        // DELETE api/galas/{galaId}?hardDelete=true|false
        [HttpDelete("{galaId}")]
        [ProducesResponseType(typeof(Response<GalaViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteGalaByIdAsync(
            [FromRoute] Guid galaId,
            bool hardDelete = false
        )
        {
            _logger.LogDebug("EditGalaByIdAsync");
            await Task.Delay(0);
            return Ok(new Response<GalaViewModel>(default));
        }
    }
}