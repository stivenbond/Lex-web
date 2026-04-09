# Infrastructure Specification: Keycloak Realm Bootstrap

## 1. Overview
- **Category**: Identity Provider Configuration
- **Parent Module**: Auth & Identity (Foundation)
- **Component Name**: KeycloakRealm

## 2. Technical Strategy
- **Base Technology**: Keycloak, JSON Realm Export.
- **Role**: Defines the security baseline for the platform, including default roles and client configurations.

## 3. Implementation Details
The realm configuration is stored as a JSON file (`realm-export.json`) in the `infra/auth` directory. It is imported automatically when the Keycloak container starts.

- **Client Configuration**:
    - `lex-web-client`: Public client (PKCE) for the frontend.
    - `lex-api`: Confidential client (Service Account) for cross-module or background tasks.
- **Default Roles**:
    - `Admin`: Full system access.
    - `Teacher`: Manage lessons, assessments, and diary entries.
    - `Student`: Access lessons, submit assessments.

## 4. Configuration & Settings
- **Realm Name**: `Lex`
- **Frontend URL**: `http://localhost:3000` (Dev)
- **Token Lifetimes**:
    - Access Token: 5m
    - Refresh Token: 30d (Offline Access allowed)

## 5. Resilience & Performance
- **High Availability**: Keycloak is stateless, relying on the `postgres` DB for session and realm state.
- **Export/Import**: Regular exports during development ensure the repo contains the latest roles and permission mappings.

## 6. Integration Points
- **OIDC**: Standard OpenID Connect endpoints (`/realms/Lex/.well-known/openid-configuration`).

## 7. Migration & Deployment
- **Bootstrap**: The `install.sh` script verifies Keycloak reachability before proceeding.
- **Verification**: Verify that the `Teacher` role can access protected endpoints after login.
