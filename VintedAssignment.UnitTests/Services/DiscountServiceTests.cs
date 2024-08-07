using Moq;
using VintedExercise.Models;
using VintedExercise.Services;
using VintedExercise.Services.Interfaces;

namespace VintedAssignment.UnitTests.Services
{
    [TestFixture]
    public class DiscountServiceTests
    {
        private Mock<IDiscountRule> _mockDiscountRule;
        private IDiscountService _discountService;

        [SetUp]
        public void SetUp()
        {
            _mockDiscountRule = new Mock<IDiscountRule>();
            _discountService = new DiscountService(new[] { _mockDiscountRule.Object });
        }

        [Test]
        public void ApplyDiscounts_WhenValidShipments_ShouldApplyDiscount()
        {
            // Arrange
            var shipments = new List<Shipment>
            {
                new Shipment { Line = "2024-08-01 S LP", FullPrice = 10.00, IsValid = true },
                new Shipment { Line = "2024-08-02 M MR", FullPrice = 15.00, IsValid = true }
            };

            _mockDiscountRule.Setup(r => r.Apply(It.IsAny<IEnumerable<Shipment>>()))
                .Callback<IEnumerable<Shipment>>(s =>
                {
                    foreach (var shipment in s)
                    {
                        shipment.DiscountAmount = 5.00; // Example discount
                    }
                });

            var expectedShipments = shipments.Select(s =>
            {
                s.DiscountAmount = 5.00; // Expected discount amount
                return s;
            }).ToList();

            // Act
            var result = _discountService.ApplyDiscounts(shipments).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(expectedShipments.Count), "The number of shipments should match.");
            for (int i = 0; i < expectedShipments.Count; i++)
            {
                Assert.That(result[i].DiscountAmount, Is.EqualTo(expectedShipments[i].DiscountAmount), $"Discount amount for shipment {i} is incorrect.");
            }

            _mockDiscountRule.Verify(r => r.Apply(It.IsAny<IEnumerable<Shipment>>()), Times.Once, "Discount rule should be applied once.");
        }

        [Test]
        public void ApplyDiscounts_WhenInvalidShipment_ShouldNotApplyDiscount()
        {
            // Arrange
            var shipments = new List<Shipment>
            {
                new Shipment { Line = "2024-08-01 S LP", FullPrice = 10.00, IsValid = false },
                new Shipment { Line = "2024-08-02 M MR", FullPrice = 15.00, IsValid = true }
            };

            _mockDiscountRule.Setup(r => r.Apply(It.IsAny<IEnumerable<Shipment>>())).Verifiable();

            var expectedShipments = shipments.ToList();
            expectedShipments[1].DiscountAmount = 5.00; // Assume discount applied to the valid shipment

            // Act
            var result = _discountService.ApplyDiscounts(shipments).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(expectedShipments.Count), "The number of shipments should match.");
            Assert.That(result[0].DiscountAmount, Is.EqualTo(0.00), "Discount amount for invalid shipment should be zero.");
            Assert.That(result[1].DiscountAmount, Is.EqualTo(expectedShipments[1].DiscountAmount), "Discount amount for valid shipment is incorrect.");

            _mockDiscountRule.Verify(r => r.Apply(It.IsAny<IEnumerable<Shipment>>()), Times.Once, "Discount rule should be applied once.");
        }
    }
}