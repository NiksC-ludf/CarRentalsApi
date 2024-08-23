namespace CarRentals.Services.CarOfferAggregator
{
    using CarRentals.Models;

    /// <summary>
    /// Car offer aggregator service logic.
    /// </summary>
    public interface ICarOfferRetrievalService
    {
        /// <summary>
        /// Get all available cars from all services in one IEnumerable.
        /// </summary>
        /// <param name="minPrice">Minimum price to filter cars by.</param>
        /// <param name="maxPrice">Maximum price to filter cars by.</param>
        /// <param name="carCategoryType">Car category - SIPP code first letter. Type: char.</param>
        /// <param name="carBodyType">Car body type - SIPP code second letter. Type: char.</param>
        /// <param name="carDriveType">Car drive type - SIPP code third letter. Type: char.</param>
        /// <param name="carFuelAndAirConSystem">Car fuel and air con system - SIPP code forth letter. Type: char.</param>
        Task<IEnumerable<CarOffer>> GetAvailableCars(
            double? minPrice = null,
            double? maxPrice = null,
            char? carCategoryType = null,
            char? carBodyType = null,
            char? carDriveType = null,
            char? carFuelAndAirConSystem = null);
    }
}
