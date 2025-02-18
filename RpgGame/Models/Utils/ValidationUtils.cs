namespace RpgGame.Models.Utils;

public static class ValidationUtils
{
    public static T ShouldNotBeNull<T>(string name, T? obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(name);
        }

        return obj;
    }

    public static int ShouldNotBeNegative(int value, string errorMsg = "should not be negative")
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(errorMsg);
        }

        return value;
    }

    public static int ShouldBeGreaterThanOrEqual(string name, int num, int target)
    {
        if (num < target)
        {
            throw new ArgumentOutOfRangeException(name);
        }

        return num;
    }

    /// <summary>
    /// the value should be within the range [inclusiveMin, inclusiveMax)
    /// </summary>
    /// <param name="name">the name of verified value</param>
    /// <param name="num">the value to be verified</param>
    /// <param name="inclusiveMin">the minimum value (inclusive)</param>
    /// <param name="inclusiveMax">the maximum value (exclusive)</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static int ShouldBeWithinRange(string name, int num, int inclusiveMin, int inclusiveMax)
    {
        if (num < inclusiveMin || num >= inclusiveMax)
        {
            throw new ArgumentOutOfRangeException(name);
        }

        return num;
    }
}
