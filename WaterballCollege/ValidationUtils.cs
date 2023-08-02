namespace WaterballCollege;

public static class ValidationUtils
{
    public static string LengthShouldBetween(string value, int min, int max)
    {
        if (value.Length < min || value.Length > max)
        {
            throw new ArgumentException($"長度必須介於 {min} ~ {max} 之間");
        }
        return value;
    }
}
