---
name: lex-platform-delivery
description: End-to-end workflow and domain guidance for working in the Lex modular-monolith repository. Use when implementing, reviewing, or restructuring Lex features, modules, specs, infrastructure, agent guidance, or repo governance so the work stays aligned with module boundaries, canonical docs, implementation status tracking, and delivery standards.
---

# Lex Platform Delivery

Use this skill when making changes in the Lex repository.

## Core Workflow

1. Read [docs/project/implementation-status.md](references/implementation-status.md) before assuming anything is complete.
2. Read [docs/architecture.md](references/architecture.md) for module boundaries and host/infrastructure rules.
3. Read [docs/README.md](references/documentation-map.md) to find the canonical doc location.
4. Keep module behavior specs with the owning module and cross-cutting specs under `docs/specs/`.
5. When implementing a feature, update code, tests, module-local spec, and implementation status in the same pass.
6. Do not mark work done because a scaffold, controller, spec, or route placeholder exists.

## Delivery Rules

- Keep one source of truth for repo status: `docs/project/implementation-status.md`.
- Treat README as onboarding material, not implementation truth.
- Prefer one public type per file for domain and shared-kernel modeling unless co-location materially improves the slice.
- Keep module boundaries strict: no direct module-to-module project references for business logic.
- If legacy `.agents` artifacts disagree with module-local or `docs/` specs, prefer the canonical docs and update or retire the legacy artifact.
- Verify pipeline or deployment claims against the actual workflow and infra files before documenting them as ready.

## Feature Implementation Checklist

1. Confirm the owning module and exact canonical spec path.
2. Check whether the change is backend-only, frontend-only, or full vertical slice.
3. Implement domain/application/infrastructure changes in the owning module only.
4. Add or update module-local spec files near the relevant feature/domain.
5. Add or update tests at the narrowest valuable level first, then broader integration coverage where justified.
6. Update `docs/project/implementation-status.md` if implementation truth changed.
7. If the change affects runtime or operations, update `docs/DEPLOYMENT.md` or other relevant docs.

## Read These References As Needed

- Domain and repo map: `references/domain-and-repo-map.md`
- Canonical docs map: `references/documentation-map.md`
- Implementation truth: `references/implementation-status.md`
- Architecture reference: `references/architecture.md`
