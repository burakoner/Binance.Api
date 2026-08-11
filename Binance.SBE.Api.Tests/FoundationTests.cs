using System.Reflection;
using Xunit;

namespace Binance.SBE.Api.Tests;

public class FoundationTests
{
    [Fact]
    public void AssemblyDoesNotPublishPlaceholderClass()
    {
        var assembly = Assembly.Load("Binance.SBE.Api");

        Assert.DoesNotContain(assembly.GetExportedTypes(), type => type.Name == "Class1");
    }
}
