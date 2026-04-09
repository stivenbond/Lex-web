# Infrastructure Specification: Auth Policy Mapping & Permission Conventions

## 1. Overview
- **Category**: Security / Foundational Infrastructure
- **Parent Module**: Infrastructure
- **Component Name**: Auth Policy Mapping

## 2. Technical Strategy
- **Base Technology**: ASP.NET Core Authorization Policies
- **Storage/Transport**: In-memory mapping (Hardcoded or Configuration-driven)
- **Convention**: Static permission classes in each module's `.Core` project.

## 3. Implementation Details

### Permission Constants Convention
Every module must define its permissions in a static class within its `Core` project:
```csharp
namespace Lex.Module.DiaryManagement;

public static class DiaryPermissions
{
    private const string Prefix = "diary";
    public const string View = $"{Prefix}.view";
    public const string Create = $"{Prefix}.create";
    public const string Edit = $"{Prefix}.edit";
    public const string Delete = $"{Prefix}.delete";
    public const string Approve = $"{Prefix}.approve";
}
```

### Role-to-Policy Mapping
In `Lex.Infrastructure`, we map Keycloak roles to ASP.NET Core policies. This allows us to use `[Authorize(Policy = "TeacherOnly")]` or more specific permission-based policies.

- **Interface Realized**: `IAuthorizationPolicyProvider` (Standard ASP.NET Core)
- **Classes**:
    - **`LexPolicyProvider`**: Dynamic policy provider that handles string-formatted permission policies (e.g., `[Authorize(Policy = DiaryPermissions.View)]`).
    - **`PermissionHandler`**: Checks if the `ICurrentUser` has the required permission claim.

### Default Policy Mappings
| Role | Associated Policies |
| :--- | :--- |
| `AppAdmin` | `AdminOnly`, and bypass for all permission checks. |
| `Teacher` | `TeacherOnly`, and specific permissions (`diary.create`, `lessons.manage`). |
| `Student` | `StudentOnly`, and specific permissions (`diary.view`, `assessment.submit`). |

## 4. Configuration & Settings
- **Settings Class**: `AuthorizationSettings`
- **Key in `appsettings.json`**: `Authorization`

## 5. Resilience & Performance
- **Caching Strategy**: User claims are cached in the `ClaimsPrincipal` for the duration of the request.
- **Optimization**: Use `ICurrentUser` service to avoid multiple JWT parsing.

## 6. Integration Points
- **Health Checks**: None.
- **Metrics/Logging**: Log unauthorized access attempts (403 Forbidden) with role/permission context.

## 7. Migration & Deployment
- **Database Migrations**: None.
- **Post-Deploy Verification**:
    - Verify that a user with `Teacher` role cannot access `AdminOnly` endpoints.
    - Verify that `diary.create` permission is correctly enforced on the Diary API.
