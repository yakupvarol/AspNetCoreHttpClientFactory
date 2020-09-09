using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspNetCoreHttpClientFactory.Models;
using AspNetCoreHttpClientFactory.DTO;
using AspNetCoreHttpClientFactory.Business;
using AspNetCoreHttpClientFactory.Core.TypedClients;

namespace AspNetCoreHttpClientFactory.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICountryService _countryService;
        private readonly IFarmerExpertHttpClient _farmerExpertHttpClient;

        public HomeController(ILogger<HomeController> logger, ICountryService countryService, IFarmerExpertHttpClient farmerExpertHttpClient)
        {
            _countryService = countryService;
            _farmerExpertHttpClient = farmerExpertHttpClient;
            _logger = logger;
        }

        //[RequestSizeLimit(40000000)]
        //[DisableRequestSizeLimit]
        public async Task<IActionResult> IndexAsync()
        {
            var resultCountryList = await _countryService.BaseList(new CountryDTO.Request { lngid = 1 });
            var resultCountryNamedClientList = await _countryService.NamedClientsList(new CountryDTO.Request { lngid = 1 });
            var resultFarmerExpertTypeClientList = await _farmerExpertHttpClient.TypeClientList(new CountryDTO.Request { lngid = 1 });
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
