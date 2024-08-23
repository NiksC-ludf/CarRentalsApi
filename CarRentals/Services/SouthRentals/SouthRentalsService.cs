namespace CarRentals.Services.SouthRentals
{
    using System.Text.Json;

    /// <summary>
    /// South Rentals service logic.
    /// </summary>
    public class SouthRentalsService : ISouthRentalsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SouthRentalsService> _logger;
        private readonly string _requestUrl;

        public SouthRentalsService(HttpClient httpClient,
            ILogger<SouthRentalsService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _requestUrl = configuration["SouthRentals:RequestUrl"] ?? throw new ArgumentNullException(nameof(configuration), "Request URL is not configured.");

            if (string.IsNullOrWhiteSpace(_requestUrl))
            {
                throw new ArgumentException("SouthRentals:RequestUrl cannot be null or empty.", nameof(configuration));
            }
        }

        /// <summary>
        /// Gets all available cars from South Rentals.
        /// </summary>
        public async Task<IEnumerable<SouthRentalsOfferDto>> GetAvailableCars()
        {
            try
            {
                var response = await _httpClient.GetAsync(_requestUrl).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var carOffers = JsonSerializer.Deserialize<IEnumerable<SouthRentalsOfferDto>>(responseString);

                if (carOffers == null || !carOffers.Any())
                {
                    _logger.LogWarning("No available cars were found.");
                    return Enumerable.Empty<SouthRentalsOfferDto>();
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
