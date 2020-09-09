using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreHttpClientFactory.DTO;
using Newtonsoft.Json;

namespace AspNetCoreHttpClientFactory.Core.TypedClients
{
    public class FarmerExpertHttpClient : IFarmerExpertHttpClient
    {
        private readonly HttpClient _httpClient;

        public FarmerExpertHttpClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(ConfigHelp.WebApiURL);
            /*
            httpClient.DefaultRequestHeaders.Add("CustomHeaderKey", "It-is-a-HttpClientFactory-Sample");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
                NoStore = true,
                MaxAge = new TimeSpan(0),
                MustRevalidate = true
            };
            */
            _httpClient = httpClient;
        }

        public async Task<IList<CountryDTO.Response>> TypeClientList(CountryDTO.Request src)
        {
            var result = await _httpClient.PostAsync($"Country/List", new StringContent(JsonConvert.SerializeObject(new CountryDTO.Request { lngid = 1 }), Encoding.UTF8, "application/json"));
            var data = await result.Content.ReadAsStringAsync();
            return  JsonConvert.DeserializeObject<List<CountryDTO.Response>>(data);
        }
    }
}
