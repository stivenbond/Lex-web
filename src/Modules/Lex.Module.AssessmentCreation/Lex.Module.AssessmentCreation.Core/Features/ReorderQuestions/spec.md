# Feature Specification: Reorder Assessment Content

## 1. Feature Overview
- **Parent Module**: AssessmentCreation
- **Description**: Update the logical order of sections and questions within an assessment.
- **Type**: Command
- **User Story**: As a Teacher, I want to reorder my questions so that the exam flows logically from easy to difficult concepts.

## 2. Request Representation
- **Request Class**: `ReorderAssessmentContentCommand`
- **Payload Structure**:
    - `AssessmentId`: GUID
    - `Sections`: List of `{ SectionId: GUID, Order: int, QuestionOrders: List<{ QuestionId: GUID, Order: int }> }`
- **Validation Rules**:
    - `AssessmentId` must not be empty.
    - All IDs must exist within the assessment.

## 3. Business Logic (The Slice)
- **Trigger**: REST `PATCH /api/assessments/{id}/reorder`
- **Domain Logic**:
    - Load `Assessment`.
    - Ensure status is `Draft`.
    - Update the `Order` property on all affected `Section` and `Question` entities.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `assessment_creation.sections`, `assessment_creation.questions`
- **Repository Methods**: `GetByIdAsync`, `UpdateAsync`

## 5. Domain Objects (High-Level)
- **Assessment**: Updated with new structural ordering.

## 6. API / UI Contracts
- **Route**: `PATCH /api/assessments/{id}/reorder`
- **Response**: `Result<Success>`
- **UI Interaction**: Triggered by drag-and-drop end events in the React Flow / Custom List builder.

## 7. Security
- **Required Permission**: `AssessmentCreationPermissions.Manage`
- **Auth Policy**: `TeacherOnly`
