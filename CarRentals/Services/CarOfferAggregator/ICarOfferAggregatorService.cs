namespace CarRentals.Services.CarOfferAggregator
{
    using CarRentals.Models;

    /// <summary>
    /// Car offer aggregator service logic.
    /// </summary>
    public interface ICarOfferAggregatorService
    {
        /// <summary>
        /// Fetches car offers from all services.
        /// </summary>
        Task<IEnumerable<CarOffer>> FetchOffersFromServices();
    }
}
