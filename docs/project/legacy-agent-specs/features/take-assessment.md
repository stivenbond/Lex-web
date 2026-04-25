# Feature Specification: Student Assessment Session

## 1. Feature Overview
- **Parent Module**: AssessmentDelivery
- **Description**: The student UI and backend for taking a test.
- **Type**: Command (Start & Record Answers)
- **User Story**: As a Student, I want to take my assigned assessment and have my answers saved as I go.

## 2. Request Representation
- **Command 1**: `StartSessionCommand` (Payload: `AssessmentId`)
- **Command 2**: `SaveAnswerCommand` (Payload: `SessionId`, `QuestionId`, `ResponseData`)

## 3. Business Logic (The Slice)
- **Start Workflow**:
    - Trigger: `POST /api/delivery/sessions`
    - Logic: Checks for existing sessions. Starts the countdown.
- **Save Workflow**:
    - Trigger: `PATCH /api/delivery/sessions/{id}/answers`
    - Logic: Idempotent update of the answer matching `QuestionId`. Verifies the session is still `InProgress`.

## 4. Persistence
- **Affected Tables**: `assessment_delivery.sessions`, `assessment_delivery.answers`
- **Repository Methods**: `IDeliveryRepository.UpdateAnswerAsync()`

## 5. Domain Objects (High-Level)
- **DeliverySession**: State and timer management.
- **Answer**: The value object representing the student's input.

## 6. API / UI Contracts
- **Routes**:
    - `POST /api/delivery/sessions`
    - `PATCH /api/delivery/sessions/{id}/answers`
- **Response**: `Result<T>`

## 7. Security
- **Required Permission**: `DeliveryPermissions.Take`
- **Auth Policy**: `RequireStudent` (Must be assigned the test)
