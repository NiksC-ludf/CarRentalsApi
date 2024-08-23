namespace CarRentals.Repositories
{
    using CarRentals.Models;

    /// <summary>
    /// Car offer repository.
    /// </summary>
    public interface ICarOfferRepository
    {
        /// <summary>
        /// Updates car offers in the database.
        /// </summary>
        /// <param name="carOffers">Car offers to update.</param>
        Task UpdateCarOffers(IEnumerable<CarOffer> carOffers);

        /// <summary>
        /// Gets the time of the oldest updated car offer.
        /// </summary>
        Task<DateTime?> GetLastUpdateTime();

        /// <summary>
        /// Gets all car offers from database.
        /// </summary>
        Task<IEnumerable<CarOffer>> GetCarOffers();
    }
}
