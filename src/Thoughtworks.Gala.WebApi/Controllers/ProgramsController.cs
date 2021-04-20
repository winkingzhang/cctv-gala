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
        private readonly IRepository<Guid, ProgramEntity> _programRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProgramsController> _logger;

        public ProgramsController(
            IRepository<Guid, ProgramEntity> programRepository,
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
            [FromBody] Request<ProgramViewModel.Creation> programRequest
        )
        {
            _logger.LogDebug("CreateProgramAsync");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var programEntity = _mapper.Map<ProgramViewModel.Creation, ProgramEntity>(programRequest.Data);
            var createdEntity = await _programRepository.CreateEntityAsync(programEntity, CancellationToken.None);
            var program = _mapper.Map<ProgramEntity, ProgramViewModel>(createdEntity);

            return CreatedAtRoute("GetProgramById",
                new { programId = program.ProgramId },
                new Response<ProgramViewModel>(program));
        }

        // GET api/programs?pageNumber=&pageSize=&galaId=
        /// <summary>
        /// Get CCTV Gala Program list which ordered by year (most recent first)
        /// </summary>
        /// <returns>
        /// A enumerator indicate the collection of CCTV Gala Program
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<ProgramViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProgramsAsync([FromQuery] Guid? galaId)
        {
            _logger.LogDebug("GetProgramsAsync");
            await Task.Delay(0);
            var data = Enumerable.Empty<ProgramViewModel>();
            return Ok(new Response<IEnumerable<ProgramViewModel>>(data));
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
            var programEntity = await _programRepository.ReadEntityAsync(programId, CancellationToken.None);
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
            [FromBody] Request<ProgramViewModel.Edit> programRequest
        )
        {
            _logger.LogDebug("EditProgramByIdAsync");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var programEntity = _mapper.Map<ProgramEntity>(programRequest.Data);
            var createdEntity = await _programRepository.UpdateEntityAsync(programId, programEntity, CancellationToken.None);
            var program = _mapper.Map<ProgramViewModel>(createdEntity);

            return Ok(new Response<ProgramViewModel>(program));
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

            var programEntity = await _programRepository.DeleteEntityAsync(programId, hardDelete, CancellationToken.None);
            var program = _mapper.Map<ProgramViewModel>(programEntity);

            return Ok(new Response<ProgramViewModel>(program));
        }
    }
}