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

    public static decimal ShouldBeLargerThan(decimal value, decimal target)
    {
        if (value <= target)
        {
            throw new ArgumentException($"必須大於 {target}");
        }
        return value;
    }

    public static int ShouldBePositive(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentException($"必須大於 0");
        }
        return value;
    }

    public static T RequiredNonNull<T>(T value) where T : class
    {
        if (value == null)
        {
            throw new ArgumentException($"不可為空");
        }
        return value;
    }
}
