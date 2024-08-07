using VintedAssignment.Helpers.Interfaces;

namespace VintedAssignment.Helpers
{
    public class FileReaderWrapper : IFileReader
    {

        //Wrap static method for easier testing
        public IEnumerable<string> ReadAllLines(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}
