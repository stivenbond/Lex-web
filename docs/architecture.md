  
   
**SYSTEM ARCHITECTURE REFERENCE**  
Modular Enterprise Platform on .NET 10  
 

Version 1.0  |  Classification: Internal  |  Audience: Architects & Senior Developers

| Purpose of This Document This document is the authoritative architecture reference for the platform. It defines every structural decision, the rationale behind it, and the rules that must be respected during implementation. It is intentionally domain-agnostic. Feature-level specifics belong in spec-docs within the codebase. This document answers: how is the system organised, why is it organised that way, and how does each concern fit together. |
| :---- |

# **Table of Contents**

# **1\. Architectural Philosophy & Guiding Principles**

Every structural decision in this system is derived from a small set of guiding principles. Understanding these principles is more important than memorising the rules — it allows any developer to reason about novel situations correctly without a spec.

## **1.1 Core Principles**

### **Modular Monolith First, Microservices Ready**

The system is built as a single deployable unit composed of strictly isolated modules. This gives the operational simplicity of a monolith (one process, one database instance, one deployment unit) while maintaining the logical boundaries that allow any individual module to be extracted into an independent service at a later date with minimal refactoring. The decision to extract a module into a microservice should be driven by a concrete operational need — independent scaling, independent deployment cadence, or team ownership — not by architectural idealism.

| Rule Modules may never reference each other directly via code. All cross-module communication must go through the message bus or through published contracts in SharedKernel. Violating this rule is the single fastest way to make future extraction impossible. |
| :---- |

### **Vertical Slice Architecture per Module**

Within each module, code is organised by feature, not by technical layer. A "feature" owns its command or query, its handler, its validator, and its domain logic in a single cohesive folder. This makes the codebase navigable by domain concept rather than by technical role, and it makes it easy for a new developer to add a feature without touching existing code.

### **Explicit Over Implicit**

Configuration, dependency registration, and routing must be explicit and discoverable. Magic conventions that work silently are acceptable in frameworks but should not be invented within application code. Every module registers itself explicitly via an extension method. Every event consumer is explicitly declared.

### **Infrastructure is an Implementation Detail**

Domain logic and application logic must never depend on infrastructure concerns. The database, the message broker, the external HTTP clients — these are all implementation details that are injected, not imported. This is enforced structurally: domain and application layers have no reference to infrastructure packages.

### **On-Prem First**

Every infrastructure decision is evaluated against the constraint that the system must install on a single server at an institution with a single command. This means no mandatory cloud services, no mandatory Kubernetes, no mandatory internet connectivity after initial installation. Cloud-native patterns are welcome where they improve the architecture but must never be load-bearing for a functional on-prem deployment.

## **1.2 What This Architecture Is Not**

* It is not event sourcing. Domain events are used for side-effects and cross-module communication, not as the source of truth for state.

* It is not CQRS with separate read and write databases per module. Modules share one Postgres instance with schema-level isolation. A dedicated read model exists only in the Reporting module.

* It is not microservices. Until a module is explicitly extracted, it runs in-process.

* It is not serverless. The deployment model is containerised long-running services.

# **2\. Solution & Project Structure**

The solution structure is the physical enforcement of module boundaries. The folder hierarchy and project reference rules below are non-negotiable — they are what makes the "modular" in modular monolith real.

## **2.1 Repository Layout**
`YourApp/                              <- repository root`
`├── src/`
`│   ├── Host/`
`│   │   └── YourApp.API/              <- ASP.NET Core entry point`
`│   │`
`│   ├── Core/`
`│   │   ├── YourApp.SharedKernel/     <- shared abstractions only`
`│   │   └── YourApp.Infrastructure/   <- cross-cutting infrastructure`
`│   │`
`│   └── Modules/`
`│       ├── YourApp.Module.{Name}/`
`│       │   ├── YourApp.Module.{Name}.Core/`
`│       │   └── YourApp.Module.{Name}.Infrastructure/`
`│       └── ... (one folder per module)`
`│`
`├── tests/`
`│   ├── YourApp.Module.{Name}.Tests/`
`│   ├── YourApp.IntegrationTests/`
`│   └── YourApp.ArchitectureTests/    <- enforces project reference rules`
`│`
`├── client/                           <- web client (framework of choice)`
`│`
`├── infra/`
`│   ├── docker-compose.yml`
`│   ├── docker-compose.override.yml   <- dev overrides`
`│   ├── k8s/                          <- Helm charts`
`│   └── scripts/                      <- install.sh, upgrade.sh, backup.sh`
`│`
`├── .github/`
`│   └── workflows/`
`│       ├── ci.yml`
`│       ├── cd-staging.yml`
`│       └── cd-release.yml`
`│`
`└── docs/                             <- this document and ADRs`

## **2.2 Project Reference Rules**

The following table defines which projects may reference which. These rules are automatically enforced by architecture unit tests (see Section 9).

| Project | May Reference | Must NOT Reference |
| :---- | :---- | :---- |
| Module.{Name}.Core | SharedKernel | Infrastructure, other Modules, EF Core, MassTransit, Refit |
| Module.{Name}.Infrastructure | Module.{Name}.Core, SharedKernel, Infrastructure | Other Module.\*.Core or Module.\*.Infrastructure directly |
| YourApp.Infrastructure | SharedKernel | Any Module project |
| YourApp.API (Host) | All Module.Infrastructure projects, SharedKernel, Infrastructure | Must not contain business logic |
| SharedKernel | (nothing internal) | Any Module, Infrastructure, or Host project |

## **2.3 Project Responsibilities**

### **YourApp.SharedKernel**

* Base entity and aggregate root classes

* Domain event base interface (IDomainEvent)

* Common value objects (e.g., Money, DateRange)

* Result\<T\> and Error types for railway-oriented error handling

* Shared contracts used by more than one module (published as NuGet if extracted)

* NO business logic. NO infrastructure dependencies.

### **YourApp.Infrastructure**

* EF Core DbContext base configuration and conventions

* MassTransit bus registration and topology configuration

* Keycloak / OpenID Connect authentication middleware registration

* Serilog sink configuration

* OpenTelemetry tracer and meter provider configuration

* YARP reverse proxy configuration

* Common middleware (exception handling, correlation IDs, audit logging)

* Health check aggregation

### **YourApp.Module.{Name}.Core**

* Domain entities and aggregates specific to this module

* Application layer: MediatR commands, queries, and handlers

* FluentValidation validators for commands

* Domain events published by this module

* Module-specific interfaces (e.g., IInvoiceRepository)

* NO EF Core. NO HTTP clients. NO MassTransit references.

### **YourApp.Module.{Name}.Infrastructure**

* EF Core DbContext slice for this module (registered against the shared DB)

* Repository implementations

* External API clients (Refit interfaces \+ registration)

* MassTransit consumers for events this module reacts to

* Module registration extension method: AddBillingModule(this IServiceCollection services)

### **YourApp.API (Host)**

* Program.cs / startup wiring only

* Calls each module's AddXxxModule() extension method

* SignalR hub definitions

* Minimal API endpoint mapping (thin delegates to MediatR)

* YARP route configuration

* Health check endpoint exposure

* Contains zero business logic

# **3\. Module Internal Architecture**

Every module follows the same internal structure. This consistency is deliberate — it reduces cognitive overhead when moving between modules and makes onboarding new developers predictable.

## **3.1 Vertical Slice Layout**

`YourApp.Module.Billing.Core/`
`├── Features/`
`│   ├── CreateInvoice/`
`│   │   ├── CreateInvoiceCommand.cs       <- IRequest<Result<InvoiceId>>`
`│   │   ├── CreateInvoiceHandler.cs       <- IRequestHandler<...>`
`│   │   └── CreateInvoiceValidator.cs     <- AbstractValidator<CreateInvoiceCommand>`
`│   ├── GetInvoice/`
`│   │   ├── GetInvoiceQuery.cs            <- IRequest<Result<InvoiceDto>>`
`│   │   └── GetInvoiceHandler.cs`
`│   └── InvoiceApproved/`
`│       └── InvoiceApprovedEvent.cs       <- IDomainEvent (published to MQ)`
`├── Domain/`
`│   ├── Invoice.cs                        <- aggregate root`
`│   └── InvoiceLineItem.cs                <- value object or entity`
`└── Abstractions/`
    `└── IInvoiceRepository.cs             <- interface only`

## **3.2 Command vs Query Routing**

The routing of a request determines its path through the system. This must be applied consistently across all modules:

| Request Type | Route | Side Effects Allowed | MQ Involvement |
| :---- | :---- | :---- | :---- |
| Query (read) | MediatR in-process → DB | None | Never |
| Simple Command | MediatR in-process → DB | Domain events published in-process | Optional: publish after commit |
| Cross-module Command | MediatR in-process → DB → publish event | Domain events published to MQ | Always |
| Long-running Command | MediatR → publish job → consumer → SignalR push | Async, via consumer | Always |

| Critical Rule: Never route queries through the message queue Routing read operations through RabbitMQ introduces async latency and failure modes where none are needed. Queries must always be resolved in-process synchronously. The MQ is for commands with observable side effects and for cross-module event propagation only. |
| :---- |

## **3.3 Handler Responsibilities**

A handler is the unit of application logic. It must:

1. Load the domain aggregate(s) via the repository interface

2. Call domain methods on the aggregate — never put business logic in the handler

3. Persist changes via the repository

4. Collect and publish domain events after a successful commit

5. Return a Result\<T\> — never throw exceptions for expected failure cases

## **3.4 Result Pattern**

All handlers return Result\<T\> (defined in SharedKernel). This enforces railway-oriented error handling and eliminates try/catch in calling code. Exceptions are reserved for truly unexpected infrastructure failures and are caught globally by middleware.

`public sealed record Result<T>(T? Value, Error? Error, bool IsSuccess);`
`public sealed record Error(string Code, string Message, ErrorType Type);`

# **4\. Messaging Architecture**

## **4.1 Technology Stack**

* Transport: RabbitMQ (containerised)

* Abstraction: MassTransit — all bus interactions go through MassTransit, never raw RabbitMQ

* In-process mediation: MediatR (commands, queries, in-process events)

* Rationale for MassTransit: transport-agnostic, built-in retry/dead-letter/saga, clean consumer registration, easy migration to Azure Service Bus or Amazon SQS if cloud deployment is ever added

## **4.2 Message Flow Diagram**

The following describes the complete lifecycle of a request from client to response:

 `Web Client / SignalR`
       `|`
       `v`
 `YARP Reverse Proxy  (TLS termination, rate limiting, routing)`
       `|`
       `v`
 `YourApp.API  (SignalR Hub / Minimal API endpoint)`
       `|`
       `v`
 `MediatR  (in-process dispatch)`
       `|`
  `_____|__________`
 `|                |`
 `v                v`
`Query           Command`
 `|                |`
 `v                v`
`DbContext    Handler -> DbContext`
 `|                |`
 `v           Domain Events`
`Response           |`
                   `v`
            `MassTransit Publisher`
                   `|`
                   `v`
               `RabbitMQ`
                   `|`
         `__________|__________`
        `|          |          |`
        `v          v          v`
   `Consumer A  Consumer B  Consumer C  (in same process or extracted)`
        `|`
        `v`
  `SignalR Hub -> Client push notification`

## **4.3 Event Naming Conventions**

All domain events must follow this naming contract. Consistent naming is critical for MassTransit topology auto-configuration and for consumer discoverability.

| Convention | Example |
| :---- | :---- |
| Event class name | InvoiceApprovedEvent |
| Namespace pattern | YourApp.Module.Billing.Features.InvoiceApproved |
| Exchange name (auto-derived) | yourapp.module.billing.features.invoiceapproved:invoiceapprovedevent |
| Consumer class name | InvoiceApprovedEventConsumer |
| Consumer namespace (in target module) | YourApp.Module.Notifications.Infrastructure.Consumers |

## **4.4 Message Versioning Policy**

This policy must be applied from the first production message contract. Violating it makes zero-downtime upgrades and multi-version on-prem installations impossible.

* Message contracts are additive only. New optional properties may be added. Existing properties must never be renamed or removed.

* Breaking changes require a new versioned contract: InvoiceApprovedEventV2. The old consumer continues to run until all publishers are migrated.

* Use MassTransit's IConsumer\<T\> with message versioning attributes to handle multiple versions in the same consumer if needed.

* All message contracts are defined in the publishing module's Core project so they can be referenced without a circular dependency.

## **4.5 Dead Letter & Retry Policy**

All consumers must have an explicit retry and dead-letter policy configured at registration time in YourApp.Infrastructure's MassTransit configuration:

* Immediate retry: 3 attempts, 500ms interval

* Requeue delay: exponential backoff, max 5 attempts

* Dead letter exchange: all failed messages route to a dedicated dead-letter queue per consumer, viewable in RabbitMQ management UI

* Alerting: dead-letter queue depth is a health check metric exposed via OpenTelemetry

# **5\. API Layer & Real-Time Communication**

## **5.1 Transport Responsibilities**

The system uses two complementary transports. Understanding which to use for a given scenario is important:

| Concern | REST (Minimal API) | SignalR |
| :---- | :---- | :---- |
| Use for | Request/response, CRUD, queries | Push notifications, async results, live updates |
| Client initiates | Yes — client pulls | Both — client can invoke, server pushes |
| Cacheable | Yes (GET endpoints) | No |
| Error handling | HTTP status codes \+ ProblemDetails | Hub method exceptions \+ client callbacks |
| Auth | Bearer token in Authorization header | Bearer token in query string on connect |
| On-prem firewall | Always works | Requires WebSocket or falls back to long-polling |

## **5.2 Async Result Pattern**

For long-running operations (anything that may take more than \~200ms), use this pattern rather than blocking an HTTP connection:

6. Client sends REST POST command → API returns 202 Accepted with a jobId (GUID)

7. Server processes asynchronously via MassTransit consumer

8. On completion, consumer publishes result to SignalR hub

9. Hub pushes result to the client connection associated with the jobId

This pattern keeps HTTP connections short, allows progress reporting, and is resilient to client reconnects (the jobId can be used to query current status via REST if the SignalR connection was dropped).

## **5.3 SignalR Hub Structure**

`Hubs/`
`├── NotificationHub.cs      <- general push notifications`
`├── JobStatusHub.cs         <- async job progress and results`
`└── [DomainSpecificHub].cs  <- one per real-time feature domain`

Each hub is thin. Hub methods dispatch to MediatR, never containing business logic directly. Hub methods must return Task and never block.

## **5.4 SignalR Backplane**

A Redis backplane is required even in single-server deployments. This is because it future-proofs horizontal scaling of the API tier without requiring architectural changes. The Redis instance adds negligible resource overhead and is already present in the compose stack for caching.

* Package: Microsoft.AspNetCore.SignalR.StackExchangeRedis

* Configuration: AddSignalR().AddStackExchangeRedis(connectionString)

* Redis is shared between the SignalR backplane and any application-level caching

## **5.5 YARP Reverse Proxy**

YARP (Yet Another Reverse Proxy) is a .NET NuGet package that runs inside the host process as middleware. It handles:

* TLS termination — all HTTP traffic outside the compose network is HTTPS

* Single ingress point for both REST and SignalR (no CORS required between services)

* Rate limiting per route and per authenticated identity

* Request/response logging at the edge before requests reach application code

* Future: if a module is extracted to a microservice, its routes in YARP config are updated to point to the new service URL — the client sees no change

YARP configuration lives in appsettings.json under the ReverseProxy key and is reloadable at runtime without restart.

# **6\. Data Architecture**

## **6.1 Database Strategy**

The system uses a single PostgreSQL instance with one schema per module. This is the correct tradeoff for a modular monolith targeting on-prem single-server deployment:

| Concern | Decision |
| :---- | :---- |
| Database engine | PostgreSQL 16 (Alpine image in Docker) |
| Schema isolation | One schema per module: billing.\*, notifications.\*, reporting.\* |
| ORM | Entity Framework Core 10 with code-first migrations |
| Connection | Single connection string, each module's DbContext sets default schema |
| Cross-schema queries | Forbidden in application code. Use Reporting module read model instead. |
| Migration management | Each module owns its own migrations folder and DbContext. Applied at startup via migration service. |
| On-prem upgrade | Migrations are additive-only (no renames, no drops). Run automatically on container start via dedicated migration job. |

## **6.2 Module DbContext Pattern**

Each module's Infrastructure project contains one DbContext scoped to that module's schema. The contexts are registered independently and share the connection string but nothing else.

`// Module registration (in Module.Infrastructure):`
`services.AddDbContext<BillingDbContext>(options =>`
    `options.UseNpgsql(connectionString,`
        `b => b.MigrationsAssembly("YourApp.Module.Billing.Infrastructure")));`
`// DbContext definition:`
`public class BillingDbContext : DbContext {`
    `protected override void OnModelCreating(ModelBuilder b) {`
        `b.HasDefaultSchema("billing");`
        `b.ApplyConfigurationsFromAssembly(typeof(BillingDbContext).Assembly);`
    `}`
`}`

## **6.3 Cross-Module Data: The Reporting Module**

No module may query another module's schema directly. When a view, dashboard, or report needs data from multiple modules, it is served by the Reporting module, which maintains its own denormalized read tables. These tables are populated by subscribing to domain events from other modules via the message bus.

`Example: Dashboard showing invoice totals + notification counts`
  `- Reporting.Consumer.InvoiceApprovedEventConsumer`
    `-> updates reporting.invoice_summary table`
  `- Reporting.Consumer.NotificationSentEventConsumer`
    `-> updates reporting.notification_summary table`
  `- Reporting query handler reads from reporting.* schema only`

| Why not just join across schemas? Cross-schema joins work today but they couple module data models together. If billing changes its schema, the reporting join breaks. Worse, if billing is ever extracted to a microservice, the join becomes a network call that cannot be expressed as SQL. The event-driven read model is more code upfront but is the only approach that survives module extraction. |
| :---- |

## **6.4 Migration Policy**

* All migrations are additive: add columns, add tables, add indexes. Never rename or drop in a migration.

* Deprecation process: mark column as obsolete in code, stop writing to it, ship a later migration to drop it after all institutions have upgraded past the transition version.

* Migrations run automatically via a dedicated MigrationService that runs before the main application starts inside the same container.

* No hand-edited migration SQL. All migrations are EF Core generated and reviewed in pull request.

# **7\. Authentication & Authorisation**

## **7.1 Identity Provider: Keycloak**

Keycloak is the identity provider for the platform. It runs as a container alongside the application and is the only component responsible for user authentication, token issuance, and identity federation. The application itself never stores passwords or manages sessions.

| Capability | How Provided |
| :---- | :---- |
| Protocol | OpenID Connect / OAuth 2.0 (PKCE flow for SPA client) |
| Token format | JWT (RS256 signed) |
| Institutional LDAP / AD | Keycloak User Federation — institutions configure their own directory |
| MFA | Keycloak built-in TOTP and WebAuthn |
| SSO between apps | Keycloak realm shared across services |
| On-prem | Keycloak runs as a container in the compose stack. Institutions manage their own realm. |

## **7.2 Authorisation Model**

Authorisation uses a combination of JWT claims and policy-based authorisation in ASP.NET Core. Keycloak issues tokens containing role claims and optionally resource-level permissions.

* Role claims are mapped to ASP.NET Core policies in YourApp.Infrastructure

* Module-level permissions are defined as constants in each Module.Core project (e.g., BillingPermissions.ViewInvoice)

* Handlers may perform additional fine-grained authorisation checks (e.g., "does this user own this resource") using a shared ICurrentUser service

* No authorisation logic in controllers or hub methods — only \[Authorize(Policy \= "...")\] attributes. Actual policy decisions live in handlers or dedicated authorization services.

## **7.3 SignalR Authentication**

SignalR cannot send the Authorization header during the WebSocket upgrade handshake. The standard pattern is:

10. Client retrieves a short-lived access token from Keycloak

11. Client passes token as query parameter: ?access\_token=... on the hub connection URL

12. Server reads this via OnMessageReceived event in JWT bearer options (standard ASP.NET pattern)

13. Token lifetime should be short (5-15 minutes); client refreshes transparently using refresh token

## **7.4 On-Prem Keycloak Setup**

The install script bootstraps a default Keycloak realm with a default admin account. The installing institution is expected to:

14. Change the default admin password immediately post-install

15. Optionally configure User Federation to their LDAP/AD server via the Keycloak admin console

16. Configure their realm's SMTP settings for email-based flows

Keycloak realm configuration is exported as a JSON file and committed to the repository. This ensures every fresh install starts from a known-good baseline configuration.

# **8\. Observability Stack**

Observability is not optional. Without it, debugging issues at an institution's server with no direct access is impossible. The stack covers the three pillars: logs, traces, and metrics.

## **8.1 Stack Components**

| Pillar | Technology & Role |
| :---- | :---- |
| Structured Logging | Serilog — enriched logs written to console (JSON) and optionally to Seq container. Log context includes correlation ID, module name, tenant, and user ID. |
| Distributed Tracing | OpenTelemetry SDK — automatic instrumentation for ASP.NET Core, EF Core, MassTransit, and HttpClient. Traces exported to Seq (via OTLP) or any OTLP-compatible backend. |
| Metrics | OpenTelemetry Metrics — standard ASP.NET runtime metrics plus custom business metrics per module. Exported to Seq or Prometheus. |
| Log & Trace UI | Seq — runs as a container in the compose stack. Provides full-text log search, trace visualisation, and alerting. Free for single-server. |
| Health Checks | ASP.NET Health Checks — /healthz (liveness) and /readyz (readiness). Checked by Docker Compose healthcheck and by Kubernetes probes if deployed on K8s. |

## **8.2 Correlation & Context Propagation**

Every request entering the system is assigned a correlation ID by YARP middleware. This ID propagates:

* As a structured log property (CorrelationId) in every log entry within the request scope

* As a trace context (W3C TraceContext header) through all downstream HTTP calls

* As a message header in all MassTransit messages, so a single originating request can be traced across module boundaries

* Back to the client in the X-Correlation-Id response header for support diagnostics

## **8.3 Module-Level Metrics**

Each module is responsible for publishing its own business metrics via OpenTelemetry. Examples:

* module.billing.invoices\_created (counter)

* module.billing.invoice\_processing\_duration (histogram)

* module.notifications.messages\_sent (counter, tagged by channel)

* module.{name}.queue\_depth (gauge, sourced from MassTransit diagnostic source)

Metric names follow the pattern: module.{module\_name}.{metric\_name}

## **8.4 Health Check Design**

Health checks are split into liveness and readiness:

* Liveness (/healthz): Is the process running? Checks: process is alive, no unrecoverable fault. Should never fail transiently.

* Readiness (/readyz): Is the process ready to serve traffic? Checks: DB connection, RabbitMQ connection, Keycloak reachability, Redis connection. Can fail transiently during startup.

* Each module may register its own readiness checks via the standard IHealthCheck interface

* Dead-letter queue depth per consumer is registered as a degraded (warning) health check

# **9\. External API Integration Pattern**

The system integrates with multiple third-party APIs. All such integrations follow the same pattern to ensure testability, replaceability, and resilience.

## **9.1 Structure per Integration**

`YourApp.Module.{Name}.Infrastructure/`

`└── ExternalApis/`
    `└── Stripe/`
        `├── IStripeClient.cs              <- Refit interface (HTTP contract)`
        `├── StripeApiSettings.cs           <- typed config (from appsettings)`
        `├── StripePaymentGateway.cs        <- implements IPaymentGateway (domain interface)`
        `└── StripeServiceRegistration.cs  <- extension method wiring Refit + Polly + DI`

## **9.2 Technology Choices**

* Refit: declares HTTP API as a C\# interface. Generates the implementation at compile time. Eliminates HttpClient boilerplate.

* Polly: resilience policies (retry, circuit breaker, timeout) are configured per integration in the registration extension method.

* Typed settings: each integration has its own settings class bound from the configuration system, never raw IConfiguration in business code.

## **9.3 Resilience Policy Standard**

Every external API integration must configure:

* Retry: 3 attempts with exponential backoff (1s, 2s, 4s) for transient HTTP errors (5xx, timeout)

* Circuit Breaker: open after 5 failures in 30 seconds, half-open after 60 seconds

* Timeout: 30 seconds per request maximum

* Fallback: if the circuit is open and the integration is non-critical (e.g., enrichment data), return a graceful degraded response. If critical, propagate a specific Error type.

## **9.4 Testability**

Because the domain interface (e.g., IPaymentGateway) is defined in Module.Core and the Refit implementation is in Module.Infrastructure, unit tests in the module's test project use in-memory fakes or Moq substitutes. Integration tests spin up WireMock.NET to record/replay real API responses.

# **10\. Containerisation Strategy**

## **10.1 Container Inventory**

| Container | Image | Role | Data Volume |
| :---- | :---- | :---- | :---- |
| api | yourapp/api:{version} | ASP.NET Core host (API \+ SignalR \+ YARP) | None (stateless) |
| web | yourapp/web:{version} | Web client static files (served by Nginx) | None |
| postgres | postgres:16-alpine | Primary database | pgdata (named volume) |
| rabbitmq | rabbitmq:3-management-alpine | Message broker | rabbitmq-data |
| redis | redis:7-alpine | SignalR backplane \+ app cache | None (ephemeral ok) |
| keycloak | quay.io/keycloak/keycloak:latest | Identity provider | keycloak-data |
| seq | datalust/seq:latest | Log & trace aggregation | seq-data |

## **10.2 API Dockerfile**

The API image uses a multi-stage build to keep the final image small and not ship SDK tooling to production:

`FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build`
`WORKDIR /src`
`COPY . .`
`RUN dotnet publish src/Host/YourApp.API/YourApp.API.csproj \`
    `-c Release -o /app/publish --no-self-contained`
`FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime`
`WORKDIR /app`
`COPY --from=build /app/publish .`
`HEALTHCHECK --interval=30s --timeout=10s --retries=3 \`
    `CMD curl -f http://localhost:8080/healthz || exit 1`
`ENTRYPOINT ["dotnet", "YourApp.API.dll"]`

## **10.3 Docker Compose Structure**

The compose file is split into base (infra/docker-compose.yml) and override (infra/docker-compose.override.yml) files:

* docker-compose.yml: production-ready service definitions, uses image references, no build instructions

* docker-compose.override.yml: development overrides — builds from source, mounts hot-reload volumes, exposes debug ports, uses in-memory Seq

* Environment variables: all sensitive values reference ${VARIABLE} resolved from a .env file. A .env.template is committed to source. The .env file is gitignored.

* Named volumes: all stateful data uses named Docker volumes, never bind mounts. This ensures portability across host OS and correct permissions.

## **10.4 Kubernetes Readiness**

All containers comply with 12-factor principles, making them Kubernetes-compatible without modification:

* Config via environment variables — no files needed at runtime beyond mounted secrets

* Stateless application tier — API containers can be replicated behind a load balancer

* Graceful shutdown — the API handles SIGTERM, drains in-flight requests, and stops consuming from MQ

* Health endpoints — /healthz and /readyz map directly to K8s liveness and readiness probes

* A Helm chart in infra/k8s/ provides Kubernetes deployment manifests as an alternative to Docker Compose for institutions with K8s infrastructure

# **11\. CI/CD Pipeline**

## **11.1 Pipeline Overview**

| Pipeline | Trigger | Jobs |
| :---- | :---- | :---- |
| ci.yml | Every pull request \+ push to any branch | Build, unit tests, architecture tests, format check, Docker build validation |
| cd-staging.yml | Merge to main branch | Build & push images to GHCR, tag with git SHA, deploy to staging environment |
| cd-release.yml | Push of version tag (v\*.\*.\*) | Build release images, tag with semver, publish release artifact ZIP to GitHub Releases |

## **11.2 CI Pipeline (ci.yml) Detail**

17. Checkout source

18. Setup .NET SDK (pinned version via global.json)

19. dotnet restore (cached by NuGet hash)

20. dotnet build \--no-restore \-c Release

21. dotnet test \--no-build — runs unit tests, architecture tests, and coverage collection

22. dotnet format \--verify-no-changes — fails PR if formatting is not applied

23. docker build for each image — validates Dockerfiles build without pushing

24. Upload coverage report to Codecov or GitHub Actions artifact

## **11.3 Architecture Tests**

Architecture tests live in tests/YourApp.ArchitectureTests/ and use NetArchTest.Rules or ArchUnitNET. They verify the project reference rules in Section 2.2 automatically on every CI run. Examples:

* Module.Core projects must not reference EF Core namespaces

* Module.Core projects must not reference other Module projects

* All public Handler classes must be internal (not leaked outside their module)

* All public API endpoints must require authorisation (no anonymous endpoints except health checks)

## **11.4 Release Artifact**

The cd-release.yml pipeline produces a versioned release ZIP containing:

* docker-compose.yml (pinned image tags for this release)

* .env.template (all required configuration variables with comments)

* install.sh / install.ps1

* upgrade.sh

* backup.sh

* CHANGELOG.md for this version

* README-install.md with step-by-step installation guide

## **11.5 Image Registry**

GitHub Container Registry (ghcr.io) is used as the image registry. Images are public or organisation-private depending on the project's visibility setting. Images are tagged with both git SHA (for traceability) and semver tag (for releases). The :latest tag is never used in compose files — always pin to a specific version.

# **12\. On-Premises Installation & Operations**

## **12.1 Prerequisites**

The only prerequisites required on the target server are:

* Docker Engine 24+ and Docker Compose v2

* curl (for healthcheck verification in install script)

* 2 GB RAM minimum, 4 GB recommended

* 10 GB disk minimum for initial install (database and log growth require more)

No .NET SDK, no Node.js, no Python, no database client. Everything runs in containers.

## **12.2 Install Script Flow**

The install.sh script performs the following steps in order:

25. Checks Docker and Docker Compose are installed and prints version

26. Copies .env.template to .env if .env does not already exist

27. Prompts for required configuration values (DB password, application secret, SMTP settings)

28. Pulls all images: docker compose pull

29. Starts infrastructure containers first (postgres, rabbitmq, redis, keycloak): docker compose up \-d postgres rabbitmq redis keycloak

30. Waits for health checks to pass on all infrastructure containers

31. Starts the API (runs DB migrations automatically on startup): docker compose up \-d api

32. Waits for /readyz to return 200

33. Starts remaining containers: web, seq

34. Prints installation summary: URLs, default admin credentials, first-login instructions

## **12.3 Upgrade Flow**

Upgrades are zero-downtime for the application tier. Database migrations are applied before old containers are stopped:

35. Download new release ZIP from GitHub Releases

36. Run upgrade.sh: pulls new images, runs migration-only container, then rolls containers one by one

37. If migration fails, the script aborts before stopping the running containers

38. Post-upgrade, runs /readyz health check to confirm success

## **12.4 Backup & Restore**

The backup.sh script produces a point-in-time backup:

* Postgres: pg\_dump via docker exec, output compressed to .sql.gz with timestamp

* Keycloak realm: exported via Keycloak admin REST API

* RabbitMQ: vhost definition exported (message data is transient and not backed up)

* Seq: data volume backup via docker cp

Restore: counterpart restore.sh takes a backup directory and restores all components in dependency order. Documented in README-install.md.

## **12.5 Air-Gapped Installations**

For institutions with no internet access:

* All images are included in the release ZIP as .tar files (docker save output)

* install.sh detects absence of internet and loads from .tar files via docker load instead of docker pull

* Keycloak and Seq update checks are disabled via environment variable

## **12.6 Multi-Instance Considerations**

Docker Compose on a single server is the primary deployment model. For institutions requiring high availability:

* The Helm chart in infra/k8s/ supports multi-replica API deployment — SignalR backplane (Redis) and stateless API design make this straightforward

* Postgres HA is outside the scope of this system's bundled infrastructure; institutions requiring it should provide an external Postgres connection string

# **13\. Secrets & Configuration Management**

## **13.1 Configuration Hierarchy**

ASP.NET Core's configuration system is used with the following source priority (highest wins):

39. Environment variables (set in docker-compose.yml from .env file)

40. appsettings.{Environment}.json (Development, Staging, Production)

41. appsettings.json (base defaults, no secrets)

## **13.2 Secret Categories**

| Secret | Storage | Notes |
| :---- | :---- | :---- |
| DB connection string | .env file on host | Never committed to source |
| RabbitMQ credentials | .env file on host | Rotate via upgrade script |
| Keycloak admin password | .env file on host | Changed post-install by institution |
| JWT signing key | Managed by Keycloak | Never handled by application code |
| External API keys | .env file on host | One variable per integration |
| Application encryption key | .env file on host | Used for field-level encryption if needed |

## **13.3 Rules**

* No secrets in source code, ever. Pre-commit hooks (using dotnet-secrets-scanner or git-secrets) enforce this.

* No secrets in appsettings.json. Defaults are empty strings or placeholders.

* The .env.template file documents every required variable with a description. It is the configuration contract between the application and the operator.

* For cloud deployments, .env variables are replaced by the cloud provider's secret manager (AWS Secrets Manager, Azure Key Vault) — the application code changes nothing because it reads environment variables either way.

# **14\. Testing Strategy**

## **14.1 Test Pyramid**

| Layer | What is tested | Tools |
| :---- | :---- | :---- |
| Unit Tests (per module) | Handler logic, domain entity behaviour, validators. No DB, no HTTP. | xUnit, FluentAssertions, Moq / NSubstitute |
| Integration Tests | Module handler \+ real DB (Postgres in container), real MassTransit in-memory transport. No external HTTP. | xUnit, Testcontainers, WireMock.NET |
| Architecture Tests | Project reference rules, naming conventions, no-anemic-domain-model checks. | NetArchTest.Rules or ArchUnitNET |
| End-to-End Tests (optional) | Full stack via HTTP: REST \+ SignalR client against running compose stack. | Playwright or custom SignalR test client |

## **14.2 Integration Test Approach**

Integration tests use Testcontainers (the .NET library) to spin up real Postgres and RabbitMQ containers during the test run. This means:

* Tests run against real infrastructure, not mocks — catching ORM and migration issues early

* Each test class gets a fresh database via EF Core's Respawn library (fast reset between tests)

* External HTTP APIs are replaced with WireMock.NET in-process servers

* MassTransit is configured with its in-memory test harness for unit-style consumer testing

## **14.3 Test Naming Convention**

All test method names follow: {Method}\_{Scenario}\_{ExpectedResult}

`CreateInvoiceHandler_WhenClientDoesNotExist_ReturnsNotFoundError()`
`InvoiceApprovedConsumer_WhenEventReceived_SendsNotificationEmail()`

# **15\. Extending the System: Adding a New Module**

This section is a procedural guide for adding a new module to the system. It is the primary workflow any developer after the original author will follow.

## **15.1 Checklist**

42. Create YourApp.Module.{Name}.Core project under src/Modules/YourApp.Module.{Name}/

43. Create YourApp.Module.{Name}.Infrastructure project in the same folder

44. Add project references per the rules in Section 2.2

45. Create the module's DbContext inheriting from DbContext, set default schema to module name (lowercase)

46. Add EF Core migration: dotnet ef migrations add InitialCreate \--project Module.{Name}.Infrastructure

47. Create the AddXxxModule(this IServiceCollection) extension method in Infrastructure

48. Register the module in YourApp.API's Program.cs

49. Add YARP route entries in appsettings.json if the module exposes new API paths

50. Define permissions constants in Module.Core and register policies in the extension method

51. Create tests/YourApp.Module.{Name}.Tests project

52. Update architecture tests to cover the new module

53. Add module-specific health checks and metrics (see Section 8\)

## **15.2 What You Must Not Do**

* Do not reference another module's Core or Infrastructure project from your new module

* Do not add tables to another module's schema

* Do not query across schemas in application code

* Do not put business logic in the API host project

* Do not skip writing architecture tests for the new module

# **16\. Extracting a Module to a Microservice**

When a module needs independent scaling, an independent deployment cadence, or separate team ownership, it can be extracted into a standalone service. The architecture was designed for this from day one.

## **16.1 Prerequisites for Extraction**

Before extracting, verify:

* The module has no direct code references from or to other modules (architecture tests enforce this)

* The module's schema contains no foreign keys referencing other modules' schemas

* All cross-module data needs are served via MassTransit events, not direct calls

* The module's external API integrations are self-contained in its Infrastructure project

## **16.2 Extraction Steps**

54. Create a new repository for the microservice

55. Move Module.{Name}.Core and Module.{Name}.Infrastructure into the new repo

56. Add a minimal ASP.NET Core host (clone the API project, remove all other module registrations)

57. Copy the Docker and CI/CD configuration, adjusting image names

58. In the original monolith, remove the module projects and their registrations

59. In YARP configuration, update the route for this module to point to the new service URL instead of in-process

60. Add the new service to the Docker Compose network so it can communicate with shared infrastructure

From the client's perspective, nothing changes. From the monolith's perspective, the module vanishes and YARP routes its traffic to a different address.

## **16.3 Data Separation**

After extraction, the module's schema may be moved to a separate database:

* Update the new service's connection string to point to a new database

* Run pg\_dump on the module's schema and restore to the new database

* Drop the schema from the original database once verified

* Any existing cross-schema read models that referenced this schema must be migrated to consume events instead

# **17\. Architecture Decision Records (ADRs)**

All significant architecture decisions must be recorded as ADRs in the docs/adr/ directory. This document itself represents the consolidated initial decision set. Future decisions that deviate from or extend this document require an ADR.

## **17.1 ADR Template**

`# ADR-{NNN}: {Title}`
`## Status`
`Proposed | Accepted | Deprecated | Superseded by ADR-{NNN}`
`## Context`
`What situation or problem prompted this decision?`
`## Decision`
`What was decided?`
`## Consequences`
`What becomes easier? What becomes harder?`
`What must change in the codebase as a result?`

## **17.2 Decisions Already Recorded by This Document**

| ADR | Decision |
| :---- | :---- |
| ADR-001 | Modular Monolith over Microservices as initial architecture |
| ADR-002 | Vertical Slice Architecture within each module |
| ADR-003 | MassTransit over raw RabbitMQ client |
| ADR-004 | YARP as in-process reverse proxy |
| ADR-005 | Keycloak for identity and federation |
| ADR-006 | Single Postgres instance with per-module schema isolation |
| ADR-007 | Reporting module as dedicated read model for cross-module queries |
| ADR-008 | Result\<T\> pattern over exceptions for expected errors |
| ADR-009 | OpenTelemetry \+ Serilog \+ Seq for observability |
| ADR-010 | Docker Compose as primary on-prem deployment, K8s via Helm as secondary |
| ADR-011 | GitHub Actions \+ GHCR for CI/CD and image registry |
| ADR-012 | Refit \+ Polly for all external HTTP integrations |

# **Appendix A: Technology Quick Reference**

| Concern | Technology | Version Policy | Notes |
| :---- | :---- | :---- | :---- |
| Web framework | ASP.NET Core 10 | Pin to LTS version | Minimal APIs preferred over controllers |
| In-process mediation | MediatR 12 | Latest stable |  |
| Validation | FluentValidation | Latest stable | Registered as MediatR pipeline behaviour |
| ORM | Entity Framework Core 10 | Match ASP.NET Core version | Code-first, Npgsql provider |
| Message broker | RabbitMQ 3 | Alpine image, pin minor |  |
| Message abstraction | MassTransit 8 | Latest stable | Configured in YourApp.Infrastructure |
| Reverse proxy | YARP 2 | Latest stable | Runs in-process |
| Auth provider | Keycloak 24+ | Pin major version | OpenID Connect, PKCE |
| Cache / backplane | Redis 7 | Alpine image | Shared by SignalR and app cache |
| Logging | Serilog | Latest stable | Seq sink for on-prem |
| Tracing \+ metrics | OpenTelemetry .NET | Latest stable | OTLP export to Seq |
| Log UI | Seq | Latest stable | Free for single server |
| HTTP client | Refit | Latest stable | All external APIs |
| Resilience | Polly 8 | Latest stable | Via Microsoft.Extensions.Http.Resilience |
| Testing | xUnit \+ FluentAssertions | Latest stable |  |
| Container testing | Testcontainers | Latest stable | Postgres \+ RabbitMQ in CI |
| Architecture testing | NetArchTest.Rules | Latest stable | Enforces module boundaries |
| CI/CD | GitHub Actions | N/A | GHCR for image registry |

*— End of Document —*