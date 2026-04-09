# Specification: Integration & E2E Testing Strategy

## 1. Overview
- **Category**: Quality Assurance
- **Scope**: Definition of the multi-layered testing strategy for the Lex modular monolith.

## 2. Testing Levels
| Level | Scope | Data Strategy | Runner |
| :--- | :--- | :--- | :--- |
| **Unit** | Domain logic / Handlers in isolation. | Mocks (NSubstitute). | xUnit |
| **Module Integration** | Single module slice (Persistence + API). | SQL Server / Postgres via **Testcontainers**. | xUnit |
| **Cross-Module Integration** | Orchestration between 2+ modules via MQ. | RabbitMQ + Postgres via **Testcontainers**. | xUnit |
| **E2E (End-to-End)** | Full user journey via Web Client. | Real stack via **Docker Compose**. | Playwright |

## 3. The "On-Prem" Quality Constraint
- Tests must be executable on a local development machine or a standard CI agent without internet access to external clouds.
- **WireMock.NET** is the standard for mocking third-party integrations (Google, Stripe, SMTP).

## 4. Test Environment Standards
- **Resetting State**: Every Integration test must use a clean database schema (applied via EF Core migrations at setup).
- **Correlation**: E2E tests must verify that the `X-Correlation-Id` is propagated through the entire stack and appears in the Seq logs.

## 5. Failure Handling
- **Async Wait**: Integration tests for background jobs must poll for a "Compeleted" event or a specific DB state with a 30s timeout before failing.
- **Artifact Capture**: Playwright tests must record videos and traces for failed runs in CI.

## 6. Performance Budget
- Module Integration tests should complete in < 5 seconds.
- Cross-Module Integration tests should complete in < 15 seconds.
