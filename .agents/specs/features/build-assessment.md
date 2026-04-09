# Feature Specification: Build Assessment Layout

## 1. Feature Overview
- **Parent Module**: AssessmentCreation
- **Description**: Managing questions and sections within an assessment.
- **Type**: Command
- **User Story**: As a Teacher, I want to add and order questions in my assessment so I can design a comprehensive test.

## 2. Request Representation
- **Command**: `UpdateAssessmentLayoutCommand`
- **Payload Structure**:
    - `AssessmentId` (Guid)
    - `Sections` (List of Section objects: Title, Questions)
- **Validation Rules**:
    - Cannot update if the assessment is already Published.

## 3. Business Logic (The Slice)
- **Trigger**: REST PATCH `/api/assessments/{id}/layout`
- **Logic**: 
    - Loads the `Assessment` aggregate.
    - Re-maps the section/question hierarchy.
    - Updates total points.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `assessment_creation.assessments`, `assessment_creation.questions`
- **Repository Methods**: `IAssessmentRepository.UpdateLayoutAsync()`

## 5. Domain Objects (High-Level)
- **Assessment**: Aggregate Root.
- **Question**: Entity list.

## 6. API / UI Contracts
- **Route**: `PATCH /api/assessments/{id}/layout`
- **Response**: `Result<Success>`

## 7. Security
- **Required Permission**: `AssessmentPermissions.Design`
- **Auth Policy**: `RequireAssessmentAuthor`
