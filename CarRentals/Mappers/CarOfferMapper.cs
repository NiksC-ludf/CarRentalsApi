namespace CarRentals.Mappers
{
    using CarRentals.Models;
    using CarRentals.Services.BestRentals;
    using CarRentals.Services.SouthRentals;
    using CarRentals.Services.NorthernRentals;

    /// <summary>
    /// Contains methods to map car offer DTOs to <see cref="CarOffer"/>.
    /// </summary>
    public static class CarOfferMapper
    {
        /// <summary>
        /// Map <see cref="BestRentalsOfferDto"/> to <see cref="CarOffer"/>.
        /// </summary>
        /// <param name="dto">Dto to be mapped.</param>
        public static CarOffer ToCarOffer(BestRentalsOfferDto dto)
        {
            var (sipp1, sipp2, sipp3, sipp4) = SplitSippCode(dto.Sipp);

            return new CarOffer
            {
                SuppliersUniqueId = dto.UniqueId,
                Price = dto.RentalCost,
                Currency = dto.RentalCostCurrency,
                VehicleName = dto.Vehicle,
                SippCode = dto.Sipp,
                ImageLink = dto.ImageLink,
                SupplierLogo = dto.Logo,
                SupplierName = "Best Rentals",
                CarCategory = sipp1,
                CarBodyType = sipp2,
                CarDriveType = sipp3,
                CarFuelAndAirConSystem = sipp4,
            };
        }

        /// <summary>
        /// Map <see cref="SouthRentalsOfferDto"/> to <see cref="CarOffer"/>.
        /// </summary>
        /// <param name="dto">Dto to be mapped.</param>
        public static CarOffer ToCarOffer(SouthRentalsOfferDto dto)
        {
            var (sipp1, sipp2, sipp3, sipp4) = SplitSippCode(dto.AcrissCode);

            return new CarOffer
            {
                SuppliersUniqueId = dto.QuoteNumber,
                Price = dto.Price,
                Currency = dto.Currency,
                VehicleName = dto.VehicleName,
                SippCode = dto.AcrissCode,
                ImageLink = dto.ImageLink,
                SupplierLogo = dto.LogoLink,
                SupplierName = "South Rentals",
                CarCategory = sipp1,
                CarBodyType = sipp2,
                CarDriveType = sipp3,
                CarFuelAndAirConSystem = sipp4,
            };
        }

        /// <summary>
        /// Map <see cref="NorthernRentalsOfferDto"/> to <see cref="CarOffer"/>.
        /// </summary>
        /// <param name="dto">Dto to be mapped.</param>
        public static CarOffer ToCarOffer(NorthernRentalsOfferDto dto)
        {
            var (sipp1, sipp2, sipp3, sipp4) = SplitSippCode(dto.SippCode);

            return new CarOffer
            {
                SuppliersUniqueId = dto.Id,
                Price = dto.Price,
                Currency = dto.Currency,
                VehicleName = dto.VehicleName,
                SippCode = dto.SippCode,
                ImageLink = dto.Image,
                SupplierLogo = dto.SupplierLogo,
                SupplierName = "Northern Rentals",
                CarCategory = sipp1,
                CarBodyType = sipp2,
                CarDriveType = sipp3,
                CarFuelAndAirConSystem = sipp4,
            };
        }

        /// <summary>
        /// Splits a SIPP code into four characters.
        /// </summary>
        /// <param name="sipp">The SIPP code to split.</param>
        /// <returns>A tuple containing the four characters of the SIPP code.</returns>
        private static (char?, char?, char?, char?) SplitSippCode(string? sipp)
        {
            if (string.IsNullOrWhiteSpace(sipp) || sipp.Length != 4)
            {
                // Can be nullable by external api schema.
                return (null, null, null, null);
            }

            if (sipp.Length != 4)
            {
                throw new ArgumentException("SIPP code must be exactly 4 characters long.");
            }

            return (sipp[0], sipp[1], sipp[2], sipp[3]);
        }
    }
}
