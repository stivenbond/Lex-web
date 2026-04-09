# Feature Specification: Get Assessment Questions

## 1. Feature Overview
- **Parent Module**: AssessmentCreation
- **Description**: Retrieve a flat or grouped list of questions within an assessment.
- **Type**: Query
- **User Story**: As a Teacher, I want to see all questions in my exam so that I can audit the total points and question variety.

## 2. Request Representation
- **Request Class**: `GetAssessmentQuestionsQuery`
- **Payload Structure**:
    - `AssessmentId`: GUID
- **Validation Rules**:
    - `AssessmentId` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `GET /api/assessments/{id}/questions`
- **Domain Logic**:
    - Fetch all questions associated with the assessment sections.
    - Return them joined with their type-specific metadata.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `assessment_creation.questions`, `assessment_creation.question_options`
- **Repository Methods**: `GetQuestionsByAssessmentIdAsync`

## 5. Domain Objects (High-Level)
- **QuestionSummaryDto**: Contains content, type, and points.

## 6. API / UI Contracts
- **Route**: `GET /api/assessments/{id}/questions`
- **Response**: `Result<List<QuestionSummaryDto>>`
- **UI Interaction**: Displayed in the "Questions" tab of the assessment builder.

## 7. Security
- **Required Permission**: `AssessmentCreationPermissions.View`
- **Auth Policy**: `TeacherOnly`
