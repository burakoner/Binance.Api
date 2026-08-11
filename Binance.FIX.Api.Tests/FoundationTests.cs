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
}
