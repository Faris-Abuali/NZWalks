using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        //[Route("All", Name = "GetAllRegions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Region[]> GetAll()
        {
            // Get data from DB (Region Domain Model)
            var regions = dbContext.Regions.ToList();

            // Map domain model to DTO
            var regionsDto = new List<RegionDto>();

            foreach (var region in regions)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    ImageUrl = region.ImageUrl,
                });
            }

            // --- Or Using LINQ
            //var regionsDto = regions.Select(r => new RegionDto
            //{
            //    Id = r.Id,
            //    Name = r.Name,
            //    Code = r.Code,
            //    ImageUrl = r.ImageUrl,
            //});

            // Return DTO
            return Ok(regionsDto);
        }

        // GET SINGLE REGION BY ID
        [HttpGet]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Region> GetById([FromRoute] Guid id)
        {
            // Get region domain model from DB
            var region = dbContext.Regions.Find(id); // find by PK

            // var region = dbContext.Regions.FirstOrDefault(r => r.Id == id); //find by any property

            if (region == null)
            {
                return NotFound();
            }

            // Map the region domain model object to RegionDto
            var regionDto = new RegionDto
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                ImageUrl = region.ImageUrl,
            };

            return Ok(regionDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Region> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Convert the DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                ImageUrl = addRegionRequestDto.ImageUrl,
            };


            // Use the Domain Model to create Region record
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            // Convert domain model back to DTO to send it back to the client
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                ImageUrl = regionDomainModel.ImageUrl,
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<RegionDto> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Get region domain model from DB
            var regionDomainModel = dbContext.Regions.Find(id); // find by PK

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Since `regionDomainModel` is tracked by dbContext, so we can directly update the `regionDomainModel`
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.ImageUrl = updateRegionRequestDto.ImageUrl;

            // because this entity is already tracked by dbContext, we don't have to call the Update method again.
            // dbContext.Regions.Update(regionDomainModel); // No need

            dbContext.SaveChanges();

            // Convert domain model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                ImageUrl = regionDomainModel.ImageUrl,
            };

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<RegionDto> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = dbContext.Regions.Find(id); // find by PK

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges();

            // Optional: Return back the delete region DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                ImageUrl = regionDomainModel.ImageUrl,
            };

            return Ok(regionDto);
        }
    }
}
