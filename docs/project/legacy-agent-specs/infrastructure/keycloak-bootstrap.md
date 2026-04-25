# Infrastructure Specification: Keycloak Realm Bootstrap

## 1. Overview
- **Category**: Identity Provider Configuration
- **Parent Module**: Host Infrastructure
- **Component Name**: Keycloak Bootstrap

## 2. Technical Strategy
- **Base Technology**: Keycloak 24+
- **Configuration Method**: JSON Realm Export / Import & Keycloak Admin CLI

## 3. Implementation Details
The system requires a pre-configured realm named `Lex`. This spec defines the baseline configuration.

- **Realm Name**: `Lex`
- **Clients**:
    - `lex-web-client`: Public client (OpenID Connect) with PKCE for the SPA.
    - `lex-api-internal`: Confidential client for service-to-service communication (if needed).
- **Roles**:
    - `Teacher`: Can create lessons, manage diaries, grade assessments.
    - `Admin`: Can manage institutions, academic years, and users.
    - `Student`: Can view diaries, take assessments.

## 4. Configuration & Settings
- **Settings Class**: `KeycloakSettings`
- **Key in `appsettings.json`**: `Authentication:Keycloak`
- **Default Admin Account**: `admin` / `admin` (must be changed on first run)

## 5. Resilience & Performance
- **Token Lifetime**: 15 minutes (Access), 30 days (Refresh).
- **Session Management**: Session cookies secured with `SameSite: Lax` and `Secure`.

## 6. Integration Points
- **Health Checks**: Keycloak health check endpoint endpoint `/health`.
- **Claim Mappings**: JWT must include `lex_permissions` claim containing a list of assigned permission strings.

## 7. Migration & Deployment
- **Bootstrap Step**: `install.sh` will copy `lex-realm.json` into the Keycloak import directory.
- **Auto-Import**: Container starts with `--import-realm` flag.
