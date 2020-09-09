using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using static HttpClientFactoryEnum;

namespace AspNetCoreHttpClientFactory.Core.Client.DTO
{
    public class HttpClientFactoryDTO
    {
        public class GetAsync
        {
            public GetAsync()
            { 
                httpverb = httpVerb.Get;
                namedclient = namedClients.Default;
            }
            public string url { get; set; }
            public string authToken { get; set; }
            public object parameter { get; set; }
            public string headapikey { get; set; }
            public httpVerb httpverb { get; set; }
            public namedClients namedclient { get; set; }
            
        }

        public class PostAsync
        {
            public PostAsync()
            { 
                httpverb = httpVerb.Post;
                namedclient = namedClients.Default;
            }
            public string url { get; set; }
            public string authToken { get; set; }
            public object request { get; set; }
            public object parameter { get; set; }
            public IFormFile file { get; set; }
            public IList<IFormFile> files { get; set; }
            public httpVerb httpverb { get; set; }
            public namedClients namedclient { get; set; }
        }

        public class SendAsync
        {
            public SendAsync()
            { 
                httpmethod = httpMethod.Get;
                namedclient = namedClients.Default;
            }
            public string url { get; set; }
            public string authToken { get; set; }
            public string headapikey { get; set; }
            public httpMethod httpmethod { get; set; }
            public namedClients namedclient { get; set; }
        }
    }
}
