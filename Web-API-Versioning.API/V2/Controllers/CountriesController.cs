using Microsoft.AspNetCore.Mvc;
using Web_API_Versioning.API.Models.DTOs;

namespace Web_API_Versioning.API.V2.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class CountriesController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
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