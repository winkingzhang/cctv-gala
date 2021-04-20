using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Thoughtworks.Gala.WebApi.Entities;
using Thoughtworks.Gala.WebApi.Repositories;
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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class PerformersController : ControllerBase
    {
        private readonly IRepository<Guid, PerformerEntity> _performerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PerformersController> _logger;

        public PerformersController(
            IRepository<Guid, PerformerEntity> performerRepository,
            IMapper mapper,
            ILogger<PerformersController> logger
        )
        {
            _performerRepository = performerRepository;
            _mapper = mapper;
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

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var performerEntity = _mapper.Map<PerformerViewModel.Creation, PerformerEntity>(performerRequest.Data);
            var createdEntity = await _performerRepository.CreateEntityAsync(performerEntity, CancellationToken.None);
            var performer = _mapper.Map<PerformerEntity, PerformerViewModel>(createdEntity);

            return CreatedAtRoute("GetPerformerById",
                new { performerId = performer.PerformerId },
                new Response<PerformerViewModel>(performer));
        }

        // GET api/performers?pageNumber=&pageSize=
        /// <summary>
        /// Get CCTV Performer list which ordered by year (most recent first)
        /// </summary>
        /// <returns>
        /// A enumerator indicate the collection of performers
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<PerformerViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPerformersAsync()
        {
            _logger.LogDebug("GetPerformersAsync");
            await Task.Delay(0);
            var data = Enumerable.Empty<PerformerViewModel>();
            return Ok(new Response<IEnumerable<PerformerViewModel>>(data));
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
            var performerEntity = await _performerRepository.ReadEntityAsync(performerId, CancellationToken.None);
            var performer = _mapper.Map<PerformerViewModel>(performerEntity);
            return Ok(new Response<PerformerViewModel>(performer));
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

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var performerEntity = _mapper.Map<PerformerEntity>(performerRequest.Data);
            var createdEntity = await _performerRepository.UpdateEntityAsync(performerId, performerEntity, CancellationToken.None);
            var performer = _mapper.Map<PerformerViewModel>(createdEntity);

            return Ok(new Response<PerformerViewModel>(performer));
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
            _logger.LogDebug("DeletePerformerByIdAsync");

            var performerEntity = await _performerRepository.DeleteEntityAsync(performerId, hardDelete, CancellationToken.None);
            var performer = _mapper.Map<PerformerViewModel>(performerEntity);
            return Ok(new Response<PerformerViewModel>(performer));
        }
    }
}