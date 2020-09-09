using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHttpClientFactory.Helper.Delegating
{
    public class LogHandler : DelegatingHandler
    {
        private readonly ILogger<LogHandler> _logger;

        public LogHandler(ILogger<LogHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var content = await request.Content.ReadAsStringAsync();
            Debug.Write($"{Environment.NewLine} ApiLog Request Path: {request.RequestUri.AbsolutePath}");
            Debug.Write($"{Environment.NewLine} ApiLog Request Uri: {request.RequestUri.AbsoluteUri}");
            Debug.Write($"{Environment.NewLine} ApiLog Request Query: {request.RequestUri.Query}");
            Debug.Write($"{Environment.NewLine} ApiLog Request Form Data: {content}");

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                Debug.Write($"{Environment.NewLine} ApiLog Response Data: {responseContent}");
                Debug.Write($"{Environment.NewLine} ApiLog Response status code: {response.StatusCode}");

            }

            return response;
        }
    }
}
