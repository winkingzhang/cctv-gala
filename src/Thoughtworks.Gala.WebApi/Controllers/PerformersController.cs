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
        private readonly IPerformerRepository _performerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PerformersController> _logger;

        public PerformersController(
            IPerformerRepository performerRepository,
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
            [FromBody] Request<PerformerViewModel.Creation> performerRequest,
            CancellationToken token = default)
        {
            _logger.LogDebug("CreatePerformerAsync");

            if (performerRequest.Data is null)
            {
                ModelState.AddModelError(nameof(performerRequest.Data), "no request data");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var performerEntity = _mapper.Map<PerformerViewModel.Creation, PerformerEntity>(performerRequest.Data!);
            var createdEntity = await _performerRepository.CreatePerformerEntityAsync(
                performerEntity,
                token);
            var performer = _mapper.Map<PerformerEntity, PerformerViewModel>(createdEntity);

            return CreatedAtRoute("GetPerformerById",
                new {performerId = performer.PerformerId},
                new Response<PerformerViewModel>(performer));
        }

        // GET api/performers?performerIds=6A5A11B6-B748-44AE-93A6-24452F5727DC&performerIds=CE223CB4-37C6-4711-AAA7-E94B1C5F446D
        /// <summary>
        /// Get CCTV Performer list by ids
        /// </summary>
        /// <returns>
        /// A enumerator indicate the collection of performers
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IList<PerformerViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse.BadRequest), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPerformersAsync(
            [FromQuery] Guid[] performerIds,
            CancellationToken token = default)
        {
            _logger.LogDebug("GetPerformersAsync");

            if (!performerIds.Any())
            {
                ModelState.AddModelError(nameof(performerIds), "empty performer ids");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var performerEntities = await _performerRepository.GetPerformerEntityListByIdsAsync(
                performerIds,
                token);

            return Ok(new Response<IList<PerformerViewModel>>(performerEntities
                .Select(entity => _mapper.Map<PerformerViewModel>(entity))
                .ToList()));
        }

        // GET api/performers/{performerId}
        [HttpGet("{performerId}", Name = "GetPerformerById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<PerformerViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPerformerByIdAsync(
            [FromRoute] Guid performerId,
            CancellationToken token = default)
        {
            _logger.LogDebug("GetPerformerByIdAsync");
            var performerEntity = await _performerRepository.GetPerformerEntityByIdAsync(
                performerId,
                token);
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
            [FromBody] Request<PerformerViewModel.Edit> performerRequest,
            CancellationToken token = default)
        {
            _logger.LogDebug("EditPerformerByIdAsync");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var performerEntity = _mapper.Map<PerformerEntity>(performerRequest.Data);
            var createdEntity = await _performerRepository.UpdatePerformerEntityByIdAsync(
                performerId,
                performerEntity,
                token);
            var performer = _mapper.Map<PerformerViewModel>(createdEntity);

            return Ok(new Response<PerformerViewModel>(performer));
        }

        // DELETE api/performers/{performerId}?hardDelete=true|false
        [HttpDelete("{performerId}")]
        [ProducesResponseType(typeof(Response<PerformerViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePerformerByIdAsync(
            [FromRoute] Guid performerId,
            bool hardDelete = false,
            CancellationToken token = default)
        {
            _logger.LogDebug("DeletePerformerByIdAsync");

            var performerEntity = hardDelete
                ? await _performerRepository.DeletePerformerEntityByIdAsync(performerId, token)
                : await _performerRepository.MarkPerformerEntityAsDeletedAsync(performerId, token);
            var performer = _mapper.Map<PerformerViewModel>(performerEntity);
            return Ok(new Response<PerformerViewModel>(performer));
        }
    }
}