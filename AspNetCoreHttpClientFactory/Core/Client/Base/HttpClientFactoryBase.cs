using AspNetCoreHttpClientFactory.Core.Client.DTO;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static HttpClientFactoryEnum;

namespace AspNetCoreHttpClientFactory.Core.Client.Base
{
    public class HttpClientFactoryBase : IHttpClientFactoryBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpClientFactoryBase(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetAsync<T>(HttpClientFactoryDTO.GetAsync src)
        {
            HttpClient client = null;

            if (src.namedclient == namedClients.Default)
            { client = _httpClientFactory.CreateClient(); }
            else
            { client = _httpClientFactory.CreateClient(src.namedclient.ToString()); }
            //

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //

            if (src.authToken != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", src.authToken);
            //

            if (src.headapikey != null)
                client.DefaultRequestHeaders.Add("apikey", src.headapikey);
            //

            client.BaseAddress = new Uri(src.url);
            //

            HttpResponseMessage response = await client.GetAsync(GenerateQueryString(src.parameter)); ;
            if (src.httpverb == httpVerb.Get)
            { response = await client.GetAsync(GenerateQueryString(src.parameter)); }
            else if (src.httpverb == httpVerb.Delete)
            { response = await client.DeleteAsync(GenerateQueryString(src.parameter)); }
            //

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(data);
            }
            else
            {
                try
                {
                    var errorData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(errorData);
                }
                catch
                {
                    return default(T);
                }
            }
        }

        public async Task<T> PostAsync<T>(HttpClientFactoryDTO.PostAsync dt)
        {
            HttpClient client = null;

            if (dt.namedclient == namedClients.Default)
            { client = _httpClientFactory.CreateClient(); }
            else
            { client = _httpClientFactory.CreateClient(dt.namedclient.ToString()); }
            //

            client.DefaultRequestHeaders.Accept.Clear();
            //

            if (dt.authToken != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", dt.authToken);

            var requestUrl = dt.url;
            if (dt.parameter != null)
                requestUrl = requestUrl + GenerateQueryString(dt.parameter);
            //

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(dt.request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            if (dt.httpverb == httpVerb.Post)
            { response = await client.PostAsync(requestUrl, httpContent); }
            else if (dt.httpverb == httpVerb.Put)
            { response = await client.PutAsync(requestUrl, httpContent); }
            else if (dt.httpverb == httpVerb.Patch)
            { response = await client.PatchAsync(requestUrl, httpContent); }
            //

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(data);
            }
            else
            {
                try
                {
                    var errorData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(errorData);
                }
                catch
                {
                    return default(T);
                }
            }

        }

        public async Task<T> SendAsync<T>(HttpClientFactoryDTO.SendAsync src)
        {
            HttpClient client = null;

            if (src.namedclient == namedClients.Default)
            { client = _httpClientFactory.CreateClient(); }
            else
            { client = _httpClientFactory.CreateClient(src.namedclient.ToString()); }
            //

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //

            if (src.authToken != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", src.authToken);
            //

            HttpMethod httpmethod = null;
            if (src.httpmethod == httpMethod.Get)
            { httpmethod = HttpMethod.Get; }
            else if (src.httpmethod == httpMethod.Post)
            { httpmethod = HttpMethod.Get; }
            else if (src.httpmethod == httpMethod.Put)
            { httpmethod = HttpMethod.Put; }
            else if (src.httpmethod == httpMethod.Patch)
            { httpmethod = HttpMethod.Patch; }
            else if (src.httpmethod == httpMethod.Delete)
            { httpmethod = HttpMethod.Delete; }
            //

            var request = new HttpRequestMessage(httpmethod, src.url);
            var response = await client.SendAsync(request);
            if (src.headapikey != null)
                request.Headers.Add("apikey", src.headapikey);
            //

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(data);
            }
            else
            {
                try
                {
                    var errorData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(errorData);
                }
                catch
                {
                    return default(T);
                }
            }
        }

        public string GenerateQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
            return "?" + String.Join("&", (properties.ToArray()));
        }

        public void AddHeaders()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Remove("userIP");
                client.DefaultRequestHeaders.Add("userIP", "192.168.1.1");
            }
        }

    }
}
