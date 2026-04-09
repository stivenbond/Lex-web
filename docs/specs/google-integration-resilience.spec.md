# Specification: Google Integration Resilience & Security

## 1. Overview
- **Category**: Infrastructure / Reliability
- **Parent Module**: GoogleIntegration
- **Purpose**: Ensure the Lex platform remains stable and responsive even when external Google APIs are slow, unreachable, or have exceeded quotas.

## 2. Resilience Patterns (Polly)

### Circuit Breaker
- **Threshold**: If 5 consecutive requests to `google-api.com` fail with `5xx` or `RequestTimeout`, open the circuit.
- **Behavior**: While the circuit is Open, all requests to `GoogleIntegration` will fail-fast with a specific error (`ServiceUnavailableException`). This prevents thread exhaustion in the host application.
- **Recovery**: Transition to Half-Open after 60 seconds to attempt a "canary" request.

### Retry Policy
- **Type**: Exponential Backoff.
- **Conditions**: Retry on `429 Too Many Requests` or `502/503/504`.
- **Max Retries**: 3 attempts before moving to a Dead Letter Queue (for background jobs) or returning an error to the user (for UI actions).

## 3. Security Patterns

### Encryption of Tokens
- **Provider**: `Microsoft.AspNetCore.DataProtection`.
- **Scope**: Per-user encryption.
- **Key Rotation**: Managed by the host infrastructure.
- **Requirement**: Plain-text tokens MUST NOT be stored in logs or transmitted in the application body except to the Google API endpoint.

### Scoping
- **Principle of Least Privilege**: 
    - Calendar: `calendar.events` (NOT full calendar access).
    - Drive: `drive.file` (Access only to files opened/created by Lex).

## 4. Air-Gapped Mode
- **Detection**: On module startup, a basic health check ping to `google.com` is performed.
- **Behavior**: If unreachable, the module enters "Local Mode". Dashboard buttons for Google Sync/Import are automatically disabled (via the `usePermissions` hook checking a system-wide `GoogleEnabled` flag).

## 5. Monitoring
- **Alerting**: Emit a `GoogleIntegrationFailureCounter` metric for Prometheus.
- **Logging**: Mask all PII (Emails) and tokens in Seq logs.
