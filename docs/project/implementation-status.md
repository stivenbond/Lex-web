# Implementation Status

Date: 2026-05-14

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
| Shared kernel | Done | Core primitives and BaseDbContext established with consistent audit logging and snake_case conventions. |
| Host/API | Done | Containerized, health checks fixed, module registration verified, and initial migrations generated for all modules. |
| Scheduling | Done | Full vertical slice with initial migrations and domain logic. |
| DiaryManagement | Done | Domain, persistence, and migrations complete. |
| LessonManagement | Done | Switched to TPH for lesson blocks; persistence and migrations complete. |
| AssessmentCreation | Partial | Core/domain/infrastructure slices exist, but repository and end-to-end flow are not complete. |
| AssessmentDelivery | Partial | Migrations generated; grading/session flow still needs production-level depth. |
| FileProcessing | Partial | Background processing skeleton and migrations exist, but workflow coverage remains incomplete. |
| Notifications | Partial | Domain, DbContext, and migrations exist; event delivery flows still unverified. |
| Reporting | Partial | Migrations generated; read-model implementation depth remains limited. |
| GoogleIntegration | Partial | Persistence and migrations complete; OAuth flow and resilience still unverified. |
| ImportExport | Partial | Persistence and migrations complete; handlers remain early-stage. |
| ObjectStorage | Partial | Persistence and migrations complete; operational validation needed. |
| Frontend shell | Done | Routes implemented using shadcn/ui; Keycloak auth flow fixed and verified in AuthContext. |
| Integration tests | Partial | Some architecture/module tests exist, but broad system validation is not complete. |
| CI/CD | Partial | GitHub workflows exist, but release packaging has configuration gaps. |
| Deployment docs | Done | .env template and Docker Compose verified. |

## Confirmed Gaps

- Several modules have migrations but lack full end-to-end behavior behind them.
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
