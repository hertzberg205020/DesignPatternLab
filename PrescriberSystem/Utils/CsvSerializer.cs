using System.Globalization;
using CsvHelper;

namespace PrescriberSystem.Utils;

public class CsvSerializer<T> : ISerializer<T>
{
    public void Serialize(T obj, string filePath)
    {
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(new[] { obj });
    }
}
