namespace CarRentals.Services.BestRentals
{
    using System.Text.Json;

    /// <summary>
    /// Best Rentals service logic.
    /// </summary>
    public class BestRentalsService : IBestRentalsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BestRentalsService> _logger;
        private readonly string _requestUrl;

        public BestRentalsService(HttpClient httpClient,
            ILogger<BestRentalsService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _requestUrl = configuration["BestRentals:RequestUrl"] ?? throw new ArgumentNullException(nameof(configuration), "Request URL is not configured.");

            if (string.IsNullOrWhiteSpace(_requestUrl))
            {
                throw new ArgumentException("BestRentals:RequestUrl cannot be null or empty.", nameof(configuration));
            }
        }

        /// <summary>
        /// Gets available cars from Best Rentals.
        /// </summary>
        public async Task<IEnumerable<BestRentalsOfferDto>> GetAvailableCars()
        {
            try
            {
                var response = await _httpClient.GetAsync(_requestUrl).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var carOffers = JsonSerializer.Deserialize<IEnumerable<BestRentalsOfferDto>>(responseString);

                if (carOffers == null || !carOffers.Any())
                {
                    _logger.LogWarning("No available cars were found.");
                    return Enumerable.Empty<BestRentalsOfferDto>();
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
