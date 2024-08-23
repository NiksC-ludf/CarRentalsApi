namespace CarRentals.Services.BestRentals
{
    /// <summary>
    /// Best Rentals service logic.
    /// </summary>
    public interface IBestRentalsService
    {
        /// <summary>
        /// Gets available cars from Best Rentals.
        /// </summary>
        Task<IEnumerable<BestRentalsOfferDto>> GetAvailableCars();
    }
}
