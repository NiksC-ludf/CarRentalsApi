namespace CarRentals.Services.SouthRentals
{
    /// <summary>
    /// South Rentals service logic.
    /// </summary>
    public interface ISouthRentalsService
    {
        /// <summary>
        /// Gets all available cars from South Rentals.
        /// </summary>
        Task<IEnumerable<SouthRentalsOfferDto>> GetAvailableCars();
    }
}
