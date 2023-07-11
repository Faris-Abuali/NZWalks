using Microsoft.AspNetCore.Mvc;
using NzWalks.UI.Models.DTO;

namespace NzWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new(); // simplified version of: new List<RegionDto>()

            try
            {
                // Get all regions from Web API
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:8888/api/Regions");

                httpResponseMessage.EnsureSuccessStatusCode(); // will through exception if false

                var responseEnumerable = await httpResponseMessage.Content.ReadFromJsonAsync<List<RegionDto>>();
                
                if (responseEnumerable != null)
                {
                    response.AddRange(responseEnumerable);
                }
            }
            catch (Exception)
            {
                // Log the exception
            }

            return View(response);

        }
    }
}
