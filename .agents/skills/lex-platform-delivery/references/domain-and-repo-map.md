# Domain and Repo Map

## Repo Structure

- `src/Core/`: shared kernel and cross-cutting infrastructure.
- `src/Host/Lex.API/`: API host, module registration, Swagger wiring, reverse proxy, auth middleware.
- `src/Modules/`: business modules. Each module owns its own specs and persistence boundary.
- `client/`: Next.js web client.
- `docs/specs/`: cross-cutting and foundation specs.
- `docs/project/`: status, reconciliation notes, and repo governance artifacts.
- `infra/`: compose, scripts, and deployment support assets.

## Current Practical Read

- Scheduling is the most implemented module.
- Most other modules are partially scaffolded and partially implemented.
- Many modules have specs and code shells but still lack migrations and production-hardening.
- CI/CD and deployment materials exist but require verification instead of trust-by-presence.

## Module Ownership Rule

Put these in the owning module folder:

- `module-spec.md`
- `api-spec.md`
- `frontend-spec.md`
- feature `spec.md` files
- domain `*.spec.md` files

Put these under `docs/specs/`:

- auth
- testing baselines
- frontend foundation
- async job pattern
- observability
- deployment automation
- other cross-cutting concerns
