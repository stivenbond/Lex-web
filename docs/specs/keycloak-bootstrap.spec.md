# Infrastructure Specification: Keycloak Bootstrap (install.sh)

## 1. Overview
- **Category**: Deployment / Orchestration
- **Parent Module**: Infrastructure
- **Component Name**: Keycloak Bootstrap Step

## 2. Technical Strategy
- **Base Technology**: Bash, `docker compose`, Keycloak CLI (`kcadm.sh`).
- **Storage/Transport**: Environment variables from `.env`, Realm configuration export.

## 3. Implementation Details
The `install.sh` script is responsible for ensuring the Keycloak instance is not only running but also correctly configured for the first use.

- **Wait for Readiness**: The script must poll the Keycloak `/health/ready` endpoint before attempting configuration.
- **Admin Authentication**: Authenticate using the `KEYCLOAK_ADMIN_PASSWORD` provided in `.env`.
- **Realm Import**:
    - If the `lex` realm does not exist, use `kcadm.sh` to import `infra/keycloak/realm-lex.json`.
- **Default User Creation**:
    - Create an initial `Admin` user (if not defined in the JSON).
    - Create a test `Teacher` and `Student` user for development/staging environments.
- **Credential Management**:
    - Ensure all default passwords are flagged for "Required Action: Update Password" on first login.

## 4. Configuration & Settings
- **Script**: `infra/scripts/install.sh`
- **Metadata**: `infra/keycloak/realm-lex.json`
- **Secrets**: `KEYCLOAK_ADMIN_PASSWORD`

## 5. Resilience & Performance
- **Timeout**: The script should wait up to 120 seconds for Keycloak to start (it is a heavy container).
- **Idempotency**: The script must check for the existing realm/users before attempting to create them to avoid errors on re-runs.

## 6. Integration Points
- **Health Checks**: Uses `docker compose ps` and `curl`.
- **Logs**: Output setup progress to `stdout` for the operator.

## 7. Migration & Deployment
- **Verification**: Ensure the `lex-web` and `lex-api` clients can receive tokens after the script completes.
