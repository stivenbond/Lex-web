# Specification: Module Integration Test Baseline

## 1. Overview
- **Category**: Integration Testing
- **Scope**: Verification of a single module's vertical slice (API -> Core -> Persistence).

## 2. Infrastructure: Testcontainers
- **Standard**: Every test class inheriting from `IntegrationTestBase` will automatically spin up:
    - `PostgresContainer`: A fresh database instance.
    - `RabbitMqContainer`: A fresh message broker instance.
- **Isolation**: Each test class uses a unique database name to allow parallel execution in C#.

## 3. The "Handler Test" Flow
- **Input**: A valid MediatR `Command` or `Query`.
- **Logic**:
    1.  Call the handler via the DI container.
    2.  The handler interacts with the real Postgres DB (via its DbContext).
    3.  The handler publishes events (captured by an in-memory `IBus` harness).
- **Verification**:
    - Assert the returned `Result<T>` is `Success`.
    - Assert the data exists in the Postgres DB using a raw query or a read-only DbContext.
    - Assert that specific cross-module events were sent to the message bus.

## 4. External Dependency Mocking
- **SMTP**: Use a "Null-Sink" implementation or a local container like `MailHog`.
- **Identity**: Mock the `ICurrentUser` service to simulate different roles (Teacher, Student, Admin).
- **Third-Party APIs**: Use `WireMock.NET` to define expected JSON responses and verify outgoing HTTP requests.

## 5. Persistence Hardening
- Tests must verify that EF Core configurations (Shadow properties, Value converters, Default schemas) are correctly mapped against the real Postgres instance, not just an "In-Memory" DB provider.

## 6. Cleanup
- Containers are destroyed automatically after the test collection finishes via `IAsyncLifetime`.
