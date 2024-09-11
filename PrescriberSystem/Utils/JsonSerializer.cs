using System.Text.Json;

namespace PrescriberSystem.Utils;

public class JsonSerializer<T> : ISerializer<T>
{
    public void Serialize(T obj, string filePath)
    {
        var jsonString = JsonSerializer.Serialize(
            obj,
            new JsonSerializerOptions { WriteIndented = true }
        );
        File.WriteAllText(filePath, jsonString);
    }
}
