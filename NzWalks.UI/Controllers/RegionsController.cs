using Microsoft.AspNetCore.Mvc;
using NzWalks.UI.Models;
using NzWalks.UI.Models.DTO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NzWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
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

                if (responseEnumerable is not null)
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

        [HttpGet] // this should have the same name as the View file "Add.cshtml"
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:8888/api/Regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"),
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode(); // will through exception if false

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response is not null)
            {
                return RedirectToAction("Index", "Regions"); // actionName, controllerName
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:8888/api/Regions/{id.ToString()}");

            if (response is not null)
            {
                return View(response);
            }

            return View(null);
        }

        [HttpPost] // because the <form> in the view only works with GET and POST
        public async Task<IActionResult> Edit(RegionDto request)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:8888/api/Regions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"),
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode(); // will through exception if false

            // This will be the updated region coming in the response
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response is not null)
            {
                return RedirectToAction("Index", "Regions"); // actionName, controllerName
            }

            return View();
        }

        [HttpPost] // because the <form> in the view only works with GET and POST\
        public async Task<IActionResult> Delete(RegionDto request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:8888/api/Regions/{request.Id}");

                httpResponseMessage.EnsureSuccessStatusCode(); // will through exception if false

                // This will be the updated region coming in the response
                var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

                return RedirectToAction("Index", "Regions"); // actionName, controllerName

            }
            catch (Exception)
            {
                // console log the exception
            }

            return View("Edit"); // return to the Edit view page
        }
    }
}
