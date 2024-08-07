using VintedExercise.Models;

namespace VintedExercise.Services.Interfaces
{
    public interface IDiscountService
    {
        public IEnumerable<Shipment> ApplyDiscounts(IEnumerable<Shipment> shipments);
    }
}
