# Lex

Lex is a modular-monolith learning platform for scheduling, diary workflows, lesson planning, assessments, reporting, notifications, file handling, and related school operations.

This repository contains meaningful architecture, specs, and partial implementations across backend, frontend, infrastructure, and deployment assets. It is not yet a fully verified or uniformly complete product. For actual implementation truth, use [docs/project/implementation-status.md](./docs/project/implementation-status.md) instead of inferring completion from scaffolding or spec coverage.

## Stack

- Backend: .NET 10, ASP.NET Core, EF Core, MediatR, MassTransit, SignalR
- Frontend: Next.js 15, React, Tailwind, Zustand, TanStack Query
- Platform: PostgreSQL, RabbitMQ, Redis, Keycloak, MinIO, Docker Compose

## Repo Shape

- `src/Core/`: shared kernel and infrastructure foundations
- `src/Host/Lex.API/`: API host, Swagger wiring, reverse proxy, module registration
- `src/Modules/`: module-owned code and specs
- `client/`: Next.js web client
- `docs/`: canonical documentation root
- `infra/`: deployment and environment assets

## How To Work In This Repo

Read these in order:

1. [docs/project/implementation-status.md](./docs/project/implementation-status.md)
2. [docs/README.md](./docs/README.md)
3. [docs/architecture.md](./docs/architecture.md)
4. [docs/web-client-architecture.md](./docs/web-client-architecture.md)

## Current Reality

- Scheduling appears to be the most implemented module.
- Most other modules are partial: specs exist, code shells exist, and some vertical slices exist, but migrations and end-to-end validation are incomplete.
- Swagger is wired in the API host for development.
- CI/CD workflows exist, but they should be treated as present scaffolding until re-verified.

## Documentation

- [Documentation Map](./docs/README.md)
- [Architecture](./docs/architecture.md)
- [Authentication](./docs/AUTHENTICATION.md)
- [API](./docs/API.md)
- [Deployment](./docs/DEPLOYMENT.md)
- [Specifications](./docs/specs)

## Local Validation

Backend:

```powershell
$env:DOTNET_CLI_HOME = "$PWD\\.dotnet"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
dotnet build Lex.slnx
```

Frontend:

```powershell
cd client
npm ci
npm run type-check
npm run lint
npm run test
```
