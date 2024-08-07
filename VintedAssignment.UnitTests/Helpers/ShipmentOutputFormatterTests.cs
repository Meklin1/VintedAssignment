using System.Globalization;
using VintedExercise.Helpers;
using VintedExercise.Helpers.Interfaces;
using VintedExercise.Models;

namespace VintedAssignment.UnitTests.Helpers
{
    [TestFixture]
    public class ShipmentOutputFormatterTests
    {
        private IShipmentOutputFormatter _formatter;
        private CultureInfo _currentCulture;

        [SetUp]
        public void SetUp()
        {
            _formatter = new ShipmentOutputFormatter();
            _currentCulture = CultureInfo.CurrentCulture;
        }

        [Test]
        public void FormatShipmentOutput_WhenValidShipment_ShouldReturnFormattedString()
        {
            // Arrange
            var shipment = new Shipment
            {
                Line = "2024-08-01 S LP",
                Date = new DateTime(2024, 8, 1),
                Size = PackageSize.S,
                Carrier = PostalService.LP,
                FullPrice = 10.00,
                DiscountAmount = 2.50,
                IsValid = true
            };
            var reducedPrice = shipment.FullPrice - shipment.DiscountAmount;
            var discountDisplay = shipment.DiscountAmount.ToString("F2", _currentCulture);
            var expectedOutput = $"{shipment.Line} {reducedPrice.ToString("F2", _currentCulture)} {discountDisplay}";

            // Act
            var result = _formatter.FormatShipmentOutput(shipment);

            // Assert
            Assert.That(result, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void FormatShipmentOutput_WhenValidShipmentAndNoDiscount_ShouldReturnFormattedString()
        {
            // Arrange
            var shipment = new Shipment
            {
                Line = "2024-08-02 M MR",
                Date = new DateTime(2024, 8, 2),
                Size = PackageSize.M,
                Carrier = PostalService.MR,
                FullPrice = 15.00,
                DiscountAmount = 0.00,
                IsValid = true
            };
            var expectedOutput = $"{shipment.Line} {shipment.FullPrice.ToString("F2", _currentCulture)} -";

            // Act
            var result = _formatter.FormatShipmentOutput(shipment);

            // Assert
            Assert.That(result, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void FormatShipmentOutput_WhenInvalidShipment_ShouldReturnIgnoredString()
        {
            // Arrange
            var shipment = new Shipment
            {
                Line = "2024-08-03 L LP",
                Date = new DateTime(2024, 8, 3),
                Size = PackageSize.L,
                Carrier = PostalService.LP,
                FullPrice = 20.00,
                DiscountAmount = 5.00,
                IsValid = false
            };
            var expectedOutput = $"{shipment.Line} Ignored";

            // Act
            var result = _formatter.FormatShipmentOutput(shipment);

            // Assert
            Assert.That(result, Is.EqualTo(expectedOutput));
        }
    }
}