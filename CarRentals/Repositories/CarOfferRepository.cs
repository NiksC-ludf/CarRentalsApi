namespace CarRentals.Repositories
{
    using CarRentals.Data;
    using CarRentals.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Car offer repository.
    /// </summary>
    public class CarOfferRepository : ICarOfferRepository
    {
        private readonly ApplicationDbContext _context;

        public CarOfferRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Updates car offers in the database, removes offers that are no longer available.
        /// </summary>
        /// <param name="carOffers">Car offers to update.</param>
        public async Task UpdateCarOffers(IEnumerable<CarOffer> carOffers)
        {
            var existingOffers = await _context.CarOffers.AsNoTracking().ToListAsync();

            var offersToRemove = existingOffers
                .Where(eo => !carOffers.Any(co => co.SuppliersUniqueId == eo.SuppliersUniqueId && co.SupplierName == eo.SupplierName))
                .ToList();
            _context.CarOffers.RemoveRange(offersToRemove);

            foreach (var offer in carOffers)
            {
                var existingOffer = existingOffers.FirstOrDefault(o => o.SuppliersUniqueId == offer.SuppliersUniqueId && o.SupplierName == offer.SupplierName);

                if (existingOffer == null)
                {
                    _context.CarOffers.Add(offer);
                }
                else
                {
                    _context.Entry(existingOffer).CurrentValues.SetValues(offer);
                    _context.Entry(existingOffer).State = EntityState.Modified;
                }
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the time of the oldest updated car offer.
        /// </summary>
        public async Task<DateTime?> GetLastUpdateTime()
        {
            return await _context.CarOffers
                .OrderByDescending(co => co.LastModified)
                .Select(co => co.LastModified)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets all car offers from database.
        /// </summary>
        public async Task<IEnumerable<CarOffer>> GetCarOffers()
        {
            return await _context.CarOffers.ToListAsync();
        }
    }
}
