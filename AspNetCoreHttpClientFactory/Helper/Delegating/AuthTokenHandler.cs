using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHttpClientFactory.Helper.Delegating
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly string _token;
        private const string TOKEN = "Authorization";
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(TOKEN) && string.IsNullOrEmpty(_token))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Missing auth token.")
                };
            }
            else if (!request.Headers.Contains(TOKEN) && !string.IsNullOrEmpty(_token))
            {
                request.Headers.Add(TOKEN, $"Bearer {_token}");
            }

            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
