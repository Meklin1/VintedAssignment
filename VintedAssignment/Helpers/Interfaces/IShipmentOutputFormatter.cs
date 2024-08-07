using VintedExercise.Models;

namespace VintedExercise.Helpers.Interfaces
{
    public interface IShipmentOutputFormatter
    {
        string FormatShipmentOutput(Shipment shipment);
    }
}