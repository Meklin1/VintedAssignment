using VintedExercise.Constants;
using VintedExercise.Models;
using VintedExercise.Services.Interfaces;

namespace VintedExercise.Services.Rules
{
    public class ThirdLShipmentFreeRule : IDiscountRule
    {
        public void Apply(IEnumerable<Shipment> allShipments)
        {
            // Group shipments by year and month for monthly discount application
            var groupedByMonth = allShipments
                .GroupBy(s => new { s.Date.Year, s.Date.Month });

            foreach (var group in groupedByMonth)
            {
                var lpLPackageShipments = group
                    .Where(s => s.Size == PackageSize.L && s.Carrier == PostalService.LP)
                    .OrderBy(s => s.Date)
                    .ToList();

                if (lpLPackageShipments.Count < 3)
                    continue;

                var thirdLShipment = lpLPackageShipments[2];

                // Calculate the remaining discount amount available for the month.
                // This takes into account any discounts that have already been applied
                // to shipments earlier in the current month.
                var monthlyDiscountAmountLeft = DiscountAmount.Max - group.Sum(s => s.DiscountAmount);

                thirdLShipment.DiscountAmount = Math.Min(thirdLShipment.FullPrice, monthlyDiscountAmountLeft);
            }
        }
    }
}
