# Feature Specification: Add Question to Assessment

## 1. Feature Overview
- **Parent Module**: AssessmentCreation
- **Description**: Add a new question to a specific section of an assessment.
- **Type**: Command
- **User Story**: As a Teacher, I want to add questions to my exam so that I can builder my assessment content.

## 2. Request Representation
- **Request Class**: `AddQuestionCommand`
- **Payload Structure**:
    - `AssessmentId`: GUID
    - `SectionId`: GUID
    - `Type`: QuestionType (MCQ, ShortAnswer, etc.)
    - `QuestionBankId`: GUID (Optional, if pulling from bank)
- **Validation Rules**:
    - `AssessmentId` must not be empty.
    - `Type` must be a supported question type.

## 3. Business Logic (The Slice)
- **Trigger**: REST `POST /api/assessments/{id}/questions`
- **Domain Logic**:
    - Load `Assessment`.
    - Verification: Ensure status is `Draft`.
    - If `QuestionBankId` is provided, clone content from the bank.
    - Otherwise, create a new `Question` entity with default values.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `assessment_creation.questions`
- **Repository Methods**: `GetByIdAsync`, `UpdateAsync`

## 5. Domain Objects (High-Level)
- **Question**: New entity added to the aggregate collection and persisted.

## 6. API / UI Contracts
- **Route**: `POST /api/assessments/{id}/sections/{sectionId}/questions`
- **Response**: `Result<Guid>` (The Question ID)
- **UI Interaction**: Triggered by the "Add Question" button in the assessment builder.

## 7. Security
- **Required Permission**: `AssessmentCreationPermissions.Manage`
- **Auth Policy**: `TeacherOnly`
