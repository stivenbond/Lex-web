# Feature Specification: [Feature Name]

## 1. Feature Overview
- **Parent Module**: [e.g., Scheduling]
- **Description**: [What problem does this feature solve?]
- **Type**: (Command | Query | Event Consumer | Long-running Job)
- **User Story**: As a [Role], I want to [Action] so that [Outcome].

## 2. Request Representation
- **Request Class**: `[FeatureName]Request` (or Command/Query)
- **Payload Structure**: [High-level description of fields]
- **Validation Rules**: (e.g., Name must not be empty, Date must be in the future)

## 3. Business Logic (The Slice)
Describe the orchestration logic inside the Handler. 

- **Trigger**: [REST POST / SignalR / Message Bus]
- **Domain Logic**: [Which aggregate methods are called? What are the invariants checked?]
- **Side Effects**: [Events published, SignalR pushes, Notifications triggered]

## 4. Persistence
- **Affected Tables**: [e.g., scheduling.slots]
- **Repository Methods**: [New or existing repository methods used]

## 5. Domain Objects (High-Level)
Briefly describe any new classes or interfaces introduced. Don't document internal members; use C# comments for that.

- **[ClassName]**: [High-level purpose]
- **[InterfaceName]**: [High-level purpose]

## 6. API / UI Contracts
- **Route**: `[Endpoint Route]`
- **Response**: `Result<T>` structure
- **UI Interaction**: [Brief description of how the frontend interacts with this]

## 7. Security
- **Required Permission**: `[Permissions.Const]`
- **Auth Policy**: [Policy Name]
