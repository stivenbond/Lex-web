# Module Specification: AssessmentCreation

## 1. Overview
- **Name**: AssessmentCreation
- **Purpose**: Authoring and structure management for tests, exams, and quizzes.
- **Schema**: `assessment_creation.*`
- **Primary Stakeholders**: Teachers (creating tests), Admins (reviewing bank).

## 2. Domain Boundaries
- **Aggregate Root: Assessment**: A container for sections and questions.
- **Entity: Question**: Individual test items with specific types (MCQ, Short, etc.).
- **Aggregate Root: QuestionBank**: A reusable library of questions categorized by subject.

## 3. Module Responsibilities
- Designing the structure of assessments (timed vs untimed, scoring rules).
- Authoring multiple question types.
- Generating snapshots for the Delivery module.

## 4. Integration & Dependencies
- **Inbound Events**: None.
- **Outbound Events**:
    - `AssessmentPublishedEvent`: Contains the full JSON snapshot of the assessment for Delivery.
- **External Dependencies**: Uses ObjectStorage for media in questions.

## 5. Security & Authorization
- **Permission Constants**: `AssessmentPermissions.cs`
- **Policies**: `RequireAssessmentAuthor`.

## 6. Cross-Module Interactions
- **AssessmentDelivery**: Subscribes to `AssessmentPublishedEvent` to "ingest" the test into its local delivery schema.
- **Reporting**: Subscribes to results (from Delivery) but uses creation metadata for labels.
