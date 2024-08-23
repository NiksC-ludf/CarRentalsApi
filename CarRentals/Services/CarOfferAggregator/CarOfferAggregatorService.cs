namespace CarRentals.Services.CarOfferAggregator
{
    using CarRentals.Mappers;
    using CarRentals.Models;
    using CarRentals.Services.BestRentals;
    using CarRentals.Services.NorthernRentals;
    using CarRentals.Services.SouthRentals;
    using CarRentals.Mappers;

    /// <summary>
    /// Car offer aggregator service logic.
    /// </summary>
    public class CarOfferAggregatorService : ICarOfferAggregatorService
    {
        private readonly IBestRentalsService _bestRentalsService;
        private readonly ISouthRentalsService _southRentalsService;
        private readonly INorthernRentalsService _northernRentalsService;
        private readonly ILogger<CarOfferAggregatorService> _logger;

        public CarOfferAggregatorService(
            IBestRentalsService bestRentalsService,
            ISouthRentalsService southRentalsService,
            INorthernRentalsService northernRentalsService,
            ILogger<CarOfferAggregatorService> logger)
        {
            _bestRentalsService = bestRentalsService;
            _southRentalsService = southRentalsService;
            _northernRentalsService = northernRentalsService;
            _logger = logger;
        }

        /// <summary>
        /// Fetches car offers from all services.
        /// </summary>
        public async Task<IEnumerable<CarOffer>> FetchOffersFromServices()
        {
            var tasks = new List<Task<IEnumerable<CarOffer>>>
            {
                GetBestRentalsOffers(),
                GetSouthRentalsOffers(),
                GetNorthernRentalsOffers()
            };

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            return results.SelectMany(result => result);
        }

        /// <summary>
        /// Gets mapped car offers from BestRentals service.
        /// </summary>
        private async Task<IEnumerable<CarOffer>> GetBestRentalsOffers()
        {
            try
            {
                var offers = await _bestRentalsService.GetAvailableCars().ConfigureAwait(false);
                return offers.Select(CarOfferMapper.ToCarOffer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching BestRentals offers.");
                return Enumerable.Empty<CarOffer>();
            }
        }

        /// <summary>
        /// Gets mapped car offers from SouthRentals service.
        /// </summary>
        private async Task<IEnumerable<CarOffer>> GetSouthRentalsOffers()
        {
            try
            {
                var offers = await _southRentalsService.GetAvailableCars().ConfigureAwait(false);
                return offers.Select(CarOfferMapper.ToCarOffer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching SouthRentals offers.");
                return Enumerable.Empty<CarOffer>();
            }
        }

        /// <summary>
        /// Gets mapped car offers from NorthernRentals service.
        /// </summary>
        private async Task<IEnumerable<CarOffer>> GetNorthernRentalsOffers()
        {
            try
            {
                var offers = await _northernRentalsService.GetAvailableCars().ConfigureAwait(false);
                return offers.Select(CarOfferMapper.ToCarOffer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching NorthernRentals offers.");
                return Enumerable.Empty<CarOffer>();
            }
        }
    }
}
