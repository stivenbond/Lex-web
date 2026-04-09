---
description: Guide for implementing a new feature that may span multiple modules logically.
---

# Implementing a New Feature

This workflow outlines the process for implementing a new feature based on a feature description. Even if the feature spans multiple modules conceptually, strict architectural boundaries must be enforced according to `docs/architecture.md`.

## General Architectural Rules
- **Vertical Slice Architecture**: All files associated with a feature (Command/Query, Handler, Validator) belong together in a single folder under `Features/[FeatureName]/` of the respective module.
- **Strict Module Isolation**: A feature in Module A cannot call a repository, DB, or code from Module B.
- **Event-Driven Cross-Module Logic**: If a feature "requires" action in another module, the originating Handler must publish an `IDomainEvent` onto the message bus, which the target module consumes.
- **Reporting & Cross-Schema Views**: If the feature is a Dashboard or Report needing data from multiple modules, it MUST be built into the `Reporting` module. The `Reporting` module subscribes to domain events to build an asynchronous denormalized read model.
- **Command vs Query**: Queries go synchronously through MediatR to the database. Commands with cross-module actions publish events to MassTransit after the DB commit. 

## Step-by-Step Implementation

### Step 1: Analyze and Decompose the Feature
1. Review the feature description.
2. Identify which domain modules own the data and behaviors described.
3. If the feature alters data in multiple domains, determine the **primary initiating module**.
4. Map out the flow: Initiating Module handles the Command -> publishes Event -> Secondary Modules handle the Event.

### Step 2: Implement the Feature in the Primary Module
In the primary module's `Core` project (e.g., `Modules/YourApp.Module.[Primary].Core/Features/[FeatureName]/`):
1. **Define the Request**: Create the `Command` (IRequest<Result<T>>) or `Query`.
2. **Define the Validator**: Create a `FluentValidation` AbstractValidator for the Command.
3. **Write the Handler**: 
   - Implement `IRequestHandler`.
   - Load the relevant Aggregate Root via its repository interface.
   - Invoke domain logic/methods on the aggregate. DO NOT put business logic directly in the handler.
   - Persist changes.
   - (For Commands) Collect and publish Domain Events in-process or to MassTransit.
   - Return a `Result<T>`. No exceptions for business logic failures.

### Step 3: Implement Cross-Module Side-Effects (If Applicable)
If the feature spans multiple modules logically:
1. Ensure the `IDomainEvent` is defined in the primary module's `Core` project.
2. In the secondary module's `Infrastructure` project, create a Consumer (e.g., `SecondaryActionConsumer : IConsumer<PrimaryEvent>`).
3. Have the Consumer map to a MediatR Command within the secondary module or directly execute the target operation.
4. Ensure the Event schema remains backward compatible (Additive properties only).

### Step 4: Implement Long-Running or Async Actions (If Applicable)
If the feature is long-running (>200ms):
1. The REST Command returns a `202 Accepted` with a JobId.
2. Process the work asynchronously via a MassTransit consumer.
3. Upon completion, push the result to the client via the SignalR `JobStatusHub`.

### Step 5: Implement Cross-Module Queries (If Applicable)
If the feature is a Read operation bridging multiple domains:
1. Do NOT write a cross-schema SQL join.
2. Create the feature inside the `Reporting` module.
3. If necessary, build consumers in the `Reporting` module to sync required data from other modules into the `Reporting` schema before serving the Query.

### Step 6: Expose the Endpoint
In the `YourApp.API` (Host) project, register the feature's Minimal API endpoint. It should simply be a thin wrapper that maps HTTP primitives to the MediatR request and dispatches it.
