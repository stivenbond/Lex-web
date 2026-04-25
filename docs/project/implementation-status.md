# Implementation Status

Date: 2026-04-25

This document is the single source of truth for implementation status in this repository.

## Status Semantics

- `Done`: Implemented, wired, and verified with meaningful tests or validation.
- `Partial`: Code exists, but key slices are missing, incomplete, unverified, or not production-ready.
- `Spec Only`: Requirements/specs exist, but implementation is absent or skeletal.
- `Unknown`: Present in the repo, but not yet verified.

Specs existing in `docs/specs/` or module folders do not count as implementation completion.

## Current Delivery Snapshot

| Area | Status | Notes |
|---|---|---|
| Shared kernel | Partial | Core primitives exist, but some abstractions and shared modeling remain uneven across modules. |
| Host/API | Partial | App boots and Swagger is wired in development, but readiness, deployment, and endpoint completeness are not fully verified. |
| Scheduling | Partial | Strongest implemented module; core domain, handlers, infrastructure, and tests exist, but migration/schema completion is still missing. |
| DiaryManagement | Partial | Domain, controller, and DbContext exist, but schema/migrations and fuller workflow completion are missing. |
| LessonManagement | Partial | Domain model and basic infrastructure exist, but implementation depth is limited and code structure needs cleanup. |
| AssessmentCreation | Partial | Core/domain/infrastructure slices exist, but repository, schema, and end-to-end flow are not complete. |
| AssessmentDelivery | Partial | Domain and controller skeletons exist, but grading/session flow is not production-complete. |
| FileProcessing | Partial | Background processing skeleton exists, but schema, workflow coverage, and integration validation remain incomplete. |
| Notifications | Partial | Basic domain and DbContext exist, but end-to-end event mapping and delivery flows are not verified. |
| Reporting | Spec Only / Partial | Specs are present, but read-model implementation depth and schema maturity are limited. |
| GoogleIntegration | Partial | Service and handler skeletons exist, but OAuth, resilience, and persistence are incomplete. |
| ImportExport | Partial | Orchestration intent is documented, but handlers and persistence remain early-stage. |
| ObjectStorage | Partial | Service, provider abstractions, and controller exist, but provider completeness and operational validation are still needed. |
| Frontend shell | Partial | Next.js app structure, auth plumbing, and route scaffolds exist, but many routes remain placeholders. |
| Integration tests | Partial | Some architecture/module tests exist, but broad system validation is not complete. |
| CI/CD | Partial | GitHub workflows exist, but reliability is not established and release packaging has configuration gaps. |
| Deployment docs | Partial | Base materials exist, but they were overstating readiness and required reconciliation with actual repo state. |

## Confirmed Gaps

- Database migrations are not present for the module DbContexts that claim schema ownership.
- Several modules have controller/service/DbContext shells without end-to-end behavior behind them.
- Some docs claimed successful builds, completed validation, or completed phases without current verification.
- Specs were split across `docs/specs/`, module folders, and legacy `.agents` material.
- Agent workflow guidance was fragmented and platform-specific.
- CI/CD configuration references artifacts that were missing from the repo.

## Quality Concerns Found

- Multiple domain types are defined in single files where clearer separation is warranted.
- Some modules contain duplicate or transitional persistence types, which makes schema ownership hard to understand.
- Repo-level status files had drifted away from actual code state.
- Historical agent documents contained overlapping guidance and stale references.

## Working Agreements Going Forward

- Update this file when implementation truth changes.
- Keep module-local specs with the module that owns the behavior.
- Treat README as orientation, not implementation truth.
- Do not mark work complete because a spec or scaffold file exists.
- When shipping a feature, update code, tests, module-local spec, and this status file together.
