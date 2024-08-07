using VintedExercise.Models;

namespace VintedExercise.Helpers.Interfaces
{
    public interface IValidator
    {
        bool IsValid(string line, out DateTime date, out PackageSize size, out PostalService carrier);
    }
}