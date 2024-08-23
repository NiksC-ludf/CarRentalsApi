namespace CarRentals.Controllers
{
    using CarRentals.Models;
    using CarRentals.Services.CarOfferAggregator;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class CarOfferController : ControllerBase
    {
        private readonly ICarOfferRetrievalService _carOfferRetrievalService;
        private readonly ILogger<CarOfferController> _logger;

        public CarOfferController(ICarOfferRetrievalService carOfferRetrievalService, ILogger<CarOfferController> logger)
        {
            _carOfferRetrievalService = carOfferRetrievalService;
            _logger = logger;
        }

        /// <summary>
        /// Gets car offers.
        /// </summary>
        /// <param name="minPrice">Minimum price to filter cars by.</param>
        /// <param name="maxPrice">Maximum price to filter cars by.</param>
        /// <param name="carCategoryType">Car category - SIPP code first letter. Type: char.</param>
        /// <param name="carBodyType">Car body type - SIPP code second letter. Type: char.</param>
        /// <param name="carDriveType">Car drive type - SIPP code third letter. Type: char.</param>
        /// <param name="carFuelAndAirConSystem">Car fuel and air con system - SIPP code forth letter. Type: char.</param>
        [HttpGet(Name = "GetCarOffers")]
        [ProducesResponseType(typeof(IEnumerable<CarOffer>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCarOffers(
            [FromQuery] double? minPrice = null,
            [FromQuery] double? maxPrice = null,
            [FromQuery] char? carCategoryType = null,
            [FromQuery] char? carBodyType = null,
            [FromQuery] char? carDriveType = null,
            [FromQuery] char? carFuelAndAirConSystem = null)
        {
            try
            {
                return Ok(await _carOfferRetrievalService.GetAvailableCars(minPrice, maxPrice, carCategoryType, carBodyType, carDriveType, carFuelAndAirConSystem));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting car offers.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}
