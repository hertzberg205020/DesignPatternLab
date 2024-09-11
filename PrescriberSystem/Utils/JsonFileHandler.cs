using System.Text.Json;

namespace PrescriberSystem.Utils;

public static class JsonFileHandler
{
    public static async Task SerializeToJsonFileAsync<T>(T obj, string filePath)
    {
        try
        {
            // 序列化物件為 JSON 字串
            var jsonString = JsonSerializer.Serialize(
                obj,
                new JsonSerializerOptions { WriteIndented = true }
            );

            // 將 JSON 字串寫入指定檔案
            await File.WriteAllTextAsync(filePath, jsonString);
            Console.WriteLine("Data has been successfully serialized to JSON file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
        }
    }

    // 從指定的 JSON 檔案讀取並反序列化為物件
    public static async Task<T?> DeserializeFromJsonFileAsync<T>(string filePath)
    {
        try
        {
            // 檢查檔案是否存在
            if (!File.Exists(filePath))
            {
                Console.WriteLine("The specified file does not exist.");
                return default;
            }

            // 從檔案讀取 JSON 字串
            var jsonString = await File.ReadAllTextAsync(filePath);

            // 將 JSON 字串反序列化為物件
            T? obj = JsonSerializer.Deserialize<T>(jsonString);
            Console.WriteLine("Data has been successfully deserialized from JSON file.");
            return obj;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            return default;
        }
    }
}
