
namespace VintedAssignment.Helpers.Interfaces
{
    public interface IFileReader
    {
        IEnumerable<string> ReadAllLines(string path);
    }
}