# Specification: Cross-Module Integration Scenarios

## 1. Overview
- **Category**: Orchestration Testing
- **Scope**: Verification of the event-driven communication between multiple isolated modules.

## 2. Infrastructure: MassTransit Test Harness
- **Standard**: Tests use the `MassTransit-Test-Harness` for real MQ testing but within a single test suite.
- **Verification Logic**: "When X happens in Module A, wait up to 5s for Y to happen in Module B."

## 3. High-Value Test Chains

### Scenario 1: The "Diary-Notification" Loop
1.  **Trigger**: `PostDiaryEntryCommand` in the `DiaryManagement` module.
2.  **Intermediate**: Module A publishes `DiaryEntryCreatedEvent`.
3.  **Observation**: The `NotificationConsumer` in the `Notifications` module receives the message.
4.  **Observation**: A new record is created in the `notifications.alerts` table.
5.  **Verify**: The SMTP mock receives an email payload for the targeted recipient.

### Scenario 2: The "Assessment-Delivery" Handover
1.  **Trigger**: `PublishAssessmentVersionCommand` in `AssessmentCreation`.
2.  **Intermediate**: Module B publishes `AssessmentPublishedEvent`.
3.  **Observation**: `AssessmentDelivery` consumer subscribes and creates its own "Ready to deliver" read-model.
4.  **Verify**: Querying the `DeliveryAPI` now returns the new assessment ID as available for session creation.

### Scenario 3: Bulk Import Ingestion Flow
1.  **Trigger**: `StartImportCommand` with a DOCX file ID.
2.  **Intermediate**: `ImportExport` publishes `ProcessFileCommand`.
3.  **Observation**: `FileProcessing` module extracts text and publishes `FileExtractionCompletedEvent`.
4.  **Intermediate**: `ImportExport` maps text to domain blocks and publishes `ImportExecutionCompletedEvent`.
5.  **Verify**: The target domain (e.g., Lesson Plan) now has the new entity persisted in its database schema.

## 4. Error Chains
- **Scenario**: Simulate an extraction failure in `FileProcessing`.
- **Verify**: `ImportExport` receives the "Failed" event and updates the `ImportJob` status to `Failed` with a reason.
