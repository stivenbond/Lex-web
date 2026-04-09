# Lex — Agent Instructions

## Architecture
This is a .NET 10 modular monolith. Read docs/architecture-reference.md before implementing anything.

## Non-negotiable rules
- Modules never reference each other's projects directly
- All cross-module communication via MassTransit events
- Commands and queries use MediatR + vertical slice layout (Features/{FeatureName}/)
- Handlers return Result<T>, never throw for expected errors
- Each module has its own DbContext with its own schema
- No business logic in handlers — logic lives on domain aggregates
- Frontend: no Next.js API routes, no Server Actions, no raw fetch() in components

## When implementing a backend feature
1. Create the command/query in Module.Core/Features/{FeatureName}/
2. Create the handler in the same folder
3. Create the FluentValidation validator in the same folder
4. Add any new domain entities to Module.Core/Domain/
5. Add EF config and repository impl to Module.Infrastructure/
6. Add the minimal API endpoint in the Host project (thin delegate to MediatR)
7. If an event is published, define it in Module.Core/Features/{FeatureName}/

## When implementing a frontend feature
1. Add TanStack Query hooks to lib/api/{module}.ts
2. Add TypeScript types to lib/types/{module}.ts
3. Create page at app/(app)/{module}/{feature}/page.tsx (server component shell)
4. Create interactive parts as client components
5. No inline styles — Tailwind only
6. Forms always use React Hook Form + Zod + useMutation