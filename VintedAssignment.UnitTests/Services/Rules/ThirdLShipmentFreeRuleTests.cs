using VintedExercise.Models;
using VintedExercise.Services.Rules;

namespace VintedAssignment.Tests
{
    namespace VintedExercise.Tests
    {
        [TestFixture]
        public class ThirdLShipmentFreeRuleTests
        {
            private ThirdLShipmentFreeRule _rule;

            [SetUp]
            public void SetUp()
            {
                _rule = new ThirdLShipmentFreeRule();
            }

            [Test]
            public void Apply_ShouldApplyDiscountForThirdLShipment()
            {
                // Arrange
                var shipments = new List<Shipment>
                {
                    new Shipment { Date = new DateTime(2024, 8, 1), Size = PackageSize.L, Carrier = PostalService.LP, FullPrice = 10.00, IsValid = true },
                    new Shipment { Date = new DateTime(2024, 8, 5), Size = PackageSize.L, Carrier = PostalService.LP, FullPrice = 10.00, IsValid = true },
                    new Shipment { Date = new DateTime(2024, 8, 15), Size = PackageSize.L, Carrier = PostalService.LP, FullPrice = 10.00, IsValid = true }, // This should be discounted
                };

                var expectedDiscounts = new Dictionary<DateTime, double>
                {
                    { new DateTime(2024, 8, 1), 0.00 },
                    { new DateTime(2024, 8, 5), 0.00 },
                    { new DateTime(2024, 8, 15), 10.00 }, // Discount is the full price of the third L package
                };

                // Act
                _rule.Apply(shipments);

                // Assert
                foreach (var shipment in shipments)
                {
                    if (shipment.Size == PackageSize.L && shipment.Carrier == PostalService.LP)
                    {
                        var expectedDiscount = expectedDiscounts[shipment.Date];
                        Assert.That(shipment.DiscountAmount, Is.EqualTo(expectedDiscount), $"Discount for shipment on {shipment.Date.ToShortDateString()} is incorrect.");
                    }
                    else
                    {
                        Assert.That(shipment.DiscountAmount, Is.EqualTo(0.00), $"Discount for non-LP L package shipment on {shipment.Date.ToShortDateString()} should be zero.");
                    }
                }
            }

            [Test]
            public void Apply_WhenLessThanThreeLShipments_ShouldNotApplyDiscount()
            {
                // Arrange
                var shipments = new List<Shipment>
            {
                new Shipment { Date = new DateTime(2024, 8, 1), Size = PackageSize.L, Carrier = PostalService.LP, FullPrice = 10.00, IsValid = true },
                new Shipment { Date = new DateTime(2024, 8, 5), Size = PackageSize.L, Carrier = PostalService.LP, FullPrice = 10.00, IsValid = true }
            };

                // Act
                _rule.Apply(shipments);

                // Assert
                foreach (var shipment in shipments)
                {
                    Assert.That(shipment.DiscountAmount, Is.EqualTo(0.00), $"Discount for shipment on {shipment.Date.ToShortDateString()} should be zero as there are fewer than three L packages.");
                }
            }

            [Test]
            public void Apply_WhenExceedsMonthlyDiscountLimit_ShouldBeLimitedByCap()
            {
                // Arrange
                var shipments = new List<Shipment>
            {
                new Shipment { Date = new DateTime(2024, 8, 1), Size = PackageSize.L, Carrier = PostalService.LP, FullPrice = 20.00, IsValid = true },
                new Shipment { Date = new DateTime(2024, 8, 5), Size = PackageSize.L, Carrier = PostalService.LP, FullPrice = 20.00, IsValid = true },
                new Shipment { Date = new DateTime(2024, 8, 15), Size = PackageSize.L, Carrier = PostalService.LP, FullPrice = 20.00, IsValid = true }, // This should be partially discounted
            };

                var expectedDiscounts = new Dictionary<DateTime, double>
            {
                { new DateTime(2024, 8, 1), 0.00 },
                { new DateTime(2024, 8, 5), 0.00 },
                { new DateTime(2024, 8, 15), 10.00 }, // Discount is capped at 10.00 due to max limit

            };

                // Act
                _rule.Apply(shipments);

                // Assert
                foreach (var shipment in shipments)
                {
                    if (shipment.Size == PackageSize.L && shipment.Carrier == PostalService.LP)
                    {
                        var expectedDiscount = expectedDiscounts[shipment.Date];
                        Assert.That(shipment.DiscountAmount, Is.EqualTo(expectedDiscount), $"Discount for shipment on {shipment.Date.ToShortDateString()} is incorrect.");
                    }
                    else
                    {
                        Assert.That(shipment.DiscountAmount, Is.EqualTo(0.00), $"Discount for non-LP L package shipment on {shipment.Date.ToShortDateString()} should be zero.");
                    }
                }
            }
        }
    }
}
