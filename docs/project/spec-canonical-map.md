# Spec Canonical Map

This file maps legacy agent spec documents to the canonical locations that should now be used.

The archived legacy copies now live under `docs/project/legacy-agent-specs/`.

## Module Specs

| Legacy path | Canonical path |
|---|---|
| `.agents/specs/modules/scheduling.md` | [src/Modules/Lex.Module.Scheduling/module-spec.md](../../src/Modules/Lex.Module.Scheduling/module-spec.md) |
| `.agents/specs/modules/assessment-creation.md` | [src/Modules/Lex.Module.AssessmentCreation/module-spec.md](../../src/Modules/Lex.Module.AssessmentCreation/module-spec.md) |
| `.agents/specs/modules/assessment-delivery.md` | [src/Modules/Lex.Module.AssessmentDelivery/module-spec.md](../../src/Modules/Lex.Module.AssessmentDelivery/module-spec.md) |
| `.agents/specs/modules/diary-management.md` | [src/Modules/Lex.Module.DiaryManagement/module-spec.md](../../src/Modules/Lex.Module.DiaryManagement/module-spec.md) |
| `.agents/specs/modules/file-processing.md` | [src/Modules/Lex.Module.FileProcessing/module-spec.md](../../src/Modules/Lex.Module.FileProcessing/module-spec.md) |
| `.agents/specs/modules/google-integration.md` | [src/Modules/Lex.Module.GoogleIntegration/module-spec.md](../../src/Modules/Lex.Module.GoogleIntegration/module-spec.md) |
| `.agents/specs/modules/import-export.md` | [src/Modules/Lex.Module.ImportExport/module-spec.md](../../src/Modules/Lex.Module.ImportExport/module-spec.md) |
| `.agents/specs/modules/lesson-management.md` | [src/Modules/Lex.Module.LessonManagement/module-spec.md](../../src/Modules/Lex.Module.LessonManagement/module-spec.md) |
| `.agents/specs/modules/notifications.md` | [src/Modules/Lex.Module.Notifications/module-spec.md](../../src/Modules/Lex.Module.Notifications/module-spec.md) |
| `.agents/specs/modules/reporting.md` | [src/Modules/Lex.Module.Reporting/module-spec.md](../../src/Modules/Lex.Module.Reporting/module-spec.md) |

## Domain Specs

| Legacy path | Canonical path |
|---|---|
| `.agents/specs/modules/scheduling-domain.md` | [src/Modules/Lex.Module.Scheduling/Lex.Module.Scheduling.Core/Domain/SchedulingDomain.spec.md](../../src/Modules/Lex.Module.Scheduling/Lex.Module.Scheduling.Core/Domain/SchedulingDomain.spec.md) |
| `.agents/specs/modules/assessment-domain.md` | [src/Modules/Lex.Module.AssessmentCreation/Lex.Module.AssessmentCreation.Core/Domain/AssessmentDomain.spec.md](../../src/Modules/Lex.Module.AssessmentCreation/Lex.Module.AssessmentCreation.Core/Domain/AssessmentDomain.spec.md) |
| `.agents/specs/modules/assessment-delivery-domain.md` | [src/Modules/Lex.Module.AssessmentDelivery/Lex.Module.AssessmentDelivery.Core/Domain/DeliveryDomain.spec.md](../../src/Modules/Lex.Module.AssessmentDelivery/Lex.Module.AssessmentDelivery.Core/Domain/DeliveryDomain.spec.md) |
| `.agents/specs/modules/diary-domain.md` | [src/Modules/Lex.Module.DiaryManagement/Lex.Module.DiaryManagement.Core/Domain/DiaryDomain.spec.md](../../src/Modules/Lex.Module.DiaryManagement/Lex.Module.DiaryManagement.Core/Domain/DiaryDomain.spec.md) |
| `.agents/specs/modules/file-processing-domain.md` | [src/Modules/Lex.Module.FileProcessing/Lex.Module.FileProcessing.Core/Domain/ProcessingDomain.spec.md](../../src/Modules/Lex.Module.FileProcessing/Lex.Module.FileProcessing.Core/Domain/ProcessingDomain.spec.md) |
| `.agents/specs/modules/lesson-domain.md` | [src/Modules/Lex.Module.LessonManagement/Lex.Module.LessonManagement.Core/Domain/LessonDomain.spec.md](../../src/Modules/Lex.Module.LessonManagement/Lex.Module.LessonManagement.Core/Domain/LessonDomain.spec.md) |
| `.agents/specs/modules/notifications-domain.md` | [src/Modules/Lex.Module.Notifications/Lex.Module.Notifications.Core/Domain/NotificationDomain.spec.md](../../src/Modules/Lex.Module.Notifications/Lex.Module.Notifications.Core/Domain/NotificationDomain.spec.md) |

## Feature Specs

| Legacy path | Canonical path |
|---|---|
| `.agents/specs/features/assign-slot.md` | [src/Modules/Lex.Module.Scheduling/Lex.Module.Scheduling.Core/Features/AssignClassToSlot/spec.md](../../src/Modules/Lex.Module.Scheduling/Lex.Module.Scheduling.Core/Features/AssignClassToSlot/spec.md) |
| `.agents/specs/features/create-academic-year.md` | [src/Modules/Lex.Module.Scheduling/Lex.Module.Scheduling.Core/Features/CreateAcademicYear/spec.md](../../src/Modules/Lex.Module.Scheduling/Lex.Module.Scheduling.Core/Features/CreateAcademicYear/spec.md) |
| `.agents/specs/features/get-schedule.md` | [src/Modules/Lex.Module.Scheduling/Lex.Module.Scheduling.Core/Features/QuerySchedule/spec.md](../../src/Modules/Lex.Module.Scheduling/Lex.Module.Scheduling.Core/Features/QuerySchedule/spec.md) |
| `.agents/specs/features/manage-diary-entry.md` | [src/Modules/Lex.Module.DiaryManagement/Lex.Module.DiaryManagement.Core/Features/ManageDiaryEntry/spec.md](../../src/Modules/Lex.Module.DiaryManagement/Lex.Module.DiaryManagement.Core/Features/ManageDiaryEntry/spec.md) |
| `.agents/specs/features/get-diary-entries.md` | [src/Modules/Lex.Module.DiaryManagement/Lex.Module.DiaryManagement.Core/Features/GetDiaryEntriesForDate/spec.md](../../src/Modules/Lex.Module.DiaryManagement/Lex.Module.DiaryManagement.Core/Features/GetDiaryEntriesForDate/spec.md) and [GetDiaryEntriesForClass/spec.md](../../src/Modules/Lex.Module.DiaryManagement/Lex.Module.DiaryManagement.Core/Features/GetDiaryEntriesForClass/spec.md) |
| `.agents/specs/features/author-lesson-plan.md` | [src/Modules/Lex.Module.LessonManagement/Lex.Module.LessonManagement.Core/Features/CreateLessonPlan/spec.md](../../src/Modules/Lex.Module.LessonManagement/Lex.Module.LessonManagement.Core/Features/CreateLessonPlan/spec.md) and related lesson-management feature specs |
| `.agents/specs/features/get-lessons.md` | [src/Modules/Lex.Module.LessonManagement/Lex.Module.LessonManagement.Core/Features/QueryLessonPlan/spec.md](../../src/Modules/Lex.Module.LessonManagement/Lex.Module.LessonManagement.Core/Features/QueryLessonPlan/spec.md) |
| `.agents/specs/features/build-assessment.md` | [src/Modules/Lex.Module.AssessmentCreation/Lex.Module.AssessmentCreation.Core/Features/CreateAssessment/spec.md](../../src/Modules/Lex.Module.AssessmentCreation/Lex.Module.AssessmentCreation.Core/Features/CreateAssessment/spec.md) and related question-management specs |
| `.agents/specs/features/take-assessment.md` | [src/Modules/Lex.Module.AssessmentDelivery/Lex.Module.AssessmentDelivery.Core/Features/StartSession/spec.md](../../src/Modules/Lex.Module.AssessmentDelivery/Lex.Module.AssessmentDelivery.Core/Features/StartSession/spec.md) and related delivery specs |
| `.agents/specs/features/diary-workflow.md` | [docs/specs/diary-attachment-flow.spec.md](../specs/diary-attachment-flow.spec.md) and the module-local diary feature specs |

## Infrastructure Specs

| Legacy path | Canonical path |
|---|---|
| `.agents/specs/infrastructure/auth-identity.md` | [docs/AUTHENTICATION.md](../AUTHENTICATION.md) and [docs/specs/auth/](../specs/auth) |
| `.agents/specs/infrastructure/frontend-foundation.md` | [docs/specs/frontend-foundation.spec.md](../specs/frontend-foundation.spec.md) and [docs/web-client-architecture.md](../web-client-architecture.md) |
| `.agents/specs/infrastructure/keycloak-bootstrap.md` | [docs/specs/keycloak-bootstrap.spec.md](../specs/keycloak-bootstrap.spec.md) |
| `.agents/specs/infrastructure/ops-observability.md` | [docs/specs/observability-metrics.spec.md](../specs/observability-metrics.spec.md) and [docs/specs/ops-health-resilience.spec.md](../specs/ops-health-resilience.spec.md) |
| `.agents/specs/infrastructure/reporting-read-models.md` | [docs/specs/reporting-readmodel-sync.spec.md](../specs/reporting-readmodel-sync.spec.md) |

## Decision

Legacy agent spec files are no longer authoritative. The canonical sources are the paths above.
