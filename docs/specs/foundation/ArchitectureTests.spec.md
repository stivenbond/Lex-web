# Infrastructure Specification: Architecture Tests

## 1. Overview
- **Category**: Quality Assurance / Enforcement
- **Parent Module**: Foundation (Shared)
- **Component Name**: ArchitectureTestSuite

## 2. Technical Strategy
- **Base Technology**: `NetArchTest.Rules` or `ArchUnitNET`.
- **Role**: Automatically enforce project reference rules and boundary constraints on every CI run.

## 3. Implementation Details
The suite contains rules that scan the loaded assemblies and verify that they comply with the rules defined in `docs/architecture.md`.

- **Classes/Interfaces**:
    - **DependencyTests**: Verifies table in Architecture Ref Section 2.2.
    - **BoundaryTests**: Ensures internal implementation details don't leak.
    - **NamingConventionTests**: Enforces event and handler naming rules.

## 4. Configuration & Settings
- **Test Project**: `tests/Lex.ArchitectureTests/`

## 5. Resilience & Performance
- **Speed**: Should run in < 5 seconds to ensure no friction in CI/CD.

## 6. Integration Points
- **CI Pipeline**: Blocks PR merge if rules are violated.

## 7. Migration & Deployment
- **Verification**: If a module is extracted to a microservice, its boundary rules should be updated in the test suite to reflect the new network boundary.
