using Microsoft.AspNetCore.Mvc;
using Web_API_Versioning.API.Models.DTOs;

namespace Web_API_Versioning.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class CountriesController : ControllerBase
    {
        [MapToApiVersion("1.0")]
        [HttpGet]
        public IActionResult GetV1()
        {
            var countriesDomainModel = CountriesData.Get();

            // Map Domain Model to DTO
            var response = countriesDomainModel.Select(countryDomainModel => new CountryDtoV1
            {
                Id = countryDomainModel.Id,
                Name = countryDomainModel.Name,
            });

            return Ok(response);
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        public IActionResult GetV2()
        {
            var countriesDomainModel = CountriesData.Get();

            // Map Domain Model to DTO
            var response = countriesDomainModel.Select(countryDomainModel => new CountryDtoV2
            {
                Id = countryDomainModel.Id,
                CountryName = countryDomainModel.Name,
            });

            return Ok(response);
        }
    }
}