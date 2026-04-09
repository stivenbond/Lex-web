# Feature Specification: Save Answer

## 1. Feature Overview
- **Parent Module**: AssessmentDelivery
- **Description**: Save or update a student's answer to a specific question during an active session.
- **Type**: Command
- **User Story**: As a Student, I want my answers to be saved automatically as I work so that I don't lose progress if my connection is interrupted.

## 2. Request Representation
- **Request Class**: `SaveAnswerCommand`
- **Payload Structure**:
    - `SessionId`: GUID
    - `QuestionId`: GUID
    - `Value`: `AnswerValue` (e.g., MCQ OptionId or text)
- **Validation Rules**:
    - `SessionId` and `QuestionId` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `PUT /api/delivery/sessions/{id}/answers/{qId}`
- **Domain Logic**:
    - Load `DeliverySession`.
    - Verification: Ensure status is `Active`.
    - Create or update the `SubmissionAnswer` entity for the given `QuestionId`.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `assessment_delivery.submission_answers`
- **Repository Methods**: `GetByIdAsync`, `SaveAnswerAsync`

## 5. Domain Objects (High-Level)
- **SubmissionAnswer**: Entity capturing the student's response.

## 6. API / UI Contracts
- **Route**: `PUT /api/delivery/sessions/{id}/answers/{qId}`
- **Response**: `Result<Success>`
- **UI Interaction**: Triggered automatically (autosave) when a student selects an option or stops typing for a brief period.

## 7. Security
- **Required Permission**: `AssessmentDeliveryPermissions.Take`
- **Auth Policy**: `StudentOnly` (plus session ownership check)
