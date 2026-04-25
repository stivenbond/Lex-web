# Implementation Plan: Lex Web Platform

## Overview

This document provides a detailed, sequential implementation plan for the entire Lex Web platform codebase. The plan follows the architectural principles outlined in `docs/architecture.md` and `docs/web-client-architecture.md`, ensuring a modular monolith structure with vertical slices, event-driven communication, and end-to-end completion of each component before progressing.

The implementation is organized into phases that respect the dependency tree, preventing conflicts and ensuring each module can be developed, tested, and integrated independently. Each phase includes:

- **Backend Implementation**: Core domain logic, infrastructure, and API endpoints
- **Frontend Implementation**: UI components, state management, and integration
- **Testing**: Unit, integration, and cross-module tests
- **Documentation**: Updates to specs and architecture docs
- **Verification**: End-to-end testing and deployment validation

## Dependency Analysis

Based on the specifications in `docs/specs/`, the module dependencies are:

### Foundation Dependencies
- **SharedKernel**: Base abstractions (no dependencies)
- **Infrastructure**: Cross-cutting concerns (depends on SharedKernel)
- **Host/API**: Entry point (depends on Infrastructure and all modules)
- **Auth Foundation**: Keycloak realm, permissions (foundation for all authenticated features)

### Module Dependencies
- **ObjectStorage**: Independent (provides storage abstraction)
- **FileProcessing**: Independent (processes files for other modules)
- **GoogleIntegration**: Independent (external API integration)
- **AssessmentCreation**: Independent (creates assessments)
- **DiaryManagement**: Independent (manages diary entries)
- **LessonManagement**: Independent (manages lessons)
- **Scheduling**: Independent (manages scheduling)
- **AssessmentDelivery**: Depends on AssessmentCreation (consumes assessment snapshots)
- **ImportExport**: Depends on GoogleIntegration, ObjectStorage, FileProcessing, and domain modules (LessonManagement, DiaryManagement)
- **Notifications**: Depends on DiaryManagement, Scheduling, LessonManagement, AssessmentCreation/Delivery, FileProcessing
- **Reporting**: Depends on LessonManagement, AssessmentDelivery, DiaryManagement

### Cross-Cutting Dependencies
- **Async Job Pattern**: Implemented in FileProcessing, used by ImportExport and Reporting
- **Frontend Foundation**: Base for all client features
- **Testing Infrastructure**: Architecture tests, integration baselines, cross-module scenarios

## Implementation Phases

### Phase 1: Foundation Infrastructure

**Goal**: Establish the core platform infrastructure that all modules depend on.

#### 1.1 SharedKernel Implementation
**Spec**: `docs/specs/foundation/`
- Implement base entities, aggregates, domain events, Result<T> pattern
- Implement common value objects and shared contracts
- Create NuGet package structure for cross-module contracts
- **Verification**: Architecture tests pass for project reference rules

#### 1.2 Infrastructure Setup
**Spec**: `docs/specs/foundation/`, `docs/specs/async-job-pattern.spec.md`
- Implement EF Core base configurations and conventions
- Set up MassTransit bus configuration
- Implement Serilog, OpenTelemetry, YARP reverse proxy
- Implement async job pattern infrastructure (SignalR hub, job status tracking)
- **Verification**: Cross-cutting middleware functional, health checks pass

#### 1.3 Host/API Setup
**Spec**: `docs/architecture.md` Section 2.3
- Create ASP.NET Core entry point with minimal API routing
- Implement module registration pattern (AddXxxModule extensions)
- Set up SignalR hubs and YARP configuration
- **Verification**: Application starts, health endpoints respond

#### 1.4 Authentication Foundation
**Spec**: `docs/specs/auth/`, `docs/specs/keycloak-bootstrap.spec.md`, `docs/specs/keycloak-realm.spec.md`
- Configure Keycloak realm with roles and clients
- Implement OIDC middleware and token management
- Set up permission system and role mappings
- **Verification**: Login flow works, tokens are validated

#### 1.5 Frontend Foundation
**Spec**: `docs/specs/frontend-foundation.spec.md`, `docs/specs/frontend-auth-api.spec.md`
- Set up Next.js 15+ with App Router and React 19
- Implement Tailwind CSS + shadcn/ui component system
- Set up Zustand for global state, TanStack Query for server state
- Implement authentication layer (tokenManager, apiFetch wrapper, middleware)
- Create app shell and routing structure
- **Verification**: App loads, authentication redirects work

#### 1.6 Architecture Tests
**Spec**: `docs/specs/foundation/ArchitectureTests.spec.md`
- Implement NetArchTest.Rules or ArchUnitNET rules
- Enforce project reference boundaries
- Set up CI pipeline integration
- **Verification**: All foundation projects pass boundary checks

### Phase 2: Independent Core Modules

**Goal**: Implement modules with no external dependencies.

#### 2.1 ObjectStorage Module
**Spec**: `docs/specs/foundation/ObjectStorageAbstractions.spec.md`
- **Backend**: Implement IObjectStorageService interface, repository pattern
- **Infrastructure**: EF Core context for metadata, file storage implementation
- **API**: Upload/download endpoints
- **Frontend**: File upload components, storage integration
- **Tests**: Unit tests, integration tests with Testcontainers
- **Verification**: Files can be uploaded/downloaded, storage abstraction works

#### 2.2 FileProcessing Module
**Spec**: `docs/specs/async-job-pattern.spec.md`
- **Backend**: Implement file processing handlers (PDF text extraction, DOCX parsing)
- **Infrastructure**: MassTransit consumers, job status tracking
- **API**: File processing endpoints with async job pattern
- **Frontend**: File processing UI with progress indicators, SignalR integration
- **Tests**: Processing accuracy tests, async job flow tests
- **Verification**: Files are processed correctly, progress updates work

#### 2.3 GoogleIntegration Module
**Spec**: `docs/specs/google-calendar-sync.spec.md`, `docs/specs/google-drive-integration.spec.md`, `docs/specs/google-integration-resilience.spec.md`
- **Backend**: Implement Google API clients (Drive, Calendar)
- **Infrastructure**: Polly resilience policies, token encryption
- **API**: Google sync endpoints
- **Frontend**: Google Picker integration, sync status UI
- **Tests**: API mocking with WireMock, resilience testing
- **Verification**: Google APIs integrate successfully, failures are handled gracefully

#### 2.4 AssessmentCreation Module
**Spec**: `docs/specs/assessment-snapshot-contract.spec.md`
- **Backend**: Assessment authoring domain, snapshot creation
- **Infrastructure**: EF Core context, event publishing
- **API**: Assessment CRUD endpoints
- **Frontend**: Assessment builder UI, rich text editing
- **Tests**: Domain logic tests, snapshot contract validation
- **Verification**: Assessments can be created and published as snapshots

#### 2.5 DiaryManagement Module
**Spec**: `docs/specs/diary-attachment-flow.spec.md`
- **Backend**: Diary entry domain, approval workflow
- **Infrastructure**: EF Core context, attachment handling via ObjectStorage
- **API**: Diary CRUD endpoints
- **Frontend**: Diary entry forms, approval UI
- **Tests**: Workflow tests, attachment integration
- **Verification**: Diary entries can be submitted and approved

#### 2.6 LessonManagement Module
**Spec**: (Inferred from usage in other specs)
- **Backend**: Lesson planning domain, resource management
- **Infrastructure**: EF Core context, content blocks
- **API**: Lesson CRUD endpoints
- **Frontend**: Lesson editor UI, content block components
- **Tests**: Content management tests
- **Verification**: Lessons can be created and published

#### 2.7 Scheduling Module
**Spec**: (Inferred from usage in notification-event-mapping.spec.md)
- **Backend**: Calendar/scheduling domain
- **Infrastructure**: EF Core context, calendar integration
- **API**: Scheduling endpoints
- **Frontend**: Calendar UI, slot management
- **Tests**: Scheduling logic tests
- **Verification**: Schedules can be created and slots assigned

### Phase 3: Dependent Core Modules

**Goal**: Implement modules that depend on Phase 2 modules.

#### 3.1 AssessmentDelivery Module
**Spec**: `docs/specs/assessment-grading-workflow.spec.md`
- **Backend**: Assessment delivery domain, submission handling, grading logic
- **Infrastructure**: EF Core context, consumers for AssessmentPublishedEvent
- **API**: Delivery session endpoints, submission endpoints
- **Frontend**: Assessment taking UI, proctor dashboard
- **Tests**: Delivery workflow tests, grading accuracy
- **Verification**: Full assessment lifecycle from creation to grading

#### 3.2 ImportExport Module
**Spec**: `docs/specs/import-export-orchestration.spec.md`
- **Backend**: Import orchestration domain, format parsers
- **Infrastructure**: MassTransit consumers, bulk command dispatching
- **API**: Import job endpoints
- **Frontend**: Import wizard UI, mapping interface
- **Tests**: End-to-end import flows, error handling
- **Verification**: Data can be imported from external sources into domain modules

### Phase 4: Cross-Cutting Modules

**Goal**: Implement modules that integrate across multiple other modules.

#### 4.1 Notifications Module
**Spec**: `docs/specs/notification-event-mapping.spec.md`
- **Backend**: Notification domain, template system
- **Infrastructure**: MassTransit consumers for all mapped events
- **API**: Notification preference endpoints
- **Frontend**: Notification UI, preference settings
- **Tests**: Event-to-notification mapping tests
- **Verification**: Notifications are sent for all specified events

#### 4.2 Reporting Module
**Spec**: `docs/specs/reporting-readmodel-sync.spec.md`
- **Backend**: Read model projections, aggregation logic
- **Infrastructure**: MassTransit consumers, dedicated reporting DB context
- **API**: Report query endpoints
- **Frontend**: Dashboard components, report visualizations
- **Tests**: Projection accuracy tests, rebuild functionality
- **Verification**: Reports reflect current system state

### Phase 5: Testing Infrastructure

**Goal**: Comprehensive testing across all modules.

#### 5.1 Integration Testing Baseline
**Spec**: `docs/specs/testing-integration-baseline.spec.md`
- Implement Testcontainers setup for all modules
- Create integration test base classes
- Implement handler testing patterns
- **Verification**: All module vertical slices tested end-to-end

#### 5.2 Cross-Module Integration Tests
**Spec**: `docs/specs/testing-cross-module.spec.md`
- Implement MassTransit test harness
- Create cross-module scenario tests (Diary-Notification, Assessment-Delivery, Import flows)
- **Verification**: Event-driven communication works across module boundaries

#### 5.3 Frontend Testing
**Spec**: `docs/specs/frontend-testing-baseline.spec.md`, `docs/specs/testing-playwright.spec.md`
- Set up Vitest for unit tests
- Implement Playwright for E2E tests
- Create component testing patterns
- **Verification**: Frontend features are fully tested

### Phase 6: Documentation and Deployment

**Goal**: Complete documentation and operational readiness.

#### 6.1 Documentation Updates
- Update architecture.md with implementation details
- Create API documentation
- Update deployment guides
- **Verification**: All specs are implemented and documented

#### 6.2 Deployment Automation
**Spec**: `docs/specs/ops-deployment-automation.spec.md`
- Implement Docker Compose configuration
- Create deployment scripts
- Set up CI/CD pipelines
- **Verification**: Platform can be deployed on-prem

#### 6.3 Observability and Health
**Spec**: `docs/specs/observability-metrics.spec.md`, `docs/specs/ops-health-resilience.spec.md`
- Implement metrics collection
- Set up health checks
- Configure logging and monitoring
- **Verification**: Platform is observable and resilient

## Implementation Guidelines

### End-to-End Completion Requirement
- **No Partial Implementation**: Each module must be fully implemented (backend, frontend, tests) before moving to dependent modules
- **Integration First**: Always implement and test integration points before assuming they work
- **Vertical Slices**: Within each module, complete full features (API → Domain → UI) before starting the next feature

### Quality Gates
- **Code Review**: All code must pass architecture tests and linting
- **Testing**: 80%+ code coverage, all integration tests passing
- **Documentation**: Specs updated, API documented
- **Demo**: Working end-to-end feature demonstration

### Risk Mitigation
- **Dependency Order**: Strict adherence to the dependency tree prevents circular references
- **Incremental Delivery**: Each phase delivers working software
- **Rollback Plan**: Each module can be disabled if issues arise

### Timeline Considerations
- **Foundation**: 2-3 weeks (critical path)
- **Independent Modules**: 1-2 weeks each (parallelizable)
- **Dependent Modules**: 1 week each
- **Cross-Cutting**: 1-2 weeks
- **Testing & Deployment**: 2 weeks

This plan ensures systematic, conflict-free development while maintaining architectural integrity and delivering working software at each milestone.