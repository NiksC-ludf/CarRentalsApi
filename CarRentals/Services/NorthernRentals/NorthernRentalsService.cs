namespace CarRentals.Services.NorthernRentals
{
    using System.Text.Json;

    /// <summary>
    /// Northern Rentals service logic.
    /// </summary>
    public class NorthernRentalsService : INorthernRentalsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NorthernRentalsService> _logger;
        private readonly string _requestUrl;

        public NorthernRentalsService(HttpClient httpClient,
            ILogger<NorthernRentalsService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _requestUrl = configuration["NorthernRentals:RequestUrl"] ?? throw new ArgumentNullException(nameof(configuration), "Request URL is not configured.");

            if (string.IsNullOrWhiteSpace(_requestUrl))
            {
                throw new ArgumentException("NorthernRentals:RequestUrl cannot be null or empty.", nameof(configuration));
            }
        }

        /// <summary>
        /// Get available cars from Northern Rentals.
        /// </summary>
        public async Task<IEnumerable<NorthernRentalsOfferDto>> GetAvailableCars()
        {
            try
            {
                var response = await _httpClient.GetAsync(_requestUrl).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var carOffers = JsonSerializer.Deserialize<IEnumerable<NorthernRentalsOfferDto>>(responseString);

                if (carOffers == null || !carOffers.Any())
                {
                    _logger.LogWarning("No available cars were found.");
                    return Enumerable.Empty<NorthernRentalsOfferDto>();
                }

                return carOffers;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP request error while fetching available cars from {RequestUrl}.", _requestUrl);
                throw;
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON deserialization error while processing available cars from {RequestUrl}.", _requestUrl);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching available cars from {RequestUrl}.", _requestUrl);
                throw;
            }
        }
    }
}
