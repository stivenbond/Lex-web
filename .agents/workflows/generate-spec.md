---
description: Guide for generating detailed specifications (Module, Feature, Infra, or Domain) within the Lex-web architecture.
---

# Workflow: Spec Generation

Use this workflow to generate a formal specification for any part of the system. This ensures that every new component is properly architected before implementation and follows the rules in `docs/architecture.md`.

## Hierarchy of Specs
1. **Module Spec**: High-level design for a new module. Should be created BEFORE any features or infra for that module.
2. **Domain Spec**: Required when modeling complex business logic or aggregate roots.
3. **Feature Spec**: Created for every vertical slice (Command/Query/Handler). References the Module Spec.
4. **Infrastructure Spec**: Created for external integrations or foundational tech components.

## General Rules for Spec Generation
- **Respect Boundaries**: Never spec a feature in Module A that directly touches Module B's database or code.
- **High-Level Context Only**: For classes, interfaces, and methods, focus on their **role** and **responsibility**. Detailed documentation of parameters, return types, and internal logic belongs in the source code as C# XML comments (`///`).
- **Traceability**: Every spec should ideally reference an item in `TODO.md` or a feature description.

## Step-by-Step Generation

### Step 1: Identify the Target
Determine where the change fits in the architecture:
- Adding a new domain capability? -> **Module Spec**
- Adding a new API endpoint/action? -> **Feature Spec**
- Adding a new integration (e.g. Stripe)? -> **Infrastructure Spec**

### Step 2: Select the Template
Load the appropriate template from `.agents/templates/`:
- `module-spec.md`
- `feature-spec.md`
- `infrastructure-spec.md`
- `domain-spec.md`

### Step 3: Analyze Requirements
- Review `docs/architecture.md` for relevant rules (e.g., cross-module event rules).
- Review `TODO.md` to see where this fits in the roadmap.
- Analyze existing code if this is an extension of an existing module.

### Step 4: Draft the Specification
- Fill out the chosen template.
- **Critical**: Ensure the cross-module communication section (Events) is explicit.
- **Critical**: Ensure the authorization section uses the standard `Permissions` class pattern.

### Step 5: Review Against Architecture
Verify the spec doesn't violate:
- **Project Isolation**: Check that nothing in Core depends on Infrastructure or other modules.
- **Event-Driven Rules**: Ensure side effects are handled via the message bus, not direct calls.
- **Schema Isolation**: Ensure the feature only operates on its designated DB schema.

### Step 6: Finalize and Implement
The finalized spec serves as the "source of truth" for the AI/Developer during implementation. Use the `/implement-feature` or `/implement-module` workflows to proceed.
