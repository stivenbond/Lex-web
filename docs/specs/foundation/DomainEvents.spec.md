# Domain Specification: Domain Events

## 1. Context
- **Module**: SharedKernel
- **Scope**: Cross-module communication and side-effect orchestration.

## 2. Core Ubiquitous Language
- **Domain Event**: A record of something that happened in a module's domain that may be of interest to other parts of the system.
- **Correlation ID**: A unique identifier (GUID) that ties multiple events and operations back to a single originating request.
- **OccurredAt**: The timestamp when the event was generated.

## 3. Domain Model Hierarchy

- **Interface: IDomainEvent**
    - **Purpose**: To provide a common contract for all events published to the message bus or handled in-process.
    - **Properties**:
        - `CorrelationId`: Essential for distributed tracing and logging.
        - `OccurredAt`: Use `DateTimeOffset` to ensure timezone consistency.

## 4. Value Objects
N/A

## 5. Domain Events
(This is a spec for the *base* of all events, not specific events).

## 6. Business Operations (Conceptual)
- **Op: Event Publication**
    - **Trigger**: An aggregate state change.
    - **Side Effects**: Event is added to an outbox or published directly to MassTransit.
- **Op: Event Consumption**
    - **Outcome**: A side effect in a different module (e.g., sending a notification, updating a read model).
