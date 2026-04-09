# Feature Specification: Get Session for Student

## 1. Feature Overview
- **Parent Module**: AssessmentDelivery
- **Description**: Retrieve the current state of an active assessment attempt for a student, allowing them to resume work.
- **Type**: Query
- **User Story**: As a Student, I want to see my current progress in an exam so that I can continue answering from where I left off.

## 2. Request Representation
- **Request Class**: `GetSessionForStudentQuery`
- **Payload Structure**:
    - `AssessmentId`: GUID
- **Validation Rules**:
    - `AssessmentId` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `GET /api/delivery/sessions/active/{assessmentId}`
- **Domain Logic**:
    - Fetch the latest `Active` session for the current authenticated student and the given `AssessmentId`.
    - Join with the `AssessmentSnapshot` logic to provide the full list of questions and their structure.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `assessment_delivery.delivery_sessions`, `assessment_delivery.submission_answers`
- **Repository Methods**: `GetActiveSessionWithAnswersAsync`

## 5. Domain Objects (High-Level)
- **SessionProgressDto**: Contains the assessment structure (from snapshot) and the student's current answers.

## 6. API / UI Contracts
- **Route**: `GET /api/delivery/sessions/active/{assessmentId}`
- **Response**: `Result<SessionProgressDto>`
- **UI Interaction**: Triggered when the student opens the "Secure Player" for a specific assessment.

## 7. Security
- **Required Permission**: `AssessmentDeliveryPermissions.Take`
- **Auth Policy**: `StudentOnly` (implicitly scoped to current user)
