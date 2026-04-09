# Infrastructure Specification: Auth Middleware & Identity

## 1. Overview
- **Category**: Cross-cutting Infrastructure
- **Parent Module**: Infrastructure
- **Component Name**: IdentityProvider / ICurrentUser

## 2. Technical Strategy
- **Base Technology**: ASP.NET Core Authentication, JWT Bearer, Claims Transformation.

## 3. Implementation Details
Transforms the JWT claims into a typed `ICurrentUser` service available via DI.

- **Interface Realized**: `ICurrentUser` (Defined in SharedKernel)
- **Role-to-Policy Mapping**:
    - Policy `RequireTeacher`: Requires role `Teacher` or permission `teacher.access`.
    - Policy `RequireAdmin`: Requires role `Admin` or permission `admin.access`.
    - Policy `RequireStudent`: Requires role `Student`.

## 4. Configuration & Settings
- **Settings Class**: `AuthSettings`
- **Key in `appsettings.json`**: `Authentication`

## 5. Resilience & Performance
- **Caching**: `ICurrentUser` is Scoped to the request. No cross-request caching needed.

## 6. Integration Points
- **Logging**: Enriches Serilog context with `UserId` and `TenantId` from the current user.
- **Auditing**: Automatically used by `AuditEvent` interceptors in EF Core to track `CreatedBy` / `ModifiedBy`.

## 7. Migration & Deployment
- Applied globally in `YourApp.Infrastructure`.
