# Domain Specification: [Domain Space Name]

## 1. Context
- **Module**: [Module Name]
- **Scope**: [The specific bounded context or subdomain being modeled]

## 2. Core Ubiquitous Language
Define the key terms used in this domain model and their business meanings.

- **[Term Name]**: [Definition]

## 3. Domain Model Hierarchy
Describe the Aggregate Roots and Entities. Detailed properties and domain methods should be described in high-level business terms here; implementation details stay in C# code.

- **Aggregate Root: [Name]**
    - **Purpose**: [Goal of the aggregate]
    - **Invariants**: [Rules that must always be true for this aggregate]
- **Entity: [Name]**
    - **Purpose**: [Role within the aggregate]

## 4. Value Objects
List significant value objects and their composition.
- **[VO Name]**: [e.g., Money, DateRange]

## 5. Domain Events
List events that originate directly from domain state changes.
- **[EventName]**: [What triggered it and why it matters]

## 6. Business Operations (Conceptual)
List the primary domain actions.
- **Op: [Action Name]**
    - **Invariants Checked**: [Specific rules verified]
    - **Resulting State**: [Visual change or event]
