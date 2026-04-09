# Infrastructure Specification: Permission System Convention

## 1. Overview
- **Category**: Security Policy
- **Parent Module**: Auth & Identity (Foundation)
- **Component Name**: PermissionConstants

## 2. Technical Strategy
- **Base Technology**: Static C# classes with string constants.
- **Role**: Provides a typed, discoverable way to reference permissions across the platform without magic strings.

## 3. Implementation Details
Each module must define its permissions in its `.Core` project to allow other modules (like Notifications) to reference them without circular dependencies.

- **Naming Convention**: `Lex.Module.[ModuleName].Core.Permissions.[ModuleName]Permissions`
- **Constant Format**: `Module:Action` (e.g., `Diary:CreateEntry`)

- **Example Structure**:
```csharp
namespace Lex.Module.DiaryManagement.Core;

public static class DiaryPermissions
{
    public const string ViewEntries = "Diary:ViewEntries";
    public const string CreateEntry = "Diary:CreateEntry";
    public const string ApproveEntry = "Diary:ApproveEntry";
}
```

## 4. Configuration & Settings
- **Reflective Seed**: During startup, a background service scans all projects for these static classes and ensures the permissions exist in Keycloak (optional, but recommended for automation).

## 5. Resilience & Performance
- **Zero Overhead**: Since these are constants, there is no runtime performance cost for accessing them.

## 6. Integration Points
- **ICurrentUser**: The `HasPermission` method checks against these constant strings.
- **Policies**: Handlers use these constants to define their security requirements.

## 7. Migration & Deployment
- **Verification**: Ensure all permissions are prefixed with the module name to avoid collisions across the platform.
