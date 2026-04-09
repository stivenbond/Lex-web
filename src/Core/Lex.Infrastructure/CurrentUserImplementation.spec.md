# Infrastructure Specification: Current User Implementation

## 1. Overview
- **Category**: Security Infrastructure
- **Parent Module**: Auth & Identity (Infrastructure)
- **Component Name**: CurrentUserImplementation

## 2. Technical Strategy
- **Base Technology**: `IHttpContextAccessor`, ASP.NET Core Security.
- **Role**: Concrete implementation of the `ICurrentUser` interface defined in `SharedKernel`.

## 3. Implementation Details
The service extracts claims from the `HttpContext.Items` or `HttpContext.User` properties which are populated by the JWT Bearer middleware.

- **Interface Realized**: `ICurrentUser`
- **Classes/Interfaces**:
    - **CurrentUserService**: Implements `ICurrentUser`.
- **Property Logic**:
    - `Id`: `Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))`.
    - `Roles`: `User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray()`.
    - `Permissions`: Custom claim `permissions` split by space or comma.

## 4. Configuration & Settings
- **Service Registration**: Registered as `Scoped` in `Lex.Infrastructure`.
- **Dependencies**: Requires `AddHttpContextAccessor()` to be called in `Program.cs`.

## 5. Resilience & Performance
- **Wait Policy**: If accessed outside of an HTTP request (e.g., in a background job), it should return logical defaults (e.g., `System` user or `IsAuthenticated = false`) or throw a specific exception if context is mandatory.

## 6. Integration Points
- **MediatR Handlers**: Injected into any handler requiring user context for authorization or auditing.

## 7. Migration & Deployment
- **Verification**: Unit test `CurrentUserService` with a mocked `ClaimsPrincipal`.
