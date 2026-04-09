# Feature Specification: Get Session Results

## 1. Feature Overview
- **Parent Module**: AssessmentDelivery
- **Description**: Retrieve the scored results of a completed assessment attempt.
- **Type**: Query
- **User Story**: As a Student, I want to see my results after my exam is graded so that I can understand my performance and errors.

## 2. Request Representation
- **Request Class**: `GetSessionResultsQuery`
- **Payload Structure**:
    - `SessionId`: GUID
- **Validation Rules**:
    - `SessionId` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `GET /api/delivery/sessions/{id}/results`
- **Domain Logic**:
    - Load `DeliverySession`.
    - Verification: Ensure status is `Submitted` and potentially `FullyGraded` (depends if partial results are allowed).
    - Map each `SubmissionAnswer` to its score and feedback provided by the teacher (for manual grading).
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `assessment_delivery.delivery_sessions`, `assessment_delivery.submission_answers`
- **Repository Methods**: `GetResultsBySessionIdAsync`

## 5. Domain Objects (High-Level)
- **SessionResultsDto**: Includes total score, question-by-question breakdown, and correct answers for review.

## 6. API / UI Contracts
- **Route**: `GET /api/delivery/sessions/{id}/results`
- **Response**: `Result<SessionResultsDto>`
- **UI Interaction**: Displayed in the "Results" page after a student submits their exam or when they review old attempts.

## 7. Security
- **Required Permission**: `AssessmentDeliveryPermissions.Take` (Student view) or `AssessmentDeliveryPermissions.Grade` (Teacher review view).
- **Auth Policy**: `GeneralAccess` (scoped via user ID or permission).
