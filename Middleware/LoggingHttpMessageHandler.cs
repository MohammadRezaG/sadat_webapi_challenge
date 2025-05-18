using System.Diagnostics;

namespace Sadas_test.Middleware
{
    public class LoggingHttpMessageHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingHttpMessageHandler> _logger;

        public LoggingHttpMessageHandler(ILogger<LoggingHttpMessageHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("[HttpClient] Sending request to {url}", request.RequestUri);

            var response = await base.SendAsync(request, cancellationToken);
            sw.Stop();

            _logger.LogInformation("[HttpClient] {method} {url} returned {status} in {elapsed}ms",
                request.Method,
                request.RequestUri,
                response.StatusCode,
                sw.ElapsedMilliseconds);

            return response;
        }
    }
}
