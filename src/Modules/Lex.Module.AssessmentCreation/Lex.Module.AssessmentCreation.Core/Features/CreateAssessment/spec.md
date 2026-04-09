# Feature Specification: Create Assessment

## 1. Feature Overview
- **Parent Module**: AssessmentCreation
- **Description**: Create a new draft assessment.
- **Type**: Command
- **User Story**: As a Teacher, I want to create a new assessment so that I can evaluate my students' learning progress.

## 2. Request Representation
- **Request Class**: `CreateAssessmentCommand`
- **Payload Structure**:
    - `Title`: String
    - `SubjectId`: GUID
    - `TemplateId`: GUID (Optional)
- **Validation Rules**:
    - `Title` must not be empty.
    - `SubjectId` must not be empty.

## 3. Business Logic (The Slice)
- **Trigger**: REST `POST /api/assessments`
- **Domain Logic**:
    - A new `Assessment` aggregate is created in `Draft` status.
    - If a `TemplateId` is provided, sections and questions are cloned from the template.
- **Side Effects**: None.

## 4. Persistence
- **Affected Tables**: `assessment_creation.assessments`
- **Repository Methods**: `AddAsync(Assessment assessment)`

## 5. Domain Objects (High-Level)
- **Assessment**: Aggregate root initialized with status and metadata.

## 6. API / UI Contracts
- **Route**: `POST /api/assessments`
- **Response**: `Result<Guid>` (The Assessment ID)
- **UI Interaction**: Triggered by the "Create Exam" button in the library.

## 7. Security
- **Required Permission**: `AssessmentCreationPermissions.Manage`
- **Auth Policy**: `TeacherOnly`
