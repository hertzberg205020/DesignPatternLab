using System.Reflection;

namespace CardGameFramework.Infra.Helpers;

public static class ReflectionHelper
{
    public static (Type, Type) GetConstructorGenericEnumTypes<T>()
    {
        var constructors = typeof(T).GetConstructors();
        ConstructorInfo? targetConstructor = null;

        foreach (var constructor in constructors)
        {
            var parameters = constructor.GetParameters();
            if (parameters.Length == 2 &&
                parameters[0].ParameterType.IsEnum &&
                parameters[1].ParameterType.IsEnum)
            {
                targetConstructor = constructor;
                break;
            }
        }

        if (targetConstructor == null)
            throw new InvalidOperationException(
                "The provided type must have a constructor that takes two Enum parameters.");

        var constructorParams = targetConstructor.GetParameters();

        Type typeU = constructorParams[0].ParameterType;
        Type typeV = constructorParams[1].ParameterType;

        return (typeU, typeV);
    }
}