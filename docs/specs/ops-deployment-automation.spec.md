# Specification: Deployment Automation & Air-Gapped Flows

## 1. Overview
- **Category**: DevOps / Deployment
- **Scope**: Lifecycle scripts for installing, upgrading, and backing up the Lex platform in modular on-prem environments.

## 2. Backup Strategy (`backup.sh`)
- **Scope**: Full state capture.
- **Components**:
    - **Postgres**: `pg_dump` of all module schemas (`*.sql.gz`).
    - **Keycloak**: Real-time export of the realm configuration.
    - **ObjectStorage**: Archive of the `/data/media` and `/data/documents` volumes.
- **Frequency**: Designed to be triggered by a Cron job on the host server.
- **Retention**: Local cycling (Keep last 7 days).

## 3. Upgrade Strategy (`upgrade.sh`)
- **Workflow**:
    1.  **Pull**: Fetch newest container images (or use `.tar` if air-gapped).
    2.  **Pre-Flight**: Run a temporary `MigrationWorker` container to apply DB migrations.
    3.  **Validation**: If migrations fail, the upgrade ABORTS before stopping the current production containers.
    4.  **Rollout**: `docker compose up -d` to recreate containers with the new image tags.
    5.  **Post-Flight**: Verify `/readyz` success.

## 4. Air-Gapped Installation Flow
- **Philosopy**: Zero internet dependency after initial download of the release bundle.
- **Standard**:
    - Release ZIP includes `images/*.tar` (output of `docker save`).
    - `install.sh` uses `docker load -i` to populate the host's image cache.
    - Application configuration defaults to "Local-First" (e.g., local storage instead of S3).
- **Manual Tasks**: The institution's IT admin must manually provision the SSL certificate (Pfx/Pem) since Let's Encrypt will not reach the internal network.

## 5. Migration History
- The `Host.API` maintains an audit log of applied migrations in the `infra.migration_history` table, including the Git SHA associated with the build.
