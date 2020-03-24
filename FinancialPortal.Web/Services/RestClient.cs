using FinancialPortal.Web.Proxy;
using FinancialPortal.Web.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services
{
    public class RestClient : IRestClient
    {

        private readonly HttpClient _client;
        private readonly ILogger<RestClient> _logger;
        private readonly JsonSerializerSettings _settings;

        public RestClient(
            ILogger<RestClient> logger,
            HttpClient client)
        {
            _logger = logger;
            _client = client;
            _client.DefaultRequestHeaders.Add("cache-control", "private, max-age=0, no-cache");

            _settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            _logger.LogDebug("Initialised");
        }

        public void Dispose()
        {
            _logger.LogDebug("Disposing RestClient along with HttpClient generated from the HttpClient Factory.");

            _client?.Dispose();
        }

        /// <summary>
        ///     Uses HTTP GET to fetch JSON response from provided endpoint.
        /// </summary>
        /// <param name="uri">Endpoint to fetch JSON response from</param>
        /// <returns>Response JSON deserialised to type T using Newtonsoft.Json libaries</returns>
        public async Task<TResponse> GetAsync<TResponse>(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException(nameof(uri), "No URI provided");
            }

            try
            {
                var uriObj = new Uri(uri);

                var sw = Stopwatch.StartNew();
                using (var response = await _client.GetAsync(uriObj))
                {
                    sw.Stop();

                    _logger.LogInformation($"Data from URI {uri} took {sw.Elapsed.TotalMilliseconds}ms.");

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new RestException(response.StatusCode, await response.Content.ReadAsStringAsync());
                    }

                    using (var content = response.Content)
                    {
                        var contentType = content.Headers.ContentType.MediaType;

                        switch (contentType.ToLower())
                        {
                            case "application/json":
                                return JsonConvert.DeserializeObject<TResponse>(await content.ReadAsStringAsync(),
                                    _settings);
                            default:
                                throw new InvalidOperationException($"Content type {contentType} is not supported!");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error has occurred while fetching data from {uri}.");
                throw;
            }
        }

        /// <summary>
        ///     Uses HTTP POST to fetch JSON response from provided endpoint.
        /// </summary>
        /// <param name="uri">Endpoint to fetch JSON response from</param>
        /// <param name="request">HTTP POST body serialised as JSON</param>
        /// <returns>Response JSON deserialised to type T using Newtonsoft.Json libaries</returns>
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest request)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException(nameof(uri), "No URI provided");
            }

            try
            {
                var uriObj = new Uri(uri);

                var jsonContent = JsonConvert.SerializeObject(request, _settings);
#if DEBUG
                _logger.LogDebug($"POST Body: {jsonContent}");
#endif
                var body = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var sw = Stopwatch.StartNew();
                using (var response = await _client.PostAsync(uriObj, body))
                {
                    sw.Stop();

                    _logger.LogInformation($"Data from URI {uri} took {sw.Elapsed.TotalMilliseconds}ms.");

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new RestException(response.StatusCode, await response.Content.ReadAsStringAsync());
                    }

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    using (var content = response.Content)
                    {
                        var contentType = content.Headers.ContentType.MediaType;

                        switch (contentType.ToLower())
                        {
                            case "application/json":
                                return JsonConvert.DeserializeObject<TResponse>(await content.ReadAsStringAsync(),
                                    _settings);
                            default:
                                throw new InvalidOperationException($"Content type {contentType} is not supported!");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error has occurred while POSTing data from {uri}.");
                throw;
            }
        }

        /// <summary>
        ///     Uses HTTP POST to fetch JSON response from provided endpoint.
        /// </summary>
        /// <param name="uri">Endpoint to fetch JSON response from</param>
        /// <param name="request">HTTP POST body serialised as JSON</param>
        /// <returns>None. Task to await if required.</returns>
        public async Task PostNoResponseAsync<TRequest>(string uri, TRequest request)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException(nameof(uri), "No URI provided");
            }

            try
            {
                var uriObj = new Uri(uri);

                var jsonContent = request == null ? "{}" : JsonConvert.SerializeObject(request, _settings);
#if DEBUG
                _logger.LogDebug($"POST Body: {jsonContent}");
#endif
                var body = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var sw = Stopwatch.StartNew();
                using (var response = await _client.PostAsync(uriObj, body))
                {
                    sw.Stop();

                    _logger.LogInformation($"Data from URI {uri} took {sw.Elapsed.TotalMilliseconds}ms.");

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new RestException(response.StatusCode, await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error has occurred while POSTing data from {uri}.");
                throw;
            }
        }

        public async Task<Stream> PostStreamAsync<TRequest>(string uri, TRequest request)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException(nameof(uri), "No URI provided");
            }

            try
            {
                var uriObj = new Uri(uri);

                var jsonContent = JsonConvert.SerializeObject(request, _settings);
#if DEBUG
                _logger.LogDebug($"POST Body: {jsonContent}");
#endif
                var body = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var sw = Stopwatch.StartNew();
                var response = await _client.PostAsync(uriObj, body);

                sw.Stop();

                _logger.LogInformation($"Data from URI {uri} took {sw.Elapsed.TotalMilliseconds}ms.");

                if (!response.IsSuccessStatusCode)
                {
                    throw new RestException(response.StatusCode, await response.Content.ReadAsStringAsync());
                }

                return await response.Content.ReadAsStreamAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error has occurred while POSTing data from {uri}.");
                throw;
            }
        }
    }
}