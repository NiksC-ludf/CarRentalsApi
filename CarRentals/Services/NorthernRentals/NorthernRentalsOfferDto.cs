namespace CarRentals.Services.NorthernRentals
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Dto for Nothern Rentals car offer.
    /// </summary>
    public class NorthernRentalsOfferDto
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("vehicleName")]
        public string? VehicleName { get; set; }

        [JsonPropertyName("sippCode")]
        public string? SippCode { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("supplierLogo")]
        public string? SupplierLogo { get; set; }
    }
}
