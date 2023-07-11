using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper; // AutoMapper between DTO and Domain objects
        private readonly ILogger<RegionsController> logger;
        private readonly JsonSerializerOptions jsonOptions;

        public RegionsController(NZWalksDbContext dbContext, 
            IRegionRepository regionRepository, 
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;

            this.jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }


        [HttpGet]
        //[Route("All", Name = "GetAllRegions")]
        //[Authorize(Roles = "Reader, Writer")]
        public async Task<ActionResult<List<Region>>> GetAll()
        {
            try
            {
                // Get data from DB (Region Domain Model)
                var regionsDomain = await regionRepository.GetAllAsync();
                // The main thread will not be blocked when the execution delays

                // Map domain models to DTOs
                return Ok(mapper.Map<List<RegionDto>>(regionsDomain)); // TDestination Map<TDestination>(object source);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        // GET SINGLE REGION BY ID
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader, Writer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Region>> GetById([FromRoute] Guid id)
        {
            // Get region domain model from DB
            var region = await regionRepository.GetByIdAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(region));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ValidateModel] // check the model before it even reaches to the action method
        //[Authorize(Roles = "Writer")]
        public async Task<ActionResult<Region>> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // No need to this check, it will return 400 without adding this check
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);

            //}

            // Convert the DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            // Use the Domain Model to create Region record
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            // Convert domain model back to DTO to send it back to the client
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ValidateModel] // check the model before it is even reaches to the action method
        //[Authorize(Roles = "Writer")]
        public async Task<ActionResult<RegionDto>> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Convert the updateRegionRequestDto to domain model to pass it to regionRepository.UpdateAsync:
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Convert domain model to DTO
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "Writer")]
        public async Task<ActionResult<RegionDto>> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Optional: Return back the delete region DTO
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
