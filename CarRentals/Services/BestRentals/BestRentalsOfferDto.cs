namespace CarRentals.Services.BestRentals
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Dto for Best Rentals car offer.
    /// </summary>
    public class BestRentalsOfferDto
    {
        [JsonPropertyName("uniqueId")]
        public string? UniqueId { get; set; }

        [JsonPropertyName("rentalCost")]
        public double RentalCost { get; set; }

        [JsonPropertyName("rentalCostCurrency")]
        public string? RentalCostCurrency { get; set; }

        [JsonPropertyName("vehicle")]
        public string? Vehicle { get; set; }

        [JsonPropertyName("sipp")]
        public string? Sipp { get; set; }

        [JsonPropertyName("imageLink")]
        public string? ImageLink { get; set; }

        [JsonPropertyName("logo")]
        public string? Logo { get; set; }
    }
}
