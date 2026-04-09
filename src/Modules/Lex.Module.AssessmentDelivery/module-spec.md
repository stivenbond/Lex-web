# Module Specification: AssessmentDelivery

## 1. Overview
- **Name**: AssessmentDelivery
- **Purpose**: Manage the secure runtime environment for students taking assessments, track their progress, and handle the grading workflow.
- **Schema**: `assessment_delivery.*`
- **Primary Stakeholders**: Students (Test-takers), Teachers (Evaluators/Proctors).

## 2. Domain Boundaries
List the main Aggregate Roots and Entities managed by this module.

- **Aggregate Root: DeliverySession**: Tracks a single student's attempt at an assessment.
- **Entity: Submission**: The collection of answers provided during a session.
- **Entity: Answer**: A specific response to a question from the assessment snapshot.
- **Value Object: AssessmentSnapshot**: A copy of the published assessment structure pulled from the `AssessmentCreation` module.

## 3. Module Responsibilities
- Consume and store `AssessmentSnapshots` to ensure students take the exact version they were assigned.
- Orchestrate the "Student Player" session (starting, pausing/resuming, and submitting).
- Implement an auto-grading engine for objective question types (MCQ, Short Answer).
- Provide a workflow for manual grading of subjective question types (Essay).

## 4. Integration & Dependencies
- **Inbound Events**: 
    - `AssessmentPublishedEvent`: Triggered when an exam is finalized. This module serializes and stores the snapshot.
- **Outbound Events**: 
    - `AssessmentAttemptStartedEvent`
    - `AssessmentSubmittedEvent`: Published once the student finishes, triggering the grading flow.
- **External Dependencies**: None.

## 5. Security & Authorization
- **Permission Constants**: `AssessmentDeliveryPermissions.cs`
- **Policies**: 
    - `CanTakeAssessment`: Granted to students assigned to a specific delivery session.
    - `CanGradeAssessment`: Granted to teachers to review and grade submissions.

## 6. Cross-Module Interactions
- **AssessmentCreation**: Provides the raw material (snapshots) for delivery.
- **Scheduling**: Determines when an assessment session becomes "Open" based on the timetable (future phase).
- **Notifications**: Notifies students when an assessment is ready or graded.
