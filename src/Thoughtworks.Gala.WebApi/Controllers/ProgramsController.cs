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
    public class ProgramsController : ControllerBase
    {
        private readonly IAmazonDynamoDB _amazonDynamoDb;
        private readonly IPaginationUriService _paginationUriService;
        private readonly ILogger<ProgramsController> _logger;

        public ProgramsController(
            IAmazonDynamoDB amazonDynamoDb,
            IPaginationUriService paginationUriService,
            ILogger<ProgramsController> logger)
        {
            _amazonDynamoDb = amazonDynamoDb;
            _paginationUriService = paginationUriService;
            _logger = logger;
        }

        // POST api/programs
        [HttpPost]
        [ProducesResponseType(typeof(Response<ProgramViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse.BadRequest), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProgramAsync(
            [FromBody] Request<ProgramViewModel.Creation> programRequest
        )
        {
            _logger.LogDebug("CreateProgramAsync");
            await Task.Delay(0);
            return CreatedAtRoute("GetProgramById",
                new { galaId = 0, programId = 0 },
                new Response<ProgramViewModel>(default));
        }

        // GET api/programs?pageNumber=&pageSize=&galaId=
        /// <summary>
        /// Get CCTV Gala Program list which ordered by year (most recent first)
        /// </summary>
        /// <returns>
        /// A enumerator indicate the collection of CCTV Gala Program
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<ProgramViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProgramsAsync(
            [FromQuery] PaginationFilter filter,
            [FromQuery] Guid? galaId
        )
        {
            _logger.LogDebug("GetProgramsAsync");
            await Task.Delay(0);
            var data = Enumerable.Empty<ProgramViewModel>();
            return Ok(data.ToPagedReponse(filter, data.Count(), _paginationUriService, Request.Path.Value));
        }

        // GET api/programs/{programId}
        [HttpGet("{programId}", Name = "GetProgramById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<ProgramViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProgramByIdAsync(
            [FromRoute] Guid programId
        )
        {
            _logger.LogDebug("GetProgramByIdAsync");
            await Task.Delay(0);
            return Ok(new Response<ProgramViewModel>(default));
        }

        // PUT api/programs/{programId}
        [HttpPut("{programId}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<ProgramViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse.BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditProgramByIdAsync(
            [FromRoute] Guid programId,
            [FromBody] Request<ProgramViewModel.Edit> programRequest
        )
        {
            _logger.LogDebug("EditProgramByIdAsync");
            await Task.Delay(0);
            return Ok(new Response<ProgramViewModel>(default));
        }

        // DELETE api/programs/{programId}?hardDelete=true|false
        [HttpDelete("{programId}")]
        [ProducesResponseType(typeof(Response<ProgramViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProgramByIdAsync(
            [FromRoute] Guid programId,
            bool hardDelete = false
        )
        {
            _logger.LogDebug("EditProgramByIdAsync");
            await Task.Delay(0);
            return Ok(new Response<ProgramViewModel>(default));
        }
    }
}