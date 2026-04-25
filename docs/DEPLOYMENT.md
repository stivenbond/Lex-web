# Deployment and Delivery

Date: 2026-04-25

This document reflects the deployment assets currently present in the repository and the gaps that still need attention.

## Current Topology

- `infra/docker-compose.yml` defines a self-hosted stack for API, web, PostgreSQL, RabbitMQ, Redis, Keycloak, MinIO, and Seq.
- `src/Host/Lex.API/Dockerfile` builds the API container.
- `client/Dockerfile` builds the Next.js web container.
- `.github/workflows/ci.yml` runs .NET and frontend checks plus Dockerfile validation.
- `.github/workflows/cd-staging.yml` builds/pushes images and triggers remote `upgrade.sh`.
- `.github/workflows/cd-release.yml` builds/pushes release images and assembles a release zip.

## Known Gaps

- CI reliability is not yet re-verified from this cleanup branch.
- Release workflow referenced `infra/.env.template`, which was missing before this cleanup.
- Deployment docs in the repo previously overstated operational readiness.
- Docker and runtime version alignment needed correction between the solution SDK and the API Dockerfile.

## Required Environment

Use [infra/.env.template](../infra/.env.template) as the baseline for deployment configuration.

At minimum, set:

- `LEX_VERSION`
- `DB_PASSWORD`
- `RABBITMQ_PASSWORD`
- `KEYCLOAK_PUBLIC_URL`
- `KEYCLOAK_ADMIN_PASSWORD`
- `MINIO_ACCESS_KEY`
- `MINIO_SECRET_KEY`

## Staging Flow

1. Push to `main`.
2. GitHub Actions builds and pushes `lex-api` and `lex-web` images tagged with the short SHA and `staging`.
3. The staging workflow SSHes into the target host and runs `infra/scripts/upgrade.sh` with `LEX_VERSION` set to that SHA.

## Release Flow

1. Push a tag matching `v*.*.*`.
2. GitHub Actions builds and pushes versioned images.
3. The workflow creates air-gap tarballs and a release zip with compose, scripts, and Keycloak assets.
4. A GitHub Release is published with the assembled zip.

## Operator Notes

- Treat the workflows as infrastructure scaffolding, not as proof of a healthy release pipeline.
- Re-verify compose startup, upgrade flow, and restore flow before relying on them for production.
- Keep deployment documentation aligned with the actual files under `infra/` and `.github/workflows/`.
