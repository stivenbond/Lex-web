# Feature Specification: Submit Assessment Session

## 1. Feature Overview
- **Parent Module**: AssessmentDelivery
- **Description**: Finalize a student's attempt at an assessment and trigger the grading process.
- **Type**: Command
- **User Story**: As a Student, I want to submit my completed exam so that it can be graded and my scores can be recorded.

## 2. Request Representation
- **Request Class**: `SubmitSessionCommand`
- **Payload Structure**:
    - `SessionId`: GUID
- **Validation Rules**:
    - `SessionId` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `POST /api/delivery/sessions/{id}/submit`
- **Domain Logic**:
    - Load `DeliverySession`.
    - Verification: Ensure status is `Active`.
    - Change status to `Submitted`.
    - Set `SubmittedAt` timestamp.
- **Side Effects**:
    - Publish `AssessmentAttemptSubmittedEvent` (which triggers the Grading Engine documented in the grading workflow spec).

## 4. Persistence
- **Affected Tables**: `assessment_delivery.delivery_sessions`
- **Repository Methods**: `GetByIdAsync`, `UpdateAsync`

## 5. Domain Objects (High-Level)
- **AssessmentAttemptSubmittedEvent**: Contains `SessionId`, `StudentId`, and `AssessmentId`.

## 6. API / UI Contracts
- **Route**: `POST /api/delivery/sessions/{id}/submit`
- **Response**: `Result<Success>`
- **UI Interaction**: Triggered by a "Confirm Submission" modal when the student clicks the final "Finish" button.

## 7. Security
- **Required Permission**: `AssessmentDeliveryPermissions.Take`
- **Auth Policy**: `StudentOnly` (plus session ownership check)
