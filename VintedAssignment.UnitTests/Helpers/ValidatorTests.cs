using VintedExercise.Helpers.Interfaces;
using VintedExercise.Helpers.VintedExercise.Helpers;
using VintedExercise.Models;

namespace VintedAssignment.UnitTests.Helpers
{
    [TestFixture]
    public class ValidatorTests
    {
        private IValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new Validator();
        }

        [TestCase("2024-08-01 S LP", true, 2024, 8, 1, PackageSize.S, PostalService.LP)]
        [TestCase("2024-08-02 M MR", true, 2024, 8, 2, PackageSize.M, PostalService.MR)]
        [TestCase("2024-08-03 L LP", true, 2024, 8, 3, PackageSize.L, PostalService.LP)]
        [TestCase("2024-08-04 L MR", true, 2024, 8, 4, PackageSize.L, PostalService.MR)]
        public void IsValid_WHenValidLines_ShouldReturnTrue(string line, bool expectedValid, int year, int month, int day, PackageSize expectedSize, PostalService expectedCarrier)
        {
            // Act
            var result = _validator.IsValid(line, out DateTime date, out PackageSize size, out PostalService carrier);

            // Assert
            Assert.That(result, Is.EqualTo(expectedValid));
            if (expectedValid)
            {
                Assert.That(date.Year, Is.EqualTo(year));
                Assert.That(date.Month, Is.EqualTo(month));
                Assert.That(date.Day, Is.EqualTo(day));
                Assert.That(size, Is.EqualTo(expectedSize));
                Assert.That(carrier, Is.EqualTo(expectedCarrier));
            }
        }

        [TestCase("2024-08-01", false)] // Missing size and carrier
        [TestCase("2024-08-01 S", false)] // Missing carrier
        [TestCase("2024-08-01 XX LP", false)] // Invalid package size
        [TestCase("2024-08-01 S XX", false)] // Invalid carrier
        [TestCase("2024-08-32 S LP", false)] // Invalid date
        [TestCase("2024-08-01 S", false)] // Only two parts
        [TestCase("", false)] // Empty line
        public void IsValid_WHenInvalidLines_ShouldReturnFalse(string line, bool expectedValid)
        {
            // Act
            var result = _validator.IsValid(line, out DateTime date, out PackageSize size, out PostalService carrier);

            // Assert
            Assert.That(result, Is.EqualTo(expectedValid));
        }
    }
}