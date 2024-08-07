using VintedExercise.Models;

namespace VintedExercise.Services.Interfaces
{
    public interface IDiscountRule
    {
        void Apply(IEnumerable<Shipment> allShipments);
    }
}
