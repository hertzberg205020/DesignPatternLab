namespace PrescriberSystem.Models;

public sealed class Patient
{
    /// <summary>
    /// 身分證字號 (id) — 開頭為大寫英文字母，之後有 9 位數字。
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// 姓名 (name) — 長度 1~30 的英文字母。
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 性別 (gender) — 男 (male) 或女 (female)
    /// 'M' | 'F'
    /// </summary>
    public char Gender { get; init; }

    /// <summary>
    /// 年紀 (age) — 1~180 的整數
    /// </summary>
    public int Age { get; init; }

    /// <summary>
    /// 身高 (height) — 1~500 的浮點數（單位：公分）
    /// </summary>
    public float Height { get; init; }

    /// <summary>
    /// 體重 (weight) — 1~500 的浮點數 (單位：公斤）
    /// </summary>
    public float Weight { get; init; }

    /// <summary>
    /// 多項病例 (patient’s cases)：為病患在醫院中的所有看診案例紀錄 (case)。
    /// </summary>
    public List<PatientCase> PatientCases { get; set; } = [];
}
