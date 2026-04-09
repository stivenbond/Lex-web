# Infrastructure Specification: Keycloak Realm Bootstrap

## 1. Overview
- **Category**: Identity Provider Configuration / External Integration
- **Parent Module**: Infrastructure (Foundation)
- **Component Name**: Keycloak Realm Bootstrap

## 2. Technical Strategy
- **Base Technology**: Keycloak 24+
- **Storage/Transport**: Realm Export JSON (Imported on first run)
- **Protocol**: OpenID Connect (OIDC) with OAuth 2.0 (PKCE for SPA)

## 3. Implementation Details
The realm configuration is maintained as a JSON export (`infra/keycloak/realm-lex.json`). This file is imported during the `install.sh` / `bootstrap.sh` process.

- **Realm Name**: `lex`
- **Clients**:
    - **lex-web**: 
        - Root URL: `http://localhost:3000`
        - Redirect URIs: `http://localhost:3000/*`
        - Access Type: Public
        - Flow: Standard Flow with PKCE
    - **lex-api**:
        - Access Type: Confidential (or Bearer-only)
        - Audience mapping enabled for the `lex` audience.

- **Roles (Global)**:
    - `AppAdmin`: Root system access.
    - `Teacher`: Manage lessons, diary, and assessments.
    - `Student`: View schedule, submit assessments.
    - `InstitutionAdmin`: Manage users and institutional settings.

- **Claim Mappings (JWT)**:
    - `roles`: Standard realm roles.
    - `permissions`: Custom claim mapped from role attributes or custom scopes (see Policy Mapping spec).
    - `tenant_id`: For future multi-tenancy support.

## 4. Configuration & Settings
- **Settings Class**: `KeycloakSettings` in `Lex.Infrastructure`
- **Key in `appsettings.json`**: `Keycloak`
- **Required Secrets**: 
    - `KEYCLOAK_ADMIN_PASSWORD` (Initial setup)
    - `LEX_API_CLIENT_SECRET` (For confidential client)

## 5. Resilience & Performance
- **Caching Strategy**: Keycloak internal Infinispan cache.
- **Token Model**: Short-lived Access Tokens (5m), Long-lived Refresh Tokens (30d).

## 6. Integration Points
- **Health Checks**: Keycloak readiness check via `/health/ready`.
- **Metrics/Logging**: Standard Keycloak logs (JSON format).

## 7. Migration & Deployment
- **Post-Deploy Verification**:
    - Verify `http://localhost:8080/realms/lex/.well-known/openid-configuration` is accessible.
    - Verify successful login via `lex-web` using a test account.
