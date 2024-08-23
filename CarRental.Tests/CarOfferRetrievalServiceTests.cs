using CarRentals.Models;
using CarRentals.Repositories;
using CarRentals.Services.CarOfferAggregator;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace CarRentals.Tests.Services
{
    public class CarOfferRetrievalServiceTests
    {
        private readonly Mock<ICarOfferAggregatorService> _carOfferAggregatorServiceMock;
        private readonly Mock<ILogger<CarOfferRetrievalService>> _loggerMock;
        private readonly Mock<IMemoryCache> _cacheMock;
        private readonly Mock<ICarOfferRepository> _repositoryMock;
        private readonly CarOfferRetrievalService _service;

        public CarOfferRetrievalServiceTests()
        {
            _carOfferAggregatorServiceMock = new Mock<ICarOfferAggregatorService>();
            _loggerMock = new Mock<ILogger<CarOfferRetrievalService>>();
            _cacheMock = new Mock<IMemoryCache>();
            _repositoryMock = new Mock<ICarOfferRepository>();

            _service = new CarOfferRetrievalService(
                _carOfferAggregatorServiceMock.Object,
                _loggerMock.Object,
                _cacheMock.Object,
                _repositoryMock.Object);
        }

        [Fact]
        public async Task GetAvailableCars_ReturnsCachedOffers_WhenCacheIsValid()
        {
            // Arrange
            var cachedOffers = new List<CarOffer>
            {
                new CarOffer { Price = 100, VehicleName = "Car A" },
                new CarOffer { Price = 200, VehicleName = "Car B" }
            };

            object cacheEntry = cachedOffers;
            _cacheMock
                .Setup(cache => cache.TryGetValue(It.IsAny<string>(), out cacheEntry))
                .Returns(true);

            // Act
            var result = await _service.GetAvailableCars();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAvailableCars_ReturnsDbOffers_WhenCacheIsExpired()
        {
            // Arrange
            var dbOffers = new List<CarOffer>
            {
                new CarOffer { Price = 150, VehicleName = "Car C" }
            };

            object cacheEntry = dbOffers;
            _cacheMock
                .Setup(cache => cache.TryGetValue(It.IsAny<string>(), out cacheEntry))
                .Returns(false);
            _repositoryMock.Setup(repo => repo.GetLastUpdateTime()).ReturnsAsync(DateTime.UtcNow);
            _repositoryMock.Setup(repo => repo.GetCarOffers()).ReturnsAsync(dbOffers);
            var cacheEntryMock = new Mock<ICacheEntry>();
            _cacheMock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                      .Returns(cacheEntryMock.Object);

            // Act
            var result = await _service.GetAvailableCars();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task GetAvailableCars_ReturnsAggregatedOffers_WhenDbIsExpired()
        {
            // Arrange
            var aggregatedOffers = new List<CarOffer>
            {
                new CarOffer { Price = 120, VehicleName = "Car D" }
            };

            object cacheEntry = aggregatedOffers;
            _cacheMock
                .Setup(cache => cache.TryGetValue(It.IsAny<string>(), out cacheEntry))
                .Returns(false);
            _repositoryMock.Setup(repo => repo.GetLastUpdateTime()).ReturnsAsync((DateTime?)null);
            _carOfferAggregatorServiceMock.Setup(service => service.FetchOffersFromServices()).ReturnsAsync(aggregatedOffers);
            var cacheEntryMock = new Mock<ICacheEntry>();
            _cacheMock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                      .Returns(cacheEntryMock.Object);

            // Act
            var result = await _service.GetAvailableCars();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task GetAvailableCars_LogsError_WhenAggregatorServiceFails()
        {
            // Arrange
            object cacheEntry = null;
            _cacheMock
                .Setup(cache => cache.TryGetValue(It.IsAny<string>(), out cacheEntry))
                .Returns(false);
            _repositoryMock.Setup(repo => repo.GetLastUpdateTime()).ReturnsAsync((DateTime?)null);
            _carOfferAggregatorServiceMock.Setup(service => service.FetchOffersFromServices()).ThrowsAsync(new Exception("Service error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetAvailableCars());
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("An error occurred while aggregating car offers.", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

        }

        [Fact]
        public async Task GetAvailableCars_AppliesFiltersCorrectly()
        {
            // Arrange
            var offers = new List<CarOffer>
            {
                new CarOffer { Price = 100, VehicleName = "Car A", CarCategory = 'E', CarBodyType = 'C', CarDriveType = 'M', CarFuelAndAirConSystem = 'R' },
                new CarOffer { Price = 200, VehicleName = "Car B", CarCategory = 'E', CarBodyType = 'C', CarDriveType = 'M', CarFuelAndAirConSystem = 'R' },
                new CarOffer { Price = 150, VehicleName = "Car C", CarCategory = 'E', CarBodyType = 'C', CarDriveType = 'M', CarFuelAndAirConSystem = 'R' }
            };

            object cacheEntry = null;
            _cacheMock
                .Setup(cache => cache.TryGetValue(It.IsAny<string>(), out cacheEntry))
                .Returns(false);
            _repositoryMock.Setup(repo => repo.GetLastUpdateTime()).ReturnsAsync(DateTime.UtcNow);
            _repositoryMock.Setup(repo => repo.GetCarOffers()).ReturnsAsync(offers);
            var cacheEntryMock = new Mock<ICacheEntry>();
            _cacheMock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                      .Returns(cacheEntryMock.Object);

            // Act
            var result = await _service.GetAvailableCars(minPrice: 120, maxPrice: 180, carCategoryType: 'E', carBodyType: 'C', carDriveType: 'M', carFuelAndAirConSystem: 'R');

            // Assert
            Assert.Single(result);
            Assert.Equal(150, result.First().Price);
        }

        [Fact]
        public async Task GetAvailableCars_ReturnsAll_WhenNoFiltersApplied()
        {
            // Arrange
            var offers = new List<CarOffer>
            {
                new CarOffer { Price = 100, VehicleName = "Car A" },
                new CarOffer { Price = 200, VehicleName = "Car B" }
            };

            object cacheEntry = null;
            _cacheMock
                .Setup(cache => cache.TryGetValue(It.IsAny<string>(), out cacheEntry))
                .Returns(false);
            _repositoryMock.Setup(repo => repo.GetLastUpdateTime()).ReturnsAsync(DateTime.UtcNow);
            _repositoryMock.Setup(repo => repo.GetCarOffers()).ReturnsAsync(offers);
            var cacheEntryMock = new Mock<ICacheEntry>();
            _cacheMock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                      .Returns(cacheEntryMock.Object);

            // Act
            var result = await _service.GetAvailableCars();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAvailableCars_IgnoresInvalidCarCategoryType()
        {
            // Arrange
            var offers = new List<CarOffer>
            {
                new CarOffer { Price = 100, VehicleName = "Car A", CarCategory = 'E', CarBodyType = 'C', CarDriveType = 'M', CarFuelAndAirConSystem = 'R' },
                new CarOffer { Price = 200, VehicleName = "Car B", CarCategory = 'E', CarBodyType = 'C', CarDriveType = 'M', CarFuelAndAirConSystem = 'R' }
            };

            object cacheEntry = null;
            _cacheMock
                .Setup(cache => cache.TryGetValue(It.IsAny<string>(), out cacheEntry))
                .Returns(false);
            _repositoryMock.Setup(repo => repo.GetLastUpdateTime()).ReturnsAsync(DateTime.UtcNow);
            _repositoryMock.Setup(repo => repo.GetCarOffers()).ReturnsAsync(offers);
            var cacheEntryMock = new Mock<ICacheEntry>();
            _cacheMock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                      .Returns(cacheEntryMock.Object);

            // Act
            var result = await _service.GetAvailableCars(carCategoryType: 'Z'); // 'Z' is not a valid CarCategory

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAvailableCars_IgnoresInvalidCarBodyType()
        {
            // Arrange
            var offers = new List<CarOffer>
            {
                new CarOffer { Price = 100, VehicleName = "Car A", CarCategory = 'E', CarBodyType = 'C', CarDriveType = 'M', CarFuelAndAirConSystem = 'R' },
                new CarOffer { Price = 200, VehicleName = "Car B", CarCategory = 'E', CarBodyType = 'C', CarDriveType = 'M', CarFuelAndAirConSystem = 'R' }
            };

            object cacheEntry = null;
            _cacheMock
                .Setup(cache => cache.TryGetValue(It.IsAny<string>(), out cacheEntry))
                .Returns(false);
            _repositoryMock.Setup(repo => repo.GetLastUpdateTime()).ReturnsAsync(DateTime.UtcNow);
            _repositoryMock.Setup(repo => repo.GetCarOffers()).ReturnsAsync(offers);
            var cacheEntryMock = new Mock<ICacheEntry>();
            _cacheMock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                      .Returns(cacheEntryMock.Object);

            // Act
            var result = await _service.GetAvailableCars(carBodyType: 'A'); // 'A' is not a valid CarBodyType

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAvailableCars_IgnoresInvalidCarDriveType()
        {
            // Arrange
            var offers = new List<CarOffer>
            {
                new CarOffer { Price = 100, VehicleName = "Car A", CarCategory = 'E', CarBodyType = 'C', CarDriveType = 'M', CarFuelAndAirConSystem = 'R' },
                new CarOffer { Price = 200, VehicleName = "Car B", CarCategory = 'E', CarBodyType = 'C', CarDriveType = 'M', CarFuelAndAirConSystem = 'R' }
            };

            object cacheEntry = null;
            _cacheMock
                .Setup(cache => cache.TryGetValue(It.IsAny<string>(), out cacheEntry))
                .Returns(false);
            _repositoryMock.Setup(repo => repo.GetLastUpdateTime()).ReturnsAsync(DateTime.UtcNow);
            _repositoryMock.Setup(repo => repo.GetCarOffers()).ReturnsAsync(offers);
            var cacheEntryMock = new Mock<ICacheEntry>();
            _cacheMock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                      .Returns(cacheEntryMock.Object);

            // Act
            var result = await _service.GetAvailableCars(carDriveType: 'Z'); // 'Z' is not a valid CarDriveType

            // Assert
            Assert.Equal(2, result.Count());
        }

    }
}
