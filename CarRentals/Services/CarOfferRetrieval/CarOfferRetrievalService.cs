namespace CarRentals.Services.CarOfferAggregator
{
    using CarRentals.Models;
    using CarRentals.Models.Enums;
    using CarRentals.Repositories;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Car offer aggregator service logic.
    /// </summary>
    public class CarOfferRetrievalService : ICarOfferRetrievalService
    {
        private readonly ICarOfferAggregatorService _carOfferAggregatorService;
        private readonly ILogger<CarOfferRetrievalService> _logger;
        private readonly IMemoryCache _cache;
        private readonly ICarOfferRepository _repository;
        private readonly string _cacheKey = "AggregatedCarOffers";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);

        /// <summary>
        /// Car offer aggregator service logic.
        /// </summary>
        public CarOfferRetrievalService(
            ICarOfferAggregatorService carOfferAggregatorService,
            ILogger<CarOfferRetrievalService> logger,
            IMemoryCache cache,
            ICarOfferRepository repository)
        {
            _carOfferAggregatorService = carOfferAggregatorService;
            _logger = logger;
            _cache = cache;
            _repository = repository;
        }

        /// <summary>
        /// Get all available cars from all services in one IEnumerable.
        /// </summary>
        /// <param name="minPrice">Minimum price to filter cars by.</param>
        /// <param name="maxPrice">Maximum price to filter cars by.</param>
        /// <param name="carCategoryType">Car category - SIPP code first letter. Type: char.</param>
        /// <param name="carBodyType">Car body type - SIPP code second letter. Type: char.</param>
        /// <param name="carDriveType">Car drive type - SIPP code third letter. Type: char.</param>
        /// <param name="carFuelAndAirConSystem">Car fuel and air con system - SIPP code forth letter. Type: char.</param>
        public async Task<IEnumerable<CarOffer>> GetAvailableCars(
            double? minPrice = null,
            double? maxPrice = null,
            char? carCategoryType = null,
            char? carBodyType = null,
            char? carDriveType = null,
            char? carFuelAndAirConSystem = null)
        {
            try
            {
                // Try to get offers from cache first
                if (_cache.TryGetValue(_cacheKey, out IEnumerable<CarOffer> cachedOffers))
                {
                    return ApplyFiltersAndSorting(cachedOffers, minPrice, maxPrice, carCategoryType, carBodyType, carDriveType, carFuelAndAirConSystem);
                }

                // If cache is empty or expired, fetch offers from this database
                var lastUpdateTime = await _repository.GetLastUpdateTime();
                if (lastUpdateTime.HasValue && (DateTime.UtcNow - lastUpdateTime.Value).TotalMinutes <= 30)
                {
                    var dbOffers = await _repository.GetCarOffers();
                    if (dbOffers != null && dbOffers.Any())
                    {
                        _cache.Set(_cacheKey, dbOffers, _cacheExpiration);
                        return ApplyFiltersAndSorting(dbOffers, minPrice, maxPrice, carCategoryType, carBodyType, carDriveType, carFuelAndAirConSystem);
                    }
                }

                // If database is empty or expired, fetch offers from services
                var aggregatedResults = await _carOfferAggregatorService.FetchOffersFromServices();
                if (aggregatedResults == null || !aggregatedResults.Any())
                {
                    _logger.LogError("No car offers available from services.");
                    return Enumerable.Empty<CarOffer>();
                }

                await _repository.UpdateCarOffers(aggregatedResults);

                _cache.Set(_cacheKey, aggregatedResults, _cacheExpiration);

                return ApplyFiltersAndSorting(aggregatedResults, minPrice, maxPrice, carCategoryType, carBodyType, carDriveType, carFuelAndAirConSystem);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while aggregating car offers.");
                throw;
            }
        }

        /// <summary>
        /// Filters and sorts car offers by optional parameters and by price and supplier name.
        /// </summary>
        /// <param name="offers">Car offers to sort.</param>
        /// <param name="minPrice">Minimum price to filter cars by.</param>
        /// <param name="maxPrice">Maximum price to filter cars by.</param>
        /// <param name="carCategoryType">Car category - SIPP code first letter. Type: char.</param>
        /// <param name="carBodyType">Car body type - SIPP code second letter. Type: char.</param>
        /// <param name="carDriveType">Car drive type - SIPP code third letter. Type: char.</param>
        /// <param name="carFuelAndAirConSystem">Car fuel and air con system - SIPP code forth letter. Type: char.</param>
        private IEnumerable<CarOffer> ApplyFiltersAndSorting(
        IEnumerable<CarOffer> offers,
        double? minPrice,
        double? maxPrice,
        char? carCategoryType = null,
        char? carBodyType = null,
        char? carDriveType = null,
        char? carFuelAndAirConSystem = null)
        {
            if (minPrice.HasValue)
            {
                offers = offers.Where(offer => offer.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                offers = offers.Where(offer => offer.Price <= maxPrice.Value);
            }

            if (carCategoryType.HasValue && Enum.IsDefined(typeof(CarCategory), (int)carCategoryType.Value))
            {
                offers = offers.Where(offer => offer.CarCategory == carCategoryType.Value);
            }

            if (carBodyType.HasValue && Enum.IsDefined(typeof(CarBodyType), (int)carBodyType.Value))
            {
                offers = offers.Where(offer => offer.CarBodyType == carBodyType.Value);
            }

            if (carDriveType.HasValue && Enum.IsDefined(typeof(CarDriveType), (int)carDriveType.Value))
            {
                offers = offers.Where(offer => offer.CarDriveType == carDriveType.Value);
            }

            if (carFuelAndAirConSystem.HasValue && Enum.IsDefined(typeof(CarFuelAndAirConSystem), (int)carFuelAndAirConSystem.Value))
            {
                offers = offers.Where(offer => offer.CarFuelAndAirConSystem == carFuelAndAirConSystem.Value);
            }

            return offers.OrderBy(offer => offer.Price).ThenBy(offer => offer.SupplierName);
        }
    }
}
