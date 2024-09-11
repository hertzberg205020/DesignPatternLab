using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using CsvHelper;
using PrescriberSystem.Models;

namespace PrescriberSystem.Utils;

public class DtoSerializer
{
    public static async Task SerializeAsync<T>(
        T obj,
        string basePath,
        string fileName,
        ExportFormat format
    )
    {
        if (format.HasFlag(ExportFormat.Json))
        {
            var jsonPath = Path.Combine(basePath, $"{fileName}.json");
            await using FileStream fs = File.Create(jsonPath);
            await JsonSerializer.SerializeAsync(
                fs,
                obj,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }
            );
        }

        if (format.HasFlag(ExportFormat.Csv))
        {
            var csvPath = Path.Combine(basePath, $"{fileName}.csv");
            await using (var writer = new StreamWriter(csvPath))
            {
                await using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    await csv.WriteRecordsAsync(new[] { obj });
                }
            }
        }
    }

    // 保留同步版本以便向後兼容
    public static void Serialize<T>(T obj, string basePath, string fileName, ExportFormat format)
    {
        SerializeAsync(obj, basePath, fileName, format).GetAwaiter().GetResult();
    }
}
