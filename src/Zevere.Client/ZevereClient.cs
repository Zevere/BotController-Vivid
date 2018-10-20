using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Zevere.Client.GraphQL;

namespace Zevere.Client
{
    /// <inheritdoc />
    public class ZevereClient : IZevereClient
    {
        private readonly HttpClient _client;

        private readonly ILogger _logger;

        /// <inheritdoc />
        public ZevereClient(
            string endpointUrl,
            ILogger logger = default
        )
            : this(logger)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(endpointUrl)
            };
        }

        /// <inheritdoc />
        public ZevereClient(
            HttpClient client,
            ILogger logger = default
        )
            : this(logger)
        {
            _client = client;
        }

        private ZevereClient(
            ILogger logger = default
        )
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<Response> MakeRequestAsync(
            Request request,
            CancellationToken cancellationToken = default
        )
        {
            string reqBody = JsonConvert.SerializeObject(request);

            _logger?.LogDebug("Making request {0}", reqBody);

            var response = await _client.PostAsync("",
                    new StringContent(reqBody, Encoding.UTF8, "application/json"),
                    cancellationToken
                )
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogError(
                    "Unexpected HTTP response with status code {0} and headers {1}.",
                    response.StatusCode,
                    response.Headers
                );
                response.EnsureSuccessStatusCode();
            }

            string respBody = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            _logger?.LogDebug("Received response {0}", respBody);

            var resp = JsonConvert.DeserializeObject<Response>(respBody);

            return resp;
        }
    }
}
