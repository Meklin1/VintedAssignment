using VintedAssignment.Helpers.Interfaces;
using VintedExercise.Constants;
using VintedExercise.Helpers.Interfaces;
using VintedExercise.Models;

namespace VintedExercise.Helpers
{
    namespace VintedExercise.Helpers
    {
        public class FileParser : IFileParser
        {
            private readonly IValidator _validator;
            private readonly IFileReader _reader;

            public FileParser(IValidator validator, IFileReader fileReader)
            {
                _validator = validator;
                _reader = fileReader;
            }

            public IEnumerable<Shipment> ReadShipments(string filePath)
            {
                var shipments = new List<Shipment>();
                var lines = _reader.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    var shipment = ParseLine(line);
                    shipment.Line = line;
                    shipments.Add(shipment);
                }

                return shipments;
            }

            private Shipment ParseLine(string line)
            {
                if (_validator.IsValid(line, out var date, out var size, out var carrier))
                {
                    var shipment = new Shipment
                    {
                        Date = date,
                        Size = size,
                        Carrier = carrier,
                        IsValid = true
                    };

                    shipment.FullPrice = (carrier, size) switch
                    {
                        (PostalService.LP, PackageSize.S) => ShippingPrices.LP_S,
                        (PostalService.LP, PackageSize.M) => ShippingPrices.LP_M,
                        (PostalService.LP, PackageSize.L) => ShippingPrices.LP_L,
                        (PostalService.MR, PackageSize.S) => ShippingPrices.MR_S,
                        (PostalService.MR, PackageSize.M) => ShippingPrices.MR_M,
                        (PostalService.MR, PackageSize.L) => ShippingPrices.MR_L,
                        _ => 0.00
                    };

                    return shipment;
                }

                return new Shipment { IsValid = false };
            }
        }
    }

}
