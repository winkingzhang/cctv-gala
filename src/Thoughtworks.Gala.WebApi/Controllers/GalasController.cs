using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Thoughtworks.Gala.WebApi.Entities;
using Thoughtworks.Gala.WebApi.Pagination;
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
    public class GalasController : ControllerBase
    {
        private readonly IRepository<Guid, GalaEntity> _galaRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationUriService _paginationUriService;
        private readonly ILogger<GalasController> _logger;

        public GalasController(
            IRepository<Guid, GalaEntity> galaRepository,
            IMapper mapper,
            IPaginationUriService paginationUriService,
            ILogger<GalasController> logger)
        {
            _galaRepository = galaRepository;
            _mapper = mapper;
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

            var galaEntity = _mapper.Map<GalaViewModel.Creation, GalaEntity>(galaRequest.Data);
            var createdEntity = await _galaRepository.CreateEntityAsync(galaEntity, CancellationToken.None);
            var gala = _mapper.Map<GalaEntity, GalaViewModel>(createdEntity);

            return CreatedAtRoute("GetGalaById",
                new { galaId = gala.GalaId },
                new Response<GalaViewModel>(gala));
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
            var galaEntity = await _galaRepository.ReadEntityAsync(galaId, CancellationToken.None);
            var gala = _mapper.Map<GalaViewModel>(galaEntity);
            return Ok(new Response<GalaViewModel>(gala));
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

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var galaEntity = _mapper.Map<GalaEntity>(galaRequest.Data);
            var createdEntity = await _galaRepository.UpdateEntityAsync(galaId, galaEntity, CancellationToken.None);
            var gala = _mapper.Map<GalaViewModel>(createdEntity);

            return Ok(new Response<GalaViewModel>(gala));
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
            _logger.LogDebug("DeleteGalaByIdAsync");

            var galaEntity = await _galaRepository.DeleteEntityAsync(galaId, hardDelete, CancellationToken.None);
            var gala = _mapper.Map<GalaViewModel>(galaEntity);
            return Ok(new Response<GalaViewModel>(gala));
        }
    }
}