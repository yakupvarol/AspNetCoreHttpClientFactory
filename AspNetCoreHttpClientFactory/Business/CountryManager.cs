using AspNetCoreHttpClientFactory.Core.Client.Base;
using AspNetCoreHttpClientFactory.Core.Client.DTO;
using AspNetCoreHttpClientFactory.DTO;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using static HttpClientFactoryEnum;

namespace AspNetCoreHttpClientFactory.Business
{
    public class CountryManager : ICountryService
    {
        private readonly IHttpClientFactoryBase _httpClient;
        private readonly ILogger<ICountryService> _log;

        public CountryManager(IHttpClientFactoryBase httpClient, ILogger<ICountryService> log)
        {
            _httpClient = httpClient;
            _log = log;
        }

        public async Task<IList<CountryDTO.Response>> BaseList(CountryDTO.Request src)
        {
            return await _httpClient.PostAsync<IList<CountryDTO.Response>>(new HttpClientFactoryDTO.PostAsync { url = $"{ConfigHelp.WebApiURL}Country/List", request = src });
        }

        public async Task<IList<CountryDTO.Response>> NamedClientsList(CountryDTO.Request src)
        {
            return await _httpClient.PostAsync<IList<CountryDTO.Response>>(new HttpClientFactoryDTO.PostAsync { url = $"Country/List", namedclient= namedClients.FarmerExpert, request = src });
        }
    }
}
