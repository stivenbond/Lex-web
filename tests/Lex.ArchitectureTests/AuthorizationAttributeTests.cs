using FluentAssertions;
using Xunit;

namespace Lex.ArchitectureTests;

/// <summary>
/// Enforces that all API controllers require authorization by default.
/// (Architecture ref §11.3: "All public API endpoints must require authorisation".)
/// </summary>
public sealed class AuthorizationAttributeTests
{
    [Fact]
    public void Controllers_ShouldHaveAuthorizeAttribute()
    {
        // Controllers live in module infrastructure assemblies.
        var infraAssemblies = new[]
        {
            "Lex.Module.DiaryManagement.Infrastructure",
            "Lex.Module.Scheduling.Infrastructure",
            "Lex.Module.AssessmentDelivery.Infrastructure",
            "Lex.Module.AssessmentCreation.Infrastructure",
            "Lex.Module.FileProcessing.Infrastructure",
            "Lex.Module.GoogleIntegration.Infrastructure",
            "Lex.Module.ImportExport.Infrastructure",
            "Lex.Module.LessonManagement.Infrastructure",
            "Lex.Module.ObjectStorage.Infrastructure",
            "Lex.Module.Notifications.Infrastructure",
            "Lex.Module.Reporting.Infrastructure"
        }.Select(System.Reflection.Assembly.Load);

        const string apiControllerAttribute = "Microsoft.AspNetCore.Mvc.ApiControllerAttribute";
        const string authorizeAttribute = "Microsoft.AspNetCore.Authorization.AuthorizeAttribute";

        foreach (var assembly in infraAssemblies)
        {
            var controllerTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(inherit: true).Any(a => a.GetType().FullName == apiControllerAttribute))
                .ToList();
            // Some modules may legitimately expose no controllers yet.
            // In that case this rule is not applicable to that assembly.
            if (controllerTypes.Count == 0)
            {
                continue;
            }

            var missingAuthorize = controllerTypes
                .Where(t => !t.GetCustomAttributes(inherit: true).Any(a => a.GetType().FullName == authorizeAttribute))
                .Select(t => t.FullName)
                .ToList();

            missingAuthorize.Should().BeEmpty($"{assembly.GetName().Name}: all [ApiController] controllers must be [Authorize]");
        }
    }
}

