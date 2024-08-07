using VintedExercise.Models;
using VintedExercise.Services.Rules;

namespace VintedAssignment.UnitTests.Services.Rules
{
    [TestFixture]
    public class LowestSPackagePriceRuleTests
    {
        private LowestSPackagePriceRule _rule;

        [SetUp]
        public void SetUp()
        {
            _rule = new LowestSPackagePriceRule();
        }

        [Test]
        public void Apply_WhenValidShipments_ShouldApplyDiscountCorrectlyForMultipleMonths()
        {
            // Arrange
            var shipments = new List<Shipment>
            {
                new Shipment { Date = new DateTime(2024, 8, 1), Size = PackageSize.S, FullPrice = 10.00, IsValid = true },
                new Shipment { Date = new DateTime(2024, 8, 5), Size = PackageSize.S, FullPrice = 13.00, IsValid = true },
                new Shipment { Date = new DateTime(2024, 8, 15), Size = PackageSize.M, FullPrice = 15.00, IsValid = true },
                new Shipment { Date = new DateTime(2024, 9, 1), Size = PackageSize.S, FullPrice = 13.00, IsValid = true },
                new Shipment { Date = new DateTime(2024, 9, 10), Size = PackageSize.S, FullPrice = 13.00, IsValid = true }
            };

            var expectedDiscountsAugust = new Dictionary<DateTime, double>
            {
                { new DateTime(2024, 8, 1), 0.00 }, // Lowest price, no discount
                { new DateTime(2024, 8, 5), 3 }  // Discount is 13.00 - 10.00 = 3.00
            };

            var expectedDiscountsSeptember = new Dictionary<DateTime, double>
            {
                { new DateTime(2024, 9, 1), 3.00 }, // Discount is 13.00 - 10.00 = 3.00
                { new DateTime(2024, 9, 10), 3.00 } // Discount is 13.00 - 10.00 = 3.00
            };

            // Act
            _rule.Apply(shipments);

            // Assert
            foreach (var shipment in shipments)
            {
                if (shipment.Size == PackageSize.S)
                {
                    var expectedDiscount = shipment.Date.Month == 8
                        ? expectedDiscountsAugust[shipment.Date]
                        : expectedDiscountsSeptember[shipment.Date];

                    Assert.That(shipment.DiscountAmount, Is.EqualTo(expectedDiscount), $"Discount for shipment on {shipment.Date.ToShortDateString()} is incorrect.");
                }
                else
                {
                    Assert.That(shipment.DiscountAmount, Is.EqualTo(0.00), $"Discount for non-small package shipment on {shipment.Date.ToShortDateString()} should be zero.");
                }
            }
        }

        [Test]
        public void Apply_WhenExceedsMonthlyDiscountLimit_SouldLimitDiscountByCap()
        {
            // Arrange
            var shipments = new List<Shipment>
            {
                new Shipment { Date = new DateTime(2024, 8, 1), Size = PackageSize.S, FullPrice = 10.00, IsValid = true },
                new Shipment { Date = new DateTime(2024, 8, 5), Size = PackageSize.S, FullPrice = 15.00, IsValid = true },
                new Shipment { Date = new DateTime(2024, 8, 15), Size = PackageSize.S, FullPrice = 30.00, IsValid = true }
            };

            var expectedDiscounts = new Dictionary<DateTime, double>
            {
                { new DateTime(2024, 8, 1), 0.00 }, // Lowest price, no discount
                { new DateTime(2024, 8, 5), 5.00 }, // Discount is 15.00 - 10.00 = 5.00
                { new DateTime(2024, 8, 15), 5.00 } // Discount is 30.00 - 10.00 = 20.00 but only 5.00 eur is left for this month
            };

            // Act
            _rule.Apply(shipments);

            // Assert
            foreach (var shipment in shipments)
            {
                var expectedDiscount = expectedDiscounts[shipment.Date];
                Assert.That(shipment.DiscountAmount, Is.EqualTo(expectedDiscount), $"Discount for shipment on {shipment.Date.ToShortDateString()} is incorrect.");
            }
        }
    }
}
