# Infrastructure Specification: Installation & Bootstrap Script

## 1. Overview
- **Category**: Deployment / DevOps
- **Parent Module**: Foundation (Lifecycle)
- **Component Name**: install.sh

## 2. Technical Strategy
- **Base Technology**: Bash (Linux/MacOS), PowerShell (Windows).
- **Role**: Automates the first-run configuration of the entire ecosystem (Docker, Keycloak, Postgres, ObjectStorage).

## 3. Implementation Details
The script performs a series of idempotent setup steps.

- **Steps**:
    1. **Dependency Check**: Verify Docker and Docker Compose presence.
    2. **Network Setup**: Create the internal Docker bridge network.
    3. **Database Bootstrap**: Run initial PostgreSQL migrations.
    4. **Keycloak Setup**: 
        - Spin up Keycloak.
        - Create the `Lex` realm if it doesn't exist.
        - Import `realm-export.json`.
        - Create the initial admin account.
    5. **Volume Initialization**: Create named volumes for Postgres, Keycloak, and S3 data.

## 4. Configuration & Settings
- **Variables**: Resolved from `.env` template.
- **Secrets**: Generates random passwords for `POSTGRES_PASSWORD` and `KEYCLOAK_ADMIN_PASSWORD` if not provided.

## 5. Resilience & Performance
- **Idempotency**: Every command must be safe to run multiple times without failure or data duplication.
- **Timeouts**: Script should wait for services to be "Healthy" (using `docker inspect`) before proceeding to the next step.

## 6. Integration Points
- **Health Checks**: Uses `/healthz` endpoints of the API and Keycloak.

## 7. Migration & Deployment
- **Verification**: The last step must trigger a "Success" message if the frontend is reachable on the expected port.
