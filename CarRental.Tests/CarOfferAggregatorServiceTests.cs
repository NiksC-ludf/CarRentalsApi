namespace CarRentals.Tests.Services
{
    using CarRentals.Services.BestRentals;
    using CarRentals.Services.CarOfferAggregator;
    using CarRentals.Services.NorthernRentals;
    using CarRentals.Services.SouthRentals;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class CarOfferAggregatorServiceTests
    {
        private readonly Mock<IBestRentalsService> _bestRentalsServiceMock;
        private readonly Mock<ISouthRentalsService> _southRentalsServiceMock;
        private readonly Mock<INorthernRentalsService> _northernRentalsServiceMock;
        private readonly Mock<ILogger<CarOfferAggregatorService>> _loggerMock;
        private readonly CarOfferAggregatorService _service;

        public CarOfferAggregatorServiceTests()
        {
            _bestRentalsServiceMock = new Mock<IBestRentalsService>();
            _southRentalsServiceMock = new Mock<ISouthRentalsService>();
            _northernRentalsServiceMock = new Mock<INorthernRentalsService>();
            _loggerMock = new Mock<ILogger<CarOfferAggregatorService>>();

            _service = new CarOfferAggregatorService(
                _bestRentalsServiceMock.Object,
                _southRentalsServiceMock.Object,
                _northernRentalsServiceMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task FetchOffersFromServices_ReturnsAggregatedOffers()
        {
            // Arrange
            var bestRentalsOffers = new List<BestRentalsOfferDto>
            {
                new BestRentalsOfferDto { RentalCost = 100, Vehicle = "BestRentals Car A" },
                new BestRentalsOfferDto { RentalCost = 200, Vehicle = "BestRentals Car B" }
            };

            var southRentalsOffers = new List<SouthRentalsOfferDto>
            {
                new SouthRentalsOfferDto { Price = 150, VehicleName = "SouthRentals Car A" }
            };

            var northernRentalsOffers = new List<NorthernRentalsOfferDto>
            {
                new NorthernRentalsOfferDto { Price = 120, VehicleName = "NorthernRentals Car A" }
            };

            _bestRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ReturnsAsync(bestRentalsOffers);

            _southRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ReturnsAsync(southRentalsOffers);

            _northernRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ReturnsAsync(northernRentalsOffers);

            // Act
            var result = await _service.FetchOffersFromServices();

            // Assert
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task FetchOffersFromServices_HandlesBestRentalsServiceException()
        {
            // Arrange
            var southRentalsOffers = new List<SouthRentalsOfferDto>
            {
                new SouthRentalsOfferDto { Price = 150, VehicleName = "SouthRentals Car A" }
            };

            var northernRentalsOffers = new List<NorthernRentalsOfferDto>
            {
                new NorthernRentalsOfferDto { Price = 120, VehicleName = "NorthernRentals Car A" }
            };

            _bestRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ThrowsAsync(new Exception("BestRentals service error"));

            _southRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ReturnsAsync(southRentalsOffers);

            _northernRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ReturnsAsync(northernRentalsOffers);

            // Act
            var result = await _service.FetchOffersFromServices();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task FetchOffersFromServices_HandlesSouthRentalsServiceException()
        {
            // Arrange
            var bestRentalsOffers = new List<BestRentalsOfferDto>
            {
                new BestRentalsOfferDto { RentalCost = 100, Vehicle = "BestRentals Car A" },
                new BestRentalsOfferDto { RentalCost = 200, Vehicle = "BestRentals Car B" }
            };

            var northernRentalsOffers = new List<NorthernRentalsOfferDto>
            {
                new NorthernRentalsOfferDto { Price = 120, VehicleName = "NorthernRentals Car A" }
            };

            _bestRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ReturnsAsync(bestRentalsOffers);

            _southRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ThrowsAsync(new Exception("SouthRentals service error"));

            _northernRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ReturnsAsync(northernRentalsOffers);

            // Act
            var result = await _service.FetchOffersFromServices();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task FetchOffersFromServices_HandlesNorthernRentalsServiceException()
        {
            // Arrange
            var bestRentalsOffers = new List<BestRentalsOfferDto>
            {
                new BestRentalsOfferDto { RentalCost = 100, Vehicle = "BestRentals Car A" },
                new BestRentalsOfferDto { RentalCost = 200, Vehicle = "BestRentals Car B" }
            };

            var southRentalsOffers = new List<SouthRentalsOfferDto>
            {
                new SouthRentalsOfferDto { Price = 150, VehicleName = "SouthRentals Car A" }
            };

            _bestRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ReturnsAsync(bestRentalsOffers);

            _southRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ReturnsAsync(southRentalsOffers);

            _northernRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ThrowsAsync(new Exception("NorthernRentals service error"));

            // Act
            var result = await _service.FetchOffersFromServices();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task FetchOffersFromServices_AllServicesThrowExceptions_ReturnsEmpty()
        {
            // Arrange
            _bestRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ThrowsAsync(new Exception("BestRentals service error"));

            _southRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ThrowsAsync(new Exception("SouthRentals service error"));

            _northernRentalsServiceMock.Setup(service => service.GetAvailableCars())
                .ThrowsAsync(new Exception("NorthernRentals service error"));

            // Act
            var result = await _service.FetchOffersFromServices();

            // Assert
            Assert.Empty(result);
        }
    }
}
