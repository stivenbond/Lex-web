# Feature Specification: Update Question

## 1. Feature Overview
- **Parent Module**: AssessmentCreation
- **Description**: Update the content, options, and scoring rules of a specific question.
- **Type**: Command
- **User Story**: As a Teacher, I want to edit my questions so that I can provide clear instructions and accurate scoring criteria.

## 2. Request Representation
- **Request Class**: `UpdateQuestionCommand`
- **Payload Structure**:
    - `Id`: GUID
    - `Content`: String (Rich text)
    - `Points`: Integer
    - `Specifics`: JSON (Type-specific data like MCQ options or expected answer)
- **Validation Rules**:
    - `Id` must not be empty.
    - `Points` must be >= 0.

## 3. Business Logic (The Slice)
- **Trigger**: REST `PUT /api/assessments/{id}/questions/{qId}`
- **Domain Logic**:
    - Load `Assessment`.
    - Ensure status is `Draft`.
    - Update the target `Question` entity with new content and logic.
    - Recalculate total points for the assessment if necessary.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `assessment_creation.questions`, `assessment_creation.question_options`
- **Repository Methods**: `GetByIdAsync`, `UpdateAsync`

## 5. Domain Objects (High-Level)
- **Question**: Updated with new content and type-specific rules.

## 6. API / UI Contracts
- **Route**: `PUT /api/assessments/{id}/questions/{qId}`
- **Response**: `Result<Success>`
- **UI Interaction**: Triggered by the "Save" or auto-save event in the Question Editor.

## 7. Security
- **Required Permission**: `AssessmentCreationPermissions.Manage`
- **Auth Policy**: `TeacherOnly`
