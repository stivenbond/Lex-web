# Module Specification: [Module Name]

## 1. Overview
- **Name**: [e.g., Scheduling]
- **Purpose**: [High-level purpose of the module]
- **Schema**: `[schema_name].*`
- **Primary Stakeholders**: [Who uses this module?]

## 2. Domain Boundaries
List the main Aggregate Roots and Entities managed by this module. Give a high-level description of their role. (Detailed property/method descriptions go in code comments).

- **Aggregate Root: [Name]**: [Role/Responsibility]
- **Entity: [Name]**: [Role/Responsibility]
- **Value Object: [Name]**: [Role/Responsibility]

## 3. Module Responsibilities
- [Responsibility 1]
- [Responsibility 2]

## 4. Integration & Dependencies
- **Inbound Events**: [Events from other modules this module consumes]
- **Outbound Events**: [Events this module publishes for others]
- **External Dependencies**: [Third-party APIs, specific infra needs]

## 5. Security & Authorization
- **Permission Constants**: `[ModuleName]Permissions.cs`
- **Policies**: [Specific module-level policies]

## 6. Cross-Module Interactions
Describe how this module interacts with others (e.g., "When a Slot is assigned in Scheduling, update Diary")
