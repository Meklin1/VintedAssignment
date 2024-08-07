using VintedExercise.Helpers.Interfaces;
using VintedExercise.Models;

namespace VintedExercise.Helpers
{
    namespace VintedExercise.Helpers
    {
        public class Validator : IValidator
        {
            public bool IsValid(string line, out DateTime date, out PackageSize size, out PostalService carrier)
            {
                date = default;
                size = default;
                carrier = default;

                var parts = line.Split(' ');

                if (parts.Length != 3)
                {
                    return false; // Invalid format
                }

                if (!DateTime.TryParse(parts[0], out date))
                {
                    return false; // Invalid date
                }

                if (!Enum.TryParse(parts[1], true, out size))
                {
                    return false; // Invalid package size
                }

                if (!Enum.TryParse(parts[2], true, out carrier))
                {
                    return false; // Invalid shipping provider
                }

                return true;
            }
        }
    }

}
