using VintedAssignment.Helpers;
using VintedExercise.Constants;
using VintedExercise.Helpers;
using VintedExercise.Helpers.VintedExercise.Helpers;
using VintedExercise.Services;
using VintedExercise.Services.Interfaces;
using VintedExercise.Services.Rules;

namespace VintedExercise
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string filePath = args.Length > 0 ? args[0] : InputFile.DefaultFilePath;

                var validator = new Validator();
                var fileReader = new FileReaderWrapper();
                var fileParser = new FileParser(validator, fileReader);
                var formatter = new ShipmentOutputFormatter();

                // Define and apply discount rules
                var rules = new List<IDiscountRule>
                {
                    new LowestSPackagePriceRule(),
                    new ThirdLShipmentFreeRule(),
                };
                IDiscountService discountService = new DiscountService(rules);

                var shipments = fileParser.ReadShipments(filePath);
                var processedShipments = discountService.ApplyDiscounts(shipments);

                foreach (var shipment in processedShipments)
                {
                    Console.WriteLine(formatter.FormatShipmentOutput(shipment));
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: File not found. {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error: File reading error. {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
