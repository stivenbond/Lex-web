# Feature Specification: Get Question Bank

## 1. Feature Overview
- **Parent Module**: AssessmentCreation
- **Description**: Search and retrieve questions from the shared question bank for reuse in assessments.
- **Type**: Query
- **User Story**: As a Teacher, I want to search the question bank so that I can reuse high-quality questions I've previously authored.

## 2. Request Representation
- **Request Class**: `GetQuestionBankQuery`
- **Payload Structure**:
    - `SearchTerm`: String
    - `SubjectId`: GUID
    - `Type`: QuestionType (Optional)
    - `Difficulty`: Integer (Optional)
- **Validation Rules**:
    - `SubjectId` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `GET /api/assessments/bank`
- **Domain Logic**:
    - Query the `assessment_creation.question_bank` table with filters.
    - Return a paged list of reusable question candidates.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `assessment_creation.question_bank`
- **Repository Methods**: `SearchBankAsync`

## 5. Domain Objects (High-Level)
- **BankQuestionDto**: Metadata and snippet of the question content.

## 6. API / UI Contracts
- **Route**: `GET /api/assessments/bank?subject=...&search=...`
- **Response**: `Result<PagedList<BankQuestionDto>>`
- **UI Interaction**: Displayed in the "Question Bank" sidebar/drawer during assessment authoring.

## 7. Security
- **Required Permission**: `AssessmentCreationPermissions.View`
- **Auth Policy**: `TeacherOnly`
