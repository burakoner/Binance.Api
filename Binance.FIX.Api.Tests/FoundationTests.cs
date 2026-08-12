using System.Reflection;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class FoundationTests
{
    [Fact]
    public void AssemblyDoesNotPublishPlaceholderClass()
    {
        var assembly = Assembly.Load("Binance.FIX.Api");

        Assert.DoesNotContain(assembly.GetExportedTypes(), type => type.Name == "Class1");
    }

    [Fact]
    public void PublicApiDoesNotExposeQuickFixTypes()
    {
        var exportedTypes = Assembly.Load("Binance.FIX.Api").GetExportedTypes();

        var publicMembers = exportedTypes.SelectMany(type =>
            type.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public));

        Assert.DoesNotContain(publicMembers, member => GetExposedTypes(member).Any(IsQuickFixType));
    }

    private static IEnumerable<Type> GetExposedTypes(MemberInfo member)
        => member switch
        {
            ConstructorInfo constructor => constructor.GetParameters().Select(parameter => parameter.ParameterType),
            MethodInfo method => method.GetParameters()
                .Select(parameter => parameter.ParameterType)
                .Append(method.ReturnType),
            PropertyInfo property => [property.PropertyType],
            FieldInfo field => [field.FieldType],
            EventInfo eventInfo when eventInfo.EventHandlerType is not null => [eventInfo.EventHandlerType],
            _ => []
        };

    private static bool IsQuickFixType(Type type)
        => IsQuickFixType(type, []);

    private static bool IsQuickFixType(Type type, HashSet<Type> visitedTypes)
    {
        if (!visitedTypes.Add(type))
        {
            return false;
        }

        if (type.IsByRef || type.IsArray || type.IsPointer)
        {
            return IsQuickFixType(type.GetElementType()!, visitedTypes);
        }

        if (type.IsGenericType
            && type.GetGenericArguments().Any(argument => IsQuickFixType(argument, visitedTypes)))
        {
            return true;
        }

        return type.Namespace?.StartsWith("QuickFix", StringComparison.Ordinal) is true
            || type.BaseType is not null && IsQuickFixType(type.BaseType, visitedTypes)
            || type.GetInterfaces().Any(@interface => IsQuickFixType(@interface, visitedTypes));
    }
}
