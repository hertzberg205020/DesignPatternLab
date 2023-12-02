using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CardGameFramework.Infra.Extensions;

public static class EnumExtension
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var enumType = enumValue.GetType();
        
        // get the name of the enum value
        var enumName = enumValue.ToString();
        
        var field = enumType.GetField(enumName);

        if (field == null) return enumName;
        
        var displayAttribute =field.GetCustomAttribute<DisplayAttribute>();
        
        return displayAttribute?.Name ?? enumName;
    }
}