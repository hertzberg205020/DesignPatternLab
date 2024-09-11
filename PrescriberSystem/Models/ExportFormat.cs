namespace PrescriberSystem.Models;

/// <summary>
/// // 2 進制
/// </summary>
[Flags]
public enum ExportFormat
{
    Json = 0x1,
    Csv = 0x2,
    Both = Json | Csv,
}
