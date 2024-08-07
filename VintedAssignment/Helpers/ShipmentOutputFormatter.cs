using VintedExercise.Helpers.Interfaces;
using VintedExercise.Models;

namespace VintedExercise.Helpers
{
    public class ShipmentOutputFormatter : IShipmentOutputFormatter
    {
        public string FormatShipmentOutput(Shipment shipment)
        {
            if (shipment.IsValid)
            {
                double reducedPrice = shipment.FullPrice - shipment.DiscountAmount;
                string discountDisplay = shipment.DiscountAmount > 0 ? $"{shipment.DiscountAmount:F2}" : "-";
                return $"{shipment.Line} {reducedPrice:F2} {discountDisplay}";
            }
            else
            {
                return $"{shipment.Line} Ignored";
            }
        }
    }
}
