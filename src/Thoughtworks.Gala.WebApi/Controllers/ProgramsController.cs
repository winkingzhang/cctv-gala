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
    /// Represent group of apis for galas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class ProgramsController : ControllerBase
    {
        private readonly IProgramRepository _programRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProgramsController> _logger;

        public ProgramsController(
            IProgramRepository programRepository,
            IMapper mapper,
            ILogger<ProgramsController> logger)
        {
            _programRepository = programRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // POST api/programs
        [HttpPost]
        [ProducesResponseType(typeof(Response<ProgramViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse.BadRequest), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProgramAsync(
            [FromBody] Request<ProgramViewModel.Creation> programRequest,
            CancellationToken token = default)
        {
            _logger.LogDebug("CreateProgramAsync");

            if (programRequest.Data is null)
            {
                ModelState.AddModelError(nameof(programRequest.Data), "no request data");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var programEntity = _mapper.Map<ProgramViewModel.Creation, ProgramEntity>(programRequest.Data!);
            var createdEntity = await _programRepository.CreateProgramEntityAsync(
                programEntity,
                token);
            var program = _mapper.Map<ProgramEntity, ProgramViewModel>(createdEntity);

            return CreatedAtRoute("GetProgramById",
                new {programId = program.ProgramId},
                new Response<ProgramViewModel>(program));
        }

        // GET api/programs?programId=030DB850-89B5-4CCD-A389-61EE3EAE1CBD
        /// <summary>
        /// Get CCTV Gala Program list which ordered by year (most recent first)
        /// </summary>
        /// <returns>
        /// A enumerator indicate the collection of CCTV Gala Program
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IList<ProgramViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProgramsByIdsAsync(
            [FromQuery] Guid[] programIds,
            CancellationToken token = default)
        {
            _logger.LogDebug("GetProgramsByIdsAsync");

            if (!programIds.Any())
            {
                ModelState.AddModelError(string.Empty, $"{nameof(programIds)} is null or empty");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var programEntities = await _programRepository.GetProgramEntityListByIdsAsync(
                programIds,
                token);

            return Ok(new Response<IList<ProgramViewModel>>(programEntities
                .Select(entity => _mapper.Map<ProgramViewModel>(entity))
                .ToList()));
        }

        // // GET api/programs/gala/{galaId}
        // /// <summary>
        // /// Get CCTV Gala Program list which ordered by gala
        // /// </summary>
        // /// <returns>
        // /// A enumerator indicate the collection of CCTV Gala Program
        // /// </returns>
        // [HttpGet("gala/{galaId}")]
        // [ProducesResponseType(typeof(Response<IList<ProgramViewModel>>), StatusCodes.Status200OK)]
        // public async Task<IActionResult> GetProgramsByGalaIdAsync([FromQuery] Guid galaId, CancellationToken token = default)
        // {
        //     _logger.LogDebug("GetProgramsByIdsAsync");
        //
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(new ErrorResponse.BadRequest(ModelState));
        //     }
        //
        //     var programEntities = await _programRepository.GetProgramEntityListByGalaIdAsync(
        //         galaId,
        //         token);
        //
        //     return Ok(new Response<IList<ProgramViewModel>>(programEntities
        //         .Select(entity => _mapper.Map<ProgramViewModel>(entity))
        //         .ToList()));
        // }

        // GET api/programs/{programId}
        [HttpGet("{programId}", Name = "GetProgramById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<ProgramViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProgramByIdAsync(
            [FromRoute] Guid programId,
            CancellationToken token = default)
        {
            _logger.LogDebug("GetProgramByIdAsync");

            var programEntity = await _programRepository.ReadEntityAsync(
                programId,
                token);

            if (programEntity is null)
            {
                return NotFound();
            }

            var program = _mapper.Map<ProgramViewModel>(programEntity);
            return Ok(new Response<ProgramViewModel>(program));
        }

        // PUT api/programs/{programId}
        [HttpPut("{programId}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<ProgramViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse.BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditProgramByIdAsync(
            [FromRoute] Guid programId,
            [FromBody] Request<ProgramViewModel.Edit> programRequest,
            CancellationToken token = default)
        {
            _logger.LogDebug("EditProgramByIdAsync");

            if (programRequest.Data is null)
            {
                ModelState.AddModelError(nameof(programRequest.Data), "no request data");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var programEntity = _mapper.Map<ProgramEntity>(programRequest.Data);
            var createdEntity = await _programRepository.UpdateEntityAsync(
                programId,
                programEntity,
                token);
            var program = _mapper.Map<ProgramViewModel>(createdEntity);

            return Ok(new Response<ProgramViewModel>(program));
        }

        // DELETE api/programs/{programId}?hardDelete=true|false
        [HttpDelete("{programId}")]
        [ProducesResponseType(typeof(Response<ProgramViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProgramByIdAsync(
            [FromRoute] Guid programId,
            bool hardDelete = false,
            CancellationToken token = default)
        {
            _logger.LogDebug("EditProgramByIdAsync");

            var programEntity = hardDelete
                ? await _programRepository.DeleteProgramEntityByIdAsync(programId, token)
                : await _programRepository.MarkProgramEntityAsDeletedAsync(programId, token);

            var program = _mapper.Map<ProgramViewModel>(programEntity);

            return Ok(new Response<ProgramViewModel>(program));
        }
    }
}