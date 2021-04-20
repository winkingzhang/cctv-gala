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
        private readonly IGalaRepository _galaRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GalasController> _logger;

        public GalasController(
            IGalaRepository galaRepository,
            IMapper mapper,
            ILogger<GalasController> logger)
        {
            _galaRepository = galaRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // POST api/galas
        /// <summary>
        ///  Create a gala with specified properties by <paramref name="galaRequest"/>
        /// </summary>
        /// <param name="galaRequest">A request in which wraps a <see cref="GalaViewModel.Creation"/> </param>
        /// <param name="token">cancellation token, will be automatically injected from <seealso cref="Microsoft.Extensions.Hosting.IHostApplicationLifetime.ApplicationStopping"/></param>
        /// <returns>
        ///  A response object in which wraps a <see cref="GalaViewModel"/> if status goes to 201, and Location in HTTP Header goes URI for new created resource
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(Response<GalaViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse.BadRequest), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateGalaAsync(
            [FromBody] Request<GalaViewModel.Creation> galaRequest,
            CancellationToken token = default)
        {
            _logger.LogDebug("CreateGalaAsync");

            if (galaRequest.Data is null)
            {
                ModelState.AddModelError(nameof(galaRequest.Data), "no gala data");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var galaEntity = _mapper.Map<GalaViewModel.Creation, GalaEntity>(galaRequest.Data!);
            var createdEntity = await _galaRepository.CreateGalaEntityAsync(
                galaEntity,
                token);
            var gala = _mapper.Map<GalaEntity, GalaViewModel>(createdEntity);

            return CreatedAtRoute("GetGalaById",
                new {galaId = gala.GalaId},
                new Response<GalaViewModel>(gala));
        }

        // GET api/galas?galaIds=CCE858B8-680B-47A8-8B70-DA73DE811643&galaIds=BA8E245C-614A-487B-8345-856566908886
        /// <summary>
        /// Get CCTV Gala list by provided id array
        /// </summary>
        /// <param name="galaIds">the id array</param>
        /// <param name="token">cancellation token, will be automatically injected from <seealso cref="Microsoft.Extensions.Hosting.IHostApplicationLifetime.ApplicationStopping"/></param>
        /// <returns>
        /// An enumerator indicate the collection of CCTV Galas.
        /// If <paramref name="galaIds" /> is null or empty, response will be <see cref="BadRequestObjectResult"/>
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IList<GalaViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse.BadRequest), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetGalasByIdsAsync(
            [FromQuery] Guid[] galaIds,
            CancellationToken token = default)
        {
            _logger.LogDebug("GetGalasByIdsAsync");
            if (!galaIds.Any())
            {
                ModelState.AddModelError(nameof(galaIds), $"{nameof(galaIds)} is null or empty");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var galasEntities = await _galaRepository.GetGalaEntityListByIdsAsync(
                galaIds,
                token);

            return Ok(new Response<IList<GalaViewModel>>(galasEntities
                .Select(galaEntity => _mapper.Map<GalaViewModel>(galaEntity))
                .ToList()));
        }

        // GET api/galas/years?years=2020&years=2021
        /// <summary>
        /// Get CCTV Gala list by year
        /// </summary>
        /// <param name="years"></param>
        /// <param name="token"></param>
        /// <returns>
        /// A enumerator indicate the collection of CCTV Galas
        /// </returns>
        [HttpGet("years/{years}")]
        [ProducesResponseType(typeof(Response<IList<GalaViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse.BadRequest), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetGalasByYearsAsync(
            [FromRoute] int[] years,
            CancellationToken token = default)
        {
            _logger.LogDebug("GetGalasByYearsAsync");

            if (!years.Any())
            {
                ModelState.AddModelError(nameof(years), $"{nameof(years)} is null or empty");
            }
            else if (years.Any(year => year < 1982 || year > DateTime.Now.Year))
            {
                ModelState.AddModelError(string.Empty,
                    $"some of {nameof(years)} are out of range of 1982 to this year");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var galasEntities = await _galaRepository.GetGalaEntityListByYearsAsync(
                years,
                token);

            return Ok(new Response<IList<GalaViewModel>>(galasEntities
                .Select(galaEntity => _mapper.Map<GalaViewModel>(galaEntity))
                .ToList()));
        }


        // GET api/galas/zodiacs/{zodiac}
        /// <summary>
        /// Get CCTV Gala list by zodiac, for example, "Ox" should same to query with years [1985,1997,2009,2021]
        /// </summary>
        /// <returns>
        /// A enumerator indicate the collection of CCTV Galas
        /// </returns>
        [HttpGet("zodiacs/{zodiac}")]
        [ProducesResponseType(typeof(Response<IList<GalaViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse.BadRequest), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetGalasByZodiacAsync(
            [FromRoute] ChineseZodiac zodiac,
            CancellationToken token = default)
        {
            _logger.LogDebug("GetGalasByZodiacAsync");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var galasEntities = await _galaRepository.GetGalaEntityListByZodiacAsync(
                zodiac,
                token);

            return Ok(new Response<IList<GalaViewModel>>(galasEntities
                .Select(galaEntity => _mapper.Map<GalaViewModel>(galaEntity))
                .ToList()));
        }

        // GET api/galas/{galaId}
        /// <summary>
        /// Get CCTV Gala by galaId
        /// </summary>
        /// <param name="galaId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("{galaId}", Name = "GetGalaById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<GalaViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGalaByIdAsync(
            [FromRoute] Guid galaId,
            CancellationToken token = default)
        {
            _logger.LogDebug("GetGalaByIdAsync");

            var galaEntity = await _galaRepository.GetGalaEntityByIdAsync(
                galaId,
                token);

            if (galaEntity is null)
            {
                return NotFound();
            }

            var gala = _mapper.Map<GalaViewModel>(galaEntity);
            return Ok(new Response<GalaViewModel>(gala));
        }

        // PUT api/galas/{galaId}
        [HttpPut("{galaId}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<GalaViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse.BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditGalaByIdAsync(
            [FromRoute] Guid galaId,
            [FromBody] Request<GalaViewModel.Edit> galaRequest,
            CancellationToken token = default)
        {
            _logger.LogDebug("EditGalaByIdAsync");

            if (galaRequest.Data is null)
            {
                ModelState.AddModelError(nameof(galaRequest.Data), "no request data");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse.BadRequest(ModelState));
            }

            var galaEntity = _mapper.Map<GalaEntity>(galaRequest.Data);
            var createdEntity = await _galaRepository.UpdateGalaEntityByIdAsync(galaId,
                galaEntity,
                token);
            var gala = _mapper.Map<GalaViewModel>(createdEntity);

            return Ok(new Response<GalaViewModel>(gala));
        }

        // DELETE api/galas/{galaId}?hardDelete=true|false
        [HttpDelete("{galaId}")]
        [ProducesResponseType(typeof(Response<GalaViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteGalaByIdAsync(
            [FromRoute] Guid galaId,
            bool hardDelete = false,
            CancellationToken token = default)
        {
            _logger.LogDebug("DeleteGalaByIdAsync");
            var galaEntity = hardDelete
                ? await _galaRepository.DeleteGalaEntityByIdAsync(galaId, token)
                : await _galaRepository.MarkGalaEntityAsDeletedAsync(galaId, token);

            var gala = _mapper.Map<GalaViewModel>(galaEntity);
            return Ok(new Response<GalaViewModel>(gala));
        }
    }
}