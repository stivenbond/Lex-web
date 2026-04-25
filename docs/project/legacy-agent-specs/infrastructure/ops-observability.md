# Infrastructure Specification: Operations & Observability

## 1. Overview
- **Category**: DevOps / SRE
- **Parent Module**: Infrastructure
- **Component Name**: Observability Stack & Scripts

## 2. Technical Strategy
- **Base Technology**: Serilog, OpenTelemetry, Health Checks, Shell scripts.
- **Monitoring Tool**: Seq (for logs/traces), Prometheus (for metrics).

## 3. Implementation Details

- **Business Metrics**:
    - Every module exposes metrics following `module.{name}.{metric}` convention.
    - Example: `module.billing.invoices_created_total`.
- **Health Checks**:
    - `/healthz`: Liveness.
    - `/readyz`: Readiness (DB, MQ, Redis connectivity).
    - **Custom Checklist**: Each module registers a readiness check for its specific DB schema and queue connectivity.

## 4. Operational Scripts

- **`backup.sh`**:
    - Triggers `pg_dump` specifically for each module schema.
    - Exports the Keycloak realm configuration.
    - Packs data volumes for Seq and ObjectStorage.
- **`upgrade.sh`**:
    - Orchestrates the zero-downtime migration (where possible).
    - Runs "Pre-upgrade" DB migrations.
    - Restarts containers with the new image versions.
    - Runs "Post-upgrade" verification jobs.

## 5. Deployment Models
- **Air-Gapped Install**:
    - A script to pre-load Docker images from a USB/Registry.
    - Pre-baked `.env` configuration for offline environments (no external dependencies).

## 6. Resilience
- **Dead-Letter Monitoring**: Health checks alert if any MassTransit dead-letter queue depth > 0.
- **Audit Logging**: All write operations through the API recorded to a tamper-evident audit log table.
