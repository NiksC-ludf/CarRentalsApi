namespace CarRentals.Services.NorthernRentals
{
    /// <summary>
    /// Northern Rentals service logic.
    /// </summary>
    public interface INorthernRentalsService
    {
        /// <summary>
        /// Get available cars from Northern Rentals.
        /// </summary>
        Task<IEnumerable<NorthernRentalsOfferDto>> GetAvailableCars();
    }
}
