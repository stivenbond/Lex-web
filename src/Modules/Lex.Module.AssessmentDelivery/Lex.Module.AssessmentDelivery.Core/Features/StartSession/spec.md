# Feature Specification: Start Assessment Session

## 1. Feature Overview
- **Parent Module**: AssessmentDelivery
- **Description**: Initialize a student's attempt at an assessment.
- **Type**: Command
- **User Story**: As a Student, I want to start my exam so that I can begin answering questions.

## 2. Request Representation
- **Request Class**: `StartSessionCommand`
- **Payload Structure**:
    - `AssessmentId`: GUID
- **Validation Rules**:
    - `AssessmentId` must not be empty.
    - Student must be allowed to take this assessment (e.g. assigned).

## 3. Business Logic (The Slice)
- **Trigger**: REST `POST /api/delivery/sessions`
- **Domain Logic**:
    - Fetch the latest `AssessmentSnapshot` for the given ID.
    - Check for an existing `Active` session for this student. If one exists, resume it.
    - Create a new `DeliverySession` in `Active` status.
- **Side Effects**:
    - Publish `AssessmentAttemptStartedEvent`.

## 4. Persistence
- **Affected Tables**: `assessment_delivery.delivery_sessions`
- **Repository Methods**: `GetActiveSessionAsync`, `AddAsync`

## 5. Domain Objects (High-Level)
- **DeliverySession**: Aggregate root tracking the student's progress.

## 6. API / UI Contracts
- **Route**: `POST /api/delivery/sessions`
- **Response**: `Result<Guid>` (The Session ID)
- **UI Interaction**: Triggered by the "Start Test" button in the student's dashboard.

## 7. Security
- **Required Permission**: `AssessmentDeliveryPermissions.Take`
- **Auth Policy**: `StudentOnly`
