# Module Specification: AssessmentDelivery

## 1. Overview
- **Name**: AssessmentDelivery
- **Purpose**: Execution, monitoring, and submission of published assessments.
- **Schema**: `assessment_delivery.*`
- **Primary Stakeholders**: Students (taking tests), Teachers (monitoring/grading).

## 2. Domain Boundaries
- **Aggregate Root: DeliverySession**: A student's specific attempt at an assessment.
- **Entity: Submission**: The finalized set of answers for a session.
- **Value Object: Snapshot**: The immutable test definition ingested from `AssessmentCreation`.

## 3. Module Responsibilities
- Rendering assessments for students.
- Recording answers in real-time.
- Enforcing time limits and auto-submission.
- Grading (auto-grading for MCQ/Short, manual for others).

## 4. Integration & Dependencies
- **Inbound Events**:
    - `AssessmentPublishedEvent`: Triggers local ingestion of the assessment snapshot.
- **Outbound Events**:
    - `AssessmentSubmittedEvent`
    - `AssessmentGradedEvent`
- **External Dependencies**: SignalR for real-time timer sync.

## 5. Security & Authorization
- **Permission Constants**: `DeliveryPermissions.cs`
- **Policies**: `RequireStudentAccess`, `RequireGrader`.

## 6. Cross-Module Interactions
- **AssessmentCreation**: Provides the content via events.
- **Reporting**: Receives results via events for long-term tracking.
