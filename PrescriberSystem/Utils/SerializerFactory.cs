namespace PrescriberSystem.Utils;

public class SerializerFactory
{
    public static ISerializer<T> GetSerializer<T>(string format)
    {
        if (string.Equals(format, "json", StringComparison.OrdinalIgnoreCase))
        {
            return new JsonSerializer<T>();
        }

        if (string.Equals(format, "csv", StringComparison.OrdinalIgnoreCase))
        {
            return new CsvSerializer<T>();
        }

        throw new ArgumentException("Unsupported format", nameof(format));
    }
}
