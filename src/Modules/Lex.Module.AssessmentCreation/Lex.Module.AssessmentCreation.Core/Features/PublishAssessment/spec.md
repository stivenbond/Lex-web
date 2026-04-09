# Feature Specification: Publish Assessment

## 1. Feature Overview
- **Parent Module**: AssessmentCreation
- **Description**: Finalize an assessment draft and generate an immutable snapshot for delivery.
- **Type**: Command
- **User Story**: As a Teacher, I want to publish my assessment so that it can be assigned to students for completion.

## 2. Request Representation
- **Request Class**: `PublishAssessmentCommand`
- **Payload Structure**:
    - `Id`: GUID
- **Validation Rules**:
    - `Id` must not be empty.
    - Assessment must have at least one question.

## 3. Business Logic (The Slice)
- **Trigger**: REST `POST /api/assessments/{id}/publish`
- **Domain Logic**:
    - Load `Assessment`.
    - Verification: Ensure status is `Draft` and business rules are met.
    - Status change: Update status to `Published`.
    - Snapshot Generation: Transform the relational structure into the `AssessmentSnapshot` JSON format (as defined in the snapshot contract).
- **Side Effects**:
    - Publish `AssessmentPublishedEvent` (containing the snapshot JSON) to the message bus.

## 4. Persistence
- **Affected Tables**: `assessment_creation.assessments`, `assessment_creation.snapshots`
- **Repository Methods**: `GetByIdAsync`, `UpdateAsync`, `AddSnapshotAsync`

## 5. Domain Objects (High-Level)
- **AssessmentPublishedEvent**: Carries the `AssessmentID`, `Version`, and the Snapshot data.

## 6. API / UI Contracts
- **Route**: `POST /api/assessments/{id}/publish`
- **Response**: `Result<Success>`
- **UI Interaction**: Triggered by the "Publish" button in the builder. Shows a final preview and a confirmation dialog.

## 7. Security
- **Required Permission**: `AssessmentCreationPermissions.Manage`
- **Auth Policy**: `TeacherOnly`
