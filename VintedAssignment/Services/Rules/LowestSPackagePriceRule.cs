using VintedExercise.Constants;
using VintedExercise.Models;
using VintedExercise.Services.Interfaces;

namespace VintedExercise.Services.Rules
{
    public class LowestSPackagePriceRule : IDiscountRule
    {
        public void Apply(IEnumerable<Shipment> allShipments)
        {
            var minSPackagePrice = allShipments
                .Where(s => s.Size == PackageSize.S)
                .Min(s => s.FullPrice);

            // Group shipments by year and month for monthly discount application
            var groupedByMonth = allShipments
                .GroupBy(s => new { s.Date.Year, s.Date.Month });

            foreach (var group in groupedByMonth)
            {
                var smallPackageShipments = group.Where(s => s.Size == PackageSize.S);

                // Calculate the remaining discount amount available for the month.
                // This takes into account any discounts that have already been applied
                // to shipments earlier in the current month.
                var monthlyDiscountAmountLeft = DiscountAmount.Max - group.Sum(s => s.DiscountAmount);

                foreach (var shipment in smallPackageShipments)
                {
                    // If there are not enough funds to fully cover a discount this calendar month, only a part is covered
                    var discount = Math.Min(shipment.FullPrice - minSPackagePrice, monthlyDiscountAmountLeft);

                    shipment.DiscountAmount = discount;
                    monthlyDiscountAmountLeft -= discount;

                    if (monthlyDiscountAmountLeft <= 0)
                        break;
                }
            }
        }
    }
}
