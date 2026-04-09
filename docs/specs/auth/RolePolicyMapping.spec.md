# Infrastructure Specification: Role-to-Policy Mapping

## 1. Overview
- **Category**: Authorization Strategy
- **Parent Module**: Auth & Identity (Infrastructure)
- **Component Name**: PolicyAuthorizer

## 2. Technical Strategy
- **Base Technology**: ASP.NET Core Authorization Policies.
- **Role**: Translates Keycloak role and permission claims into reusable application policies.

## 3. Implementation Details
The mapping is registered in `Lex.Infrastructure` during service configuration.

- **Default Policies**:
    - `RequireAdmin`: Requires the `Admin` role claim.
    - `RequireTeacher`: Requires the `Teacher` role claim.
    - `RequireStudent`: Requires the `Student` role claim.
- **Dynamic Permission Policies**:
    - A custom `IAuthorizationRequirement` that checks if the `CurrentUser` has a specific permission constant string.

## 4. Configuration & Settings
- **Registration**:
```csharp
services.AddAuthorization(options => {
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    // Add dynamic permission handler registration
});
```

## 5. Resilience & Performance
- **Caching**: ASP.NET Core caches requirements evaluation for the duration of the request.

## 6. Integration Points
- **Attributes**: `[Authorize(Policy = "AdminOnly")]` used on Minimal API delegates or Hub methods.

## 7. Migration & Deployment
- **Verification**: Ensure that adding a role in Keycloak and assigning it to a user correctly unlocks the corresponding policy in the API.
