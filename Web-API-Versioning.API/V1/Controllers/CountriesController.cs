using Microsoft.AspNetCore.Mvc;
using Web_API_Versioning.API.Models.DTOs;

namespace Web_API_Versioning.API.V1.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CountriesController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
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

    }
}