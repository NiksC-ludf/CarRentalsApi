namespace CarRentals.Services.SouthRentals
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Dto for South Rentals car offer.
    /// </summary>
    public class SouthRentalsOfferDto
    {
        [JsonPropertyName("quoteNumber")]
        public string? QuoteNumber { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("vehicleName")]
        public string? VehicleName { get; set; }

        [JsonPropertyName("acrissCode")]
        public string? AcrissCode { get; set; }

        [JsonPropertyName("imageLink")]
        public string? ImageLink { get; set; }

        [JsonPropertyName("logoLink")]
        public string? LogoLink { get; set; }
    }
}
