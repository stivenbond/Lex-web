# Domain Specification: Assessment Delivery & Grading

## 1. Context
- **Module**: AssessmentDelivery
- **Scope**: Patiently managing student sessions, time constraints, and the grading workflow.

## 2. Core Ubiquitous Language
- **Session**: A single attempt by a student to complete an assessment.
- **Answer**: The student's response to a specific question (stored within a session).
- **Submission**: The completed session, locked for grading.
- **AutoGrade**: Logic that compares an Answer to the Snapshot's correct answer criteria.

## 3. Domain Model Hierarchy

- **Aggregate Root: DeliverySession**
    - **Purpose**: Tracks the progress of a student taking a test.
    - **State Machine**: `Initialized` -> `InProgress` -> `Submitted` -> `Graded`.
    - **Invariants**: 
        - Cannot answer questions once the session time has expired.
        - Only one active session per student per assessment allowed.

## 4. Grading Logic
- **Manual Grade**: A teacher provides a score and feedback for descriptive/file questions.
- **Auto Grade**: System calculates scores for MCQ and auto-match short answers based on the ingested snapshot.

## 5. Domain Events
- **AssessmentSubmittedEvent**: Triggered when a student clicks submit or the timer expires.
- **AssessmentGradedEvent**: Triggered when all questions have been scored (final result).

## 6. Business Operations (Conceptual)
- **Op: StartSession**: Initializes the timer and creates the aggregate.
- **Op: RecordAnswer**: Updates a specific question response (idempotent).
- **Op: ExpiryCheck**: (Async/Job) Automatically transitions a session to `Submitted` when time runs out.
- **Op: ProvideFeedback**: Updates the session with manual scores and comments.
