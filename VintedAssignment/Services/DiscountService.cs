using VintedExercise.Models;
using VintedExercise.Services.Interfaces;

namespace VintedExercise.Services
{

    public class DiscountService : IDiscountService
    {
        private readonly IEnumerable<IDiscountRule> _rules;

        public DiscountService(IEnumerable<IDiscountRule> rules)
        {
            _rules = rules;
        }

        public IEnumerable<Shipment> ApplyDiscounts(IEnumerable<Shipment> shipments)
        {
            var validshipments = shipments.Where(s => s.IsValid);
            foreach (var rule in _rules)
            {
                rule.Apply(validshipments);
            }
            return shipments;
        }
    }

}
