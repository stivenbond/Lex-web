# Feature Specification: Get Assessment by ID

## 1. Feature Overview
- **Parent Module**: AssessmentCreation
- **Description**: Retrieve the full structure and metadata of an assessment draft or published version.
- **Type**: Query
- **User Story**: As a Teacher, I want to view my assessment structure so that I can see the current progress and layout.

## 2. Request Representation
- **Request Class**: `GetAssessmentByIdQuery`
- **Payload Structure**:
    - `Id`: GUID
- **Validation Rules**:
    - `Id` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `GET /api/assessments/{id}`
- **Domain Logic**:
    - Load `Assessment` aggregate.
    - If status is `Published`, use the most recent snapshot as the data source to ensure fidelity.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `assessment_creation.assessments`, `assessment_creation.sections`, `assessment_creation.questions`
- **Repository Methods**: `GetByIdAsync`

## 5. Domain Objects (High-Level)
- **AssessmentDetailsDto**: Nested structure including sections and questions.

## 6. API / UI Contracts
- **Route**: `GET /api/assessments/{id}`
- **Response**: `Result<AssessmentDetailsDto>`
- **UI Interaction**: Opens the assessment builder view for a specific exam.

## 7. Security
- **Required Permission**: `AssessmentCreationPermissions.View`
- **Auth Policy**: `TeacherOnly`
