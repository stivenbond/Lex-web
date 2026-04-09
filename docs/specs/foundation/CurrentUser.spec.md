# Infrastructure Specification: Current User Service

## 1. Overview
- **Category**: Identity Abstraction
- **Parent Module**: SharedKernel (Core Interface) / Lex.Infrastructure (Implementation)
- **Component Name**: ICurrentUser

## 2. Technical Strategy
- **Base Technology**: ASP.NET Core `HttpContext`, JWT Claims.
- **Role**: Provides a typed bridge between the security context (ClaimsPrincipal) and the application layer.

## 3. Implementation Details
The implementation reads claims from the standard `HttpContext.User` object, typically populated by the Keycloak JWT bearer middleware.

- **Interface Realized**: `ICurrentUser`
- **Classes/Interfaces**:
    - **ICurrentUser**: Interface defining the user identity properties.
    - **CurrentUserService**: (In Lex.Infrastructure) Concrete implementation using `IHttpContextAccessor`.

## 4. Configuration & Settings
- **Required Secrets**: Valid JWT issued by Keycloak.
- **Claims Mapping**:
    - `sub` -> `Id` (GUID)
    - `email` -> `Email`
    - `realm_access.roles` -> `Roles`
    - `permissions` -> `Permissions` (Custom claim)

## 5. Resilience & Performance
- **Lifecycle**: Scoped per request.
- **Caching**: The `ClaimsPrincipal` is already in memory via the `HttpContext`.

## 6. Integration Points
- **Health Checks**: N/A
- **Metrics/Logging**: User ID should be enriched into all structured logs (Serilog).

## 7. Migration & Deployment
- **Post-Deploy Verification**: Login as different roles and verify `HasPermission` logic in a debug endpoint.
