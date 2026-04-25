# Documentation Map

This directory is the canonical documentation root for the Lex codebase.

## Start Here

- [architecture.md](./architecture.md): System architecture and module boundaries.
- [web-client-architecture.md](./web-client-architecture.md): Frontend architecture and client-side conventions.
- [project/implementation-status.md](./project/implementation-status.md): Source of truth for implementation status, known gaps, and delivery posture.
- [project/spec-canonical-map.md](./project/spec-canonical-map.md): Mapping from legacy agent specs to canonical module-local or docs-level specs.
- [DEPLOYMENT.md](./DEPLOYMENT.md): Current deployment topology, CI/CD flows, and known pipeline gaps.

## Structure

- `docs/adr/`: Architectural decision records.
- `docs/specs/`: Cross-cutting specs and foundation/infrastructure specs.
- `docs/project/`: Repo governance, implementation truth, migration notes, and cleanup artifacts.
- `src/Modules/<ModuleName>/`: Module-local specs that belong with their code.

## Canonical Rules

- Module-level behavior, API, and feature specs belong next to the module that owns them.
- Cross-cutting platform specs belong under `docs/specs/`.
- Repo status is tracked only in `docs/project/implementation-status.md`.
- Legacy agent/platform-specific material is not authoritative unless it has been reconciled into the locations above.

## Historical Documents

The following files remain in the repo only as compatibility entry points and should not be treated as status ledgers:

- `implementation.md`
- `TODO.md`
- `DOCUMENTATION.md`
