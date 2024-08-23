namespace CarRentals.Tests.Mappers
{
    using CarRentals.Mappers;
    using CarRentals.Services.BestRentals;
    using CarRentals.Services.NorthernRentals;
    using CarRentals.Services.SouthRentals;
    using Xunit;

    public class CarOfferMapperTests
    {
        [Fact]
        public void ToCarOffer_BestRentalsOfferDto_MapsCorrectly()
        {
            // Arrange
            var dto = new BestRentalsOfferDto
            {
                UniqueId = "BR123",
                RentalCost = 100,
                RentalCostCurrency = "USD",
                Vehicle = "BestRentals Car A",
                Sipp = "ECMR",
                ImageLink = "http://example.com/car.jpg",
                Logo = "http://example.com/logo.jpg"
            };

            // Act
            var result = CarOfferMapper.ToCarOffer(dto);

            // Assert
            Assert.Equal(dto.UniqueId, result.SuppliersUniqueId);
            Assert.Equal(dto.RentalCost, result.Price);
            Assert.Equal(dto.RentalCostCurrency, result.Currency);
            Assert.Equal(dto.Vehicle, result.VehicleName);
            Assert.Equal(dto.Sipp, result.SippCode);
            Assert.Equal(dto.ImageLink, result.ImageLink);
            Assert.Equal(dto.Logo, result.SupplierLogo);
            Assert.Equal("Best Rentals", result.SupplierName);
            Assert.Equal('E', result.CarCategory);
            Assert.Equal('C', result.CarBodyType);
            Assert.Equal('M', result.CarDriveType);
            Assert.Equal('R', result.CarFuelAndAirConSystem);
        }

        [Fact]
        public void ToCarOffer_SouthRentalsOfferDto_MapsCorrectly()
        {
            // Arrange
            var dto = new SouthRentalsOfferDto
            {
                QuoteNumber = "SR123",
                Price = 150,
                Currency = "USD",
                VehicleName = "SouthRentals Car A",
                AcrissCode = "ECMR",
                ImageLink = "http://example.com/car.jpg",
                LogoLink = "http://example.com/logo.jpg"
            };

            // Act
            var result = CarOfferMapper.ToCarOffer(dto);

            // Assert
            Assert.Equal(dto.QuoteNumber, result.SuppliersUniqueId);
            Assert.Equal(dto.Price, result.Price);
            Assert.Equal(dto.Currency, result.Currency);
            Assert.Equal(dto.VehicleName, result.VehicleName);
            Assert.Equal(dto.AcrissCode, result.SippCode);
            Assert.Equal(dto.ImageLink, result.ImageLink);
            Assert.Equal(dto.LogoLink, result.SupplierLogo);
            Assert.Equal("South Rentals", result.SupplierName);
            Assert.Equal('E', result.CarCategory);
            Assert.Equal('C', result.CarBodyType);
            Assert.Equal('M', result.CarDriveType);
            Assert.Equal('R', result.CarFuelAndAirConSystem);
        }

        [Fact]
        public void ToCarOffer_NorthernRentalsOfferDto_MapsCorrectly()
        {
            // Arrange
            var dto = new NorthernRentalsOfferDto
            {
                Id = "NR123",
                Price = 120,
                Currency = "USD",
                VehicleName = "NorthernRentals Car A",
                SippCode = "ECMR",
                Image = "http://example.com/car.jpg",
                SupplierLogo = "http://example.com/logo.jpg"
            };

            // Act
            var result = CarOfferMapper.ToCarOffer(dto);

            // Assert
            Assert.Equal(dto.Id, result.SuppliersUniqueId);
            Assert.Equal(dto.Price, result.Price);
            Assert.Equal(dto.Currency, result.Currency);
            Assert.Equal(dto.VehicleName, result.VehicleName);
            Assert.Equal(dto.SippCode, result.SippCode);
            Assert.Equal(dto.Image, result.ImageLink);
            Assert.Equal(dto.SupplierLogo, result.SupplierLogo);
            Assert.Equal("Northern Rentals", result.SupplierName);
            Assert.Equal('E', result.CarCategory);
            Assert.Equal('C', result.CarBodyType);
            Assert.Equal('M', result.CarDriveType);
            Assert.Equal('R', result.CarFuelAndAirConSystem);
        }

        [Fact]
        public void SplitSippCode_NullOrEmptySippCode_ReturnsNulls()
        {
            // Arrange
            var dto = new SouthRentalsOfferDto
            {
                QuoteNumber = "SR123",
                Price = 150,
                Currency = "USD",
                VehicleName = "SouthRentals Car A",
                AcrissCode = null,
                ImageLink = "http://example.com/car.jpg",
                LogoLink = "http://example.com/logo.jpg"
            };

            // Act
            var result = CarOfferMapper.ToCarOffer(dto);

            // Assert
            Assert.Null(result.CarCategory);
            Assert.Null(result.CarBodyType);
            Assert.Null(result.CarDriveType);
            Assert.Null(result.CarFuelAndAirConSystem);
        }
    }
}
