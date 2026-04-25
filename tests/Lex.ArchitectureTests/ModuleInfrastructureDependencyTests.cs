using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace Lex.ArchitectureTests;

/// <summary>
/// Enforces that Module.*.Infrastructure does not depend on other modules directly.
/// (Architecture ref §2.2: Module.Infrastructure must not reference other Module.* projects.)
/// </summary>
public sealed class ModuleInfrastructureDependencyTests
{
    private static readonly string[] Modules =
    [
        "DiaryManagement", "Scheduling", "LessonManagement",
        "AssessmentCreation", "AssessmentDelivery", "GoogleIntegration",
        "FileProcessing", "ImportExport", "ObjectStorage",
        "Reporting", "Notifications"
    ];

    [Fact]
    public void ModuleInfrastructure_ShouldNotReference_OtherModules()
    {
        foreach (var module in Modules)
        {
            var infraAssembly = GetInfrastructureAssembly(module);

            foreach (var other in Modules.Where(m => m != module).Select(m => $"Lex.Module.{m}"))
            {
                var result = Types.InAssembly(infraAssembly)
                    .ShouldNot().HaveDependencyOn(other).GetResult();

                result.IsSuccessful.Should().BeTrue($"{module}.Infrastructure must not reference {other}");
            }
        }
    }

    private static System.Reflection.Assembly GetInfrastructureAssembly(string module) =>
        System.Reflection.Assembly.Load($"Lex.Module.{module}.Infrastructure");
}

