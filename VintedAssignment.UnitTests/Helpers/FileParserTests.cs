using Moq;
using VintedAssignment.Helpers.Interfaces;
using VintedExercise.Constants;
using VintedExercise.Helpers.Interfaces;
using VintedExercise.Helpers.VintedExercise.Helpers;
using VintedExercise.Models;


namespace VintedAssignment.UnitTests.Helpers
{
    public class FileParserTests
    {
        private Mock<IValidator> _mockValidator;
        private Mock<IFileReader> _mockFileReader;
        private IFileParser _fileParser;

        [SetUp]
        public void SetUp()
        {
            _mockValidator = new Mock<IValidator>();
            _mockFileReader = new Mock<IFileReader>();
            _fileParser = new FileParser(_mockValidator.Object, _mockFileReader.Object);
        }

        [Test]
        public void ReadShipments_WhenValidLines_ShouldReturnValidShipments()
        {
            // Arrange
            var lines = new[]
            {
                "2024-08-01 S LP",
                "2024-08-02 S MR",
            };

            var shipments = new[]
            {
                new Shipment { Date = new DateTime(2024, 8, 1), Size = PackageSize.S, Carrier = PostalService.LP, FullPrice = ShippingPrices.LP_S, IsValid = true },
                new Shipment { Date = new DateTime(2024, 8, 2), Size = PackageSize.S, Carrier = PostalService.MR, FullPrice = ShippingPrices.MR_S, IsValid = true },

            };
            _mockFileReader.Setup(fr => fr.ReadAllLines(It.IsAny<string>())).Returns(lines);
            _mockValidator.Setup(v => v.IsValid(It.IsAny<string>(), out It.Ref<DateTime>.IsAny, out It.Ref<PackageSize>.IsAny, out It.Ref<PostalService>.IsAny))
                .Returns((string line, out DateTime date, out PackageSize size, out PostalService carrier) =>
                {
                    switch (line)
                    {
                        case "2024-08-01 S LP":
                            date = new DateTime(2024, 8, 1);
                            size = PackageSize.S;
                            carrier = PostalService.LP;
                            return true;
                        case "2024-08-02 S MR":
                            date = new DateTime(2024, 8, 2);
                            size = PackageSize.S;
                            carrier = PostalService.MR;
                            return true;
                        default:
                            date = default;
                            size = default;
                            carrier = default;
                            return false;
                    }
                });

            // Act
            var result = _fileParser.ReadShipments("dummyFilePath").ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(shipments.Length));
            for (int i = 0; i < shipments.Length; i++)
            {
                if (shipments[i].IsValid)
                {
                    Assert.That(result[i].Date, Is.EqualTo(shipments[i].Date));
                    Assert.That(result[i].Size, Is.EqualTo(shipments[i].Size));
                    Assert.That(result[i].Carrier, Is.EqualTo(shipments[i].Carrier));
                    Assert.That(result[i].FullPrice, Is.EqualTo(shipments[i].FullPrice));
                }
                Assert.That(result[i].IsValid, Is.EqualTo(shipments[i].IsValid));
            }
        }

        [Test]
        public void ReadShipments_WhenEmptyLine_ShouldReturnEmptyList()
        {
            // Arrange
            var lines = new string[] { };
            _mockFileReader.Setup(fr => fr.ReadAllLines(It.IsAny<string>())).Returns(lines);
            _mockValidator.Setup(v => v.IsValid(It.IsAny<string>(), out It.Ref<DateTime>.IsAny, out It.Ref<PackageSize>.IsAny, out It.Ref<PostalService>.IsAny))
                .Returns(false);

            // Act
            var result = _fileParser.ReadShipments("dummyFilePath").ToList();

            // Assert
            Assert.That(result, Is.Empty, "Expected the list of shipments to be empty for empty input lines.");
        }



        [Test]
        public void ReadShipments_WhenInvalidLines_ShouldReturnsInvalidShipments()
        {
            // Arrange
            var filePath = "path/to/file";
            var lines = new[] { "invalid line" };

            _mockFileReader.Setup(fr => fr.ReadAllLines(filePath)).Returns(lines);
            _mockValidator.Setup(v => v.IsValid(It.IsAny<string>(), out It.Ref<DateTime>.IsAny, out It.Ref<PackageSize>.IsAny, out It.Ref<PostalService>.IsAny))
                .Returns(false);

            // Act
            var result = _fileParser.ReadShipments(filePath).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.IsFalse(result[0].IsValid);
        }
    }
}