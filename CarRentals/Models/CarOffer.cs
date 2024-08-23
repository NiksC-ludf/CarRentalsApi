namespace CarRentals.Models
{
    public class CarOffer
    {
        /// <summary>
        /// Unique identifier of the car offer for this database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Car identifier.
        /// </summary>
        public string? SuppliersUniqueId { get; set; }

        /// <summary>
        /// Car price.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Currency in which the price is expressed.
        /// </summary>
        public string? Currency { get; set; }

        /// <summary>
        /// Name of the vehicle.
        /// </summary>
        public string? VehicleName { get; set; }

        /// <summary>
        /// Vehicles sipp / acriss code - Standard Interline Passenger Procedures.
        /// </summary>
        public string? SippCode { get; set; }

        /// <summary>
        /// Car category - SIPP code first letter.
        /// Example: M - Mini, E - Economy, C - Compact.
        /// </summary>
        public char? CarCategory { get; set; }

        /// <summary>
        /// Car body type - SIPP code second letter.
        /// Example: B - 2/3 Door, C - 2/4 Door, D - 4 Door, W - Estate, V - Passenger Van.
        /// </summary>
        public char? CarBodyType { get; set; }

        /// <summary>
        /// Car drive type - SIPP code third letter.
        /// Example: M - Manual, A - Automatic.
        /// </summary>
        public char? CarDriveType { get; set; }

        /// <summary>
        /// Car fuel and air conditioning system - SIPP code fourth letter.
        /// Example: R - Unspecified fuel with air conditioning, N - Unspecified fuel without air conditioning.
        /// </summary>
        public char? CarFuelAndAirConSystem { get; set; }

        /// <summary>
        /// Link to car image.
        /// </summary>
        public string? ImageLink { get; set; }

        /// <summary>
        /// Link to supplier logo.
        /// </summary>
        public string? SupplierLogo { get; set; }

        /// <summary>
        /// Name of cars supplier.
        /// </summary>
        public string? SupplierName { get; set; }

        /// <summary>
        /// Date and time when the offer was last modified.
        /// </summary>
        public DateTime? LastModified { get; set; }
    }
}
