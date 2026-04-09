# Infrastructure Specification: [Component Name]

## 1. Overview
- **Category**: (External API Client | DB Repository implementation | Message Consumer | File Provider)
- **Parent Module**: [e.g., ObjectStorage]
- **Component Name**: [e.g., StripePaymentGateway]

## 2. Technical Strategy
- **Base Technology**: [e.g., Refit, EF Core, Polly, AWS SDK]
- **Storage/Transport**: [e.g., PostgreSQL, RabbitMQ, Garage S3]

## 3. Implementation Details
Briefly describe the implementation strategy. Use C# comments for class-level technical documentation.

- **Interface Realized**: [Name of the Core interface being implemented]
- **Classes/Interfaces**:
    - **[Name]**: [Brief role description]

## 4. Configuration & Settings
- **Settings Class**: `[Name]Settings`
- **Key in `appsettings.json`**: `[Path.To.Key]`
- **Required Secrets**: [e.g., API Key, Client Secret]

## 5. Resilience & Performance
- **Polly Policies**: [Retry, Circuit Breaker configuration]
- **Caching Strategy**: [Redis usage, TTL]
- **Timeout Model**: [Standard vs. Long-running]

## 6. Integration Points
- **Health Checks**: [How is this component's health verified?]
- **Metrics/Logging**: [Specific metrics emitted (module.name.metric)]

## 7. Migration & Deployment
- **Database Migrations**: [Schema affected]
- **Post-Deploy Verification**: [Steps to ensure it works]
