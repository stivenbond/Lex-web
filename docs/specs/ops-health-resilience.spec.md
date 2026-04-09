# Specification: Ops Health & Resilience

## 1. Overview
- **Category**: Operations / Stability
- **Scope**: Liveness and readiness monitoring logic for the modular monolith process.

## 2. Readiness Check Architecture (`/readyz`)
- **Philosophy**: The system is ready only when all module-critical infrastructure is reachable.
- **Dependencies**:
    - **Postgres**: Successful `SELECT 1` on the main connection.
    - **RabbitMQ**: Active connection from the MassTransit bus.
    - **Keycloak**: Successful discovery document fetch (`/.well-known/openid-configuration`).
    - **ObjectStorage**: Basic connectivity check to the Local Volume or S3.

## 3. Module-Specific Readiness
- Modules may register specific readiness predicates:
    - **ImportExport**: Verifies the temporary scratch directory is writable.
    - **GoogleIntegration**: Verifies encrypted data keys are initialized.

## 4. Message Bus Resilience (MassTransit)
- **DLQ Monitoring**:
    - **Logic**: A background service monitors the depth of all `_error` queues in RabbitMQ.
    - **Health Mapping**: If any DLQ depth > 10, report a `Degraded` health status.
    - **Alerting**: The console log emits a `CRITICAL` signal if a message enters a DLQ, captured by Seq alerting.

## 5. Startup Sequence (Orchestration)
- **The "Migration Barrier"**: The API process will block its HTTP ports until all EF Core migrations have been successfully applied.
- **SignalR**: Hub connections are only accepted once the Readiness check passes.

## 6. Liveness Strategy (`/healthz`)
- Minimal check to verify the process is not deadlocked. 
- Includes a basic check of the thread-pool health.
