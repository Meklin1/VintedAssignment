using VintedExercise.Models;

namespace VintedExercise.Helpers.Interfaces
{
    public interface IFileParser
    {
        IEnumerable<Shipment> ReadShipments(string filePath);
    }
}