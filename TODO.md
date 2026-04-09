# Lex — Sequential Implementation Checklist
Phase 0 — Foundation (nothing works without this)
	∙	[x] Garage used as primary always - Finalize and write ADR-013 (object storage dual-provider design: PostgreSQL bytea + Garage, metadata model, auth model, migration path)
	∙	[x] Stabilize BlockContent model in SharedKernel — define the base block types (text, image, embedded diagram, custom block), versioning strategy, and serialization contract before any module consumes it
	∙	[x] Define Result<T> and Error types in SharedKernel with all error categories (NotFound, Validation, Forbidden, Conflict, Infrastructure)
	∙	[x] Define IDomainEvent base interface and event envelope (correlation ID, timestamp, source module) in SharedKernel
	∙	[x] Define ICurrentUser service interface in SharedKernel (userId, roles, tenantId)
	∙	[ ] Write IObjectStorageService / IFileStorageService adapter interfaces in SharedKernel per ADR-002
	∙	[ ] Implement PostgreSQL bytea storage provider in ObjectStorage module infrastructure
	∙	[ ] Implement Garage (S3-compatible) storage provider in ObjectStorage module infrastructure, behind the same adapter interface
	∙	[ ] Write file_records metadata table migration and repository in ObjectStorage module (provider-agnostic)
	∙	[ ] Implement FileController streaming endpoint — all file access mediated through API with permission check before byte delivery
	∙	[x] Write architecture tests covering all project reference rules from the architecture reference (this enforces the boundary rules on every CI run going forward)

Phase 1 — Auth & Identity wiring
	∙	[x] Write spec for Keycloak realm bootstrap — default realm JSON, client config, roles (Teacher, Admin, Student), claim mappings
	∙	[x] Define permission constants structure convention (one static class per module’s Core project)
	∙	[/] Spec and implement ICurrentUser concrete implementation reading from JWT claims in YourApp.Infrastructure
	∙	[x] Spec role-to-policy mapping in YourApp.Infrastructure (which roles map to which ASP.NET Core policy names)
	∙	[x] Write install.sh Keycloak bootstrap step spec — what the script must configure automatically on first run

Phase 2 — Scheduling module (foundational, everything references it)
Scheduling is load-bearing because DiaryManagement, LessonManagement, and AssessmentDelivery all depend on the concept of a scheduled period/slot.
	∙	[x] Spec Scheduling domain model — Period, Slot, Class, Classroom, AcademicYear, Term entities and their relationships
	∙	[x] Spec commands: CreateAcademicYear, CreateTerm, CreatePeriod, AssignClassToSlot, UpdateSlot
	∙	[x] Spec queries: GetScheduleForClass, GetScheduleForTeacher, GetPeriodsForDate
	∙	[x] Spec published events: SlotAssignedEvent, SlotRemovedEvent, AcademicYearCreatedEvent
	∙	[ ] Define Scheduling database schema (scheduling.*) and write initial EF Core migration
	∙	[x] Spec Scheduling API surface (REST routes, request/response shapes)
	∙	[x] Spec Scheduling frontend views — weekly timetable grid, slot detail, teacher view vs class view

Phase 3 — DiaryManagement module
	∙	[x] Spec DiaryEntry domain model — entry, subject, date, period reference (FK to Scheduling concept via ID only, no direct join), BlockContent body
	∙	[x] Spec commands: CreateDiaryEntry, UpdateDiaryEntry, SubmitDiaryEntry, ApproveDiaryEntry
	∙	[x] Spec queries: GetDiaryEntriesForClass, GetDiaryEntriesForDate, GetDiaryEntryById
	∙	[x] Spec published events: DiaryEntrySubmittedEvent, DiaryEntryApprovedEvent
	∙	[ ] Define DiaryManagement database schema (diary.*) and initial migration
	∙	[x] Spec file attachment flow — diary entries can have attachments; attachment upload/download routes through FileController in ObjectStorage
	∙	[x] Spec DiaryManagement API surface
	∙	[x] Spec DiaryManagement frontend views — diary feed, entry editor (Tiptap BlockContent renderer), submission flow, approval flow

Phase 4 — LessonManagement module
	∙	[x] Spec Lesson domain model — LessonPlan, LessonResource, Subject reference, Scheduling slot reference, BlockContent body
	∙	[x] Spec commands: CreateLessonPlan, UpdateLessonPlan, PublishLessonPlan, AttachResource
	∙	[x] Spec queries: GetLessonPlanById, GetLessonPlansForSubject, GetLessonPlansForPeriod
	∙	[x] Spec published events: LessonPlanPublishedEvent, LessonResourceAttachedEvent
	∙	[ ] Define LessonManagement database schema (lesson.*) and initial migration
	∙	[x] Spec LessonManagement API surface
	∙	[x] Spec LessonManagement frontend views — lesson plan editor (Tiptap), lesson library, resource panel

Phase 5 — AssessmentCreation module
	∙	[x] Spec Assessment domain model — Assessment, Question (with question type variants: MCQ, short answer, essay, file upload), Section, QuestionBank
	∙	[x] Spec commands: CreateAssessment, AddQuestion, UpdateQuestion, ReorderQuestions, PublishAssessment, ArchiveAssessment
	∙	[x] Spec queries: GetAssessmentById, GetAssessmentQuestions, GetQuestionBank
	∙	[x] Spec snapshot contract — exactly what gets snapshotted at publish time and the format AssessmentDelivery will consume (this is the handoff contract between the two modules)
	∙	[x] Spec published events: AssessmentPublishedEvent (carries the full snapshot payload)
	∙	[ ] Define AssessmentCreation database schema (assessment_creation.*) and initial migration
	∙	[x] Spec AssessmentCreation API surface
	∙	[x] Spec AssessmentCreation frontend views — assessment builder (drag-and-drop question ordering with React Flow or custom list), question editor, question bank, publish flow

Phase 6 — AssessmentDelivery module
	∙	[x] Spec AssessmentDelivery domain model — DeliverySession, Submission, SubmissionAnswer, SubmissionStatus (in-progress, submitted, graded)
	∙	[x] Spec commands: StartDeliverySession, SubmitAnswer, FinishSubmission
	∙	[x] Spec queries: GetActiveSession, GetSubmissionSummary
	∙	[x] Spec published events: AssessmentAttemptFinishedEvent
	∙	[ ] Define AssessmentDelivery database schema (assessment_delivery.*) and initial migration
	∙	[x] Spec auto-grading logic and manual grading workflow — how MCQ is graded automatically vs Essay manually (Note: Timed assessments skipped per user request)
	∙	[x] Spec AssessmentDelivery API surface
	∙	[x] Spec AssessmentDelivery frontend views: Secure student player, Proctor dashboard

Phase 7 — FileProcessing module
	∙	[x] Spec FileProcessing domain model — ProcessingJob, ProcessingStatus, ProcessingResult
	∙	[x] Spec what processing means for PDF text extraction.
	∙	[x] Spec commands: EnqueueProcessingJob (triggered by FileUploadedEvent from ObjectStorage)
	∙	[x] Spec published events: FileProcessingCompletedEvent, FileProcessingFailedEvent
	∙	[x] Spec async job pattern integration — long-running jobs use the 202 Accepted + SignalR push pattern
	∙	[ ] Define FileProcessing database schema (file_processing.*) and initial migration
	∙	[x] Spec FileProcessing API surface
	∙	[x] Spec FileProcessing frontend views — processing status icons on file cards, job details dashboard

Phase 8 — Notifications module (cross-cutting)
	∙	[x] Spec Notification domain model — Notification, NotificationChannel (in-app, email), NotificationTemplate, DeliveryStatus
	∙	[x] Spec which domain events trigger notifications and what each notification says — map every *Event already specced to zero or more notification rules
	∙	[x] Spec in-app notification delivery path (SignalR push to NotificationHub)
	∙	[x] Spec email notification delivery path (SMTP via configured provider)
	∙	[x] Spec commands: MarkNotificationRead, MarkAllRead, UpdateNotificationPreferences
	∙	[x] Spec queries: GetNotificationsForUser (paged), GetUnreadCount
	∙	[ ] Define Notifications database schema (notifications.*) and initial migration
	∙	[x] Spec Notifications API surface
	∙	[x] Spec Notifications frontend views — notification drawer/bell, notification list, preference settings

Phase 9 — Reporting module (cross-cutting read model)
	∙	[x] Spec which cross-module views are needed — list every dashboard, summary table, or report that requires data from more than one module
	∙	[x] Spec read model tables for each cross-module view (denormalized, event-sourced from domain events)
	∙	[x] Spec consumer list — one consumer per source event per read model that needs updating
	∙	[x] Spec queries: all report/dashboard query handlers (read from reporting.* schema only)
	∙	[ ] Define Reporting database schema (reporting.*) and initial migration
	∙	[x] Spec Reporting API surface
	∙	[x] Spec Reporting frontend views — dashboard cards, summary tables, charts (if any)

Phase 10 — GoogleIntegration module
	∙	[x] Spec adapter interface IGoogleCalendarService, IGoogleDriveService in GoogleIntegration Core (pure adapter, no domain logic)
	∙	[x] Spec OAuth2 flow for per-teacher Google account linking (authorization code flow, token storage, refresh)
	∙	[x] Spec Calendar sync — which Scheduling or DiaryManagement events trigger calendar writes
	∙	[x] Spec Drive import/export — what can be imported from Drive (lesson resources, assessment files) and exported to Drive
	∙	[x] Spec published events: GoogleSyncCompletedEvent, GoogleImportCompletedEvent
	∙	[x] Spec resilience policy — circuit breaker, retry, graceful degradation when Google is unavailable (on-prem with no internet must not break)
	∙	[ ] Define GoogleIntegration database schema (google_integration.*) and initial migration (token storage)
	∙	[x] Spec GoogleIntegration API surface (OAuth callback, sync trigger, status)
	∙	[x] Spec GoogleIntegration frontend views — account link/unlink, sync status, import file picker

Phase 11 — ImportExport module
	∙	[x] Spec what can be imported: lesson plans, assessments, diary templates — from CSV, DOCX, PDF, or Google Drive
	∙	[x] Spec what can be exported: the same, plus results/reports as CSV or PDF
	∙	[x] Spec orchestration flow — ImportExport is the most complex integration surface; map its event dependencies on FileProcessing, GoogleIntegration, and ObjectStorage explicitly
	∙	[x] Spec commands: StartImport, StartExport, CancelJob
	∙	[x] Spec published events: ImportCompletedEvent, ImportFailedEvent, ExportReadyEvent
	∙	[x] Spec async job pattern — all import/export jobs use 202 Accepted + SignalR push for progress
	∙	[ ] Define ImportExport database schema (import_export.*) and initial migration
	∙	[x] Spec ImportExport API surface
	∙	[x] Spec ImportExport frontend views — import wizard, export options, job progress indicator, download link

Phase 12 — Frontend infrastructure (can be parallelized with Phase 1–3)
	∙	[x] Spec and implement apiFetch wrapper in lib/api/client.ts with token attachment, ProblemDetails error normalization, 401 silent refresh, 403 handling
	∙	[x] Spec and implement tokenManager in lib/auth/ (in-memory access token, httpOnly refresh cookie, silent refresh lifecycle)
	∙	[x] Spec and implement middleware.ts auth guard — protected vs public routes, Keycloak redirect
	∙	[x] Spec and implement SignalRProvider and connectionManager.ts — hub lifecycle, reconnect, token refresh on reconnect, cache invalidation on reconnect
	∙	[x] Spec and implement useAuthStore (Zustand) — current user identity, decoded JWT claims, auth status
	∙	[x] Spec and implement useSignalRStore (Zustand) — connection status per hub
	∙	[x] Spec and implement usePermissions() hook — reads claims from auth store, exposes permission check interface
	∙	[x] Spec and implement useAsyncJob() hook — subscribes to JobStatusHub, returns status for a given jobId
	∙	[x] Spec and implement app shell layout — nav, sidebar, notification bell, user menu, connection status indicator
	∙	[x] Spec and implement PWA manifest and service worker — icons, display: standalone, cache strategies, offline fallback, update banner
	∙	[x] Spec and implement DocumentEditor component (Tiptap) — toolbar, auto-save hook, save status display, DiagramBlock extension
	∙	[x] Spec and implement DiagramCanvas component (React Flow) — base canvas, toolbar, custom node chrome, useDiagramSync for SignalR updates
	∙	[x] Spec and implement PresentationViewer component — full-screen shell, keyboard nav, Framer Motion transitions, slide renderer
	∙	[x] Spec MSW handler baseline — mirror every API endpoint as it’s specced, so frontend tests are never blocked by backend availability

Phase 13 — Observability & ops hardening
	∙	[x] Spec custom business metrics per module — list every counter, histogram, and gauge each module should emit (follow module.{name}.{metric} convention)
	∙	[x] Spec dead-letter queue health check — one per MassTransit consumer, surfaced as degraded health check
	∙	[x] Spec module-level readiness checks — what each module checks in /readyz (its own DB schema, its own consumers)
	∙	[x] Write backup.sh spec — pg_dump per-module schema coverage, Keycloak realm export, Seq volume, restore ordering
	∙	[x] Write upgrade.sh spec — migration-first ordering, rollback abort condition, post-upgrade readiness check
	∙	[x] Spec air-gapped install — docker save/load flow, offline Keycloak/Seq config, verification step
	∙	[x] Write end-to-end install walkthrough (README-install.md spec) — what an institution operator sees from download to first login

Phase 14 — Integration and E2E tests
	∙	[x] Write integration test baseline for each module: real Postgres via Testcontainers, MassTransit in-memory harness, WireMock for external HTTP
	∙	[x] Write cross-module integration tests for the critical event chains: DiaryEntry submitted → Notification sent, Assessment published → Delivery session created, File uploaded → FileProcessing completed
	∙	[x] Write ImportExport integration tests — the most complex orchestration surface; cover happy path and failure/retry paths
	∙	[x] Write Playwright E2E smoke tests (optional but high value): login → create diary entry → submit → verify notification received
