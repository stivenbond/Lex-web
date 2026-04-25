using FluentAssertions;
using Xunit;

namespace Lex.ArchitectureTests;

/// <summary>
/// Ensures the Host project stays wiring-only (no business logic types such as handlers).
/// (Architecture ref §2.3: Host contains zero business logic.)
/// </summary>
public sealed class HostPurityTests
{
    [Fact]
    public void Host_ShouldNotContain_Handlers()
    {
        var host = System.Reflection.Assembly.Load("Lex.API");

        var handlerTypes = host
            .GetTypes()
            .Where(t => t.Name.EndsWith("Handler", StringComparison.Ordinal));

        handlerTypes.Should().BeEmpty("Host must not contain handlers/business logic");
    }

    // Host module dependency rules are enforced via project reference rules and the ModuleBoundaryTests.
}

