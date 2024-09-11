namespace PrescriberSystem.Models;

/// <summary>
/// 診斷結果為一道治療處方 (Prescription)
/// </summary>
public sealed class Prescription
{
    /// <summary>
    /// 名字 (name) — 4~30 個字元
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// 潛在疾病 (potential disease) — 3~100 個字元
    /// </summary>
    public string? PotentialDisease { get; init; }

    /// <summary>
    /// 食用藥物 (medicines) — 包含多個藥物名稱，每個藥物名稱包含 3~30 個字元
    /// </summary>
    public HashSet<string> Medicines { get; init; } = new();

    /// <summary>
    /// 使用方法 (usage) — 長度 0~1000 的字元。
    /// </summary>
    public string? Usage { get; init; }

    public override bool Equals(object? obj)
    {
        if (obj is not Prescription prescription)
        {
            return false;
        }

        return Name == prescription.Name
            && PotentialDisease == prescription.PotentialDisease
            && Medicines.OrderBy(x => x).SequenceEqual(prescription.Medicines.OrderBy(x => x))
            && (Usage ?? string.Empty) == (prescription.Usage ?? string.Empty);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, PotentialDisease, Medicines, Usage);
    }
}
