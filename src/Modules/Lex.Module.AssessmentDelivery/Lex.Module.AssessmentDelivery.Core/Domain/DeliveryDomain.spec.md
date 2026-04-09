# Domain Specification: Assessment Delivery & Attempts

## 1. Context
- **Module**: AssessmentDelivery
- **Scope**: Lifecycle of student attempts at assessments and the recording of their responses.

## 2. Core Ubiquitous Language
- **DeliverySession**: The lifecycle of a student's interaction with a specific assessment exam.
- **Submission**: The final set of answers committed by the student.
- **Answer**: The recorded response to an individual question.
- **Grading**: The calculation of points based on the answer compared to the snapshot's scoring rules.

## 3. Domain Model Hierarchy

- **Aggregate Root: DeliverySession**
    - **Purpose**: Tracks everything about a student's attempt.
    - **Invariants**:
        - Only one session can be `Active` for a given Student-Assessment pair at a time.
        - Questions available in the session must match the linked `AssessmentSnapshot`.
        - Once status is `Submitted`, no further answers can be recorded.
- **Entity: SubmissionAnswer**
    - **Purpose**: Store the student's response to a specific question ID.
    - **Status**: Can be `Graded` (automatic) or `AwaitingManualGrading` (essays).

## 4. Value Objects
- **AnswerValue**: Polymorphic container for student input (JSON).
- **SessionStatus**: Enum (`Active`, `Submitted`, `Cancelled`).

## 5. Domain Events
- **AssessmentAttemptStarted**: Triggers proctoring tools if enabled.
- **AssessmentAttemptSubmitted**: Triggers the auto-grading engine.
- **GradingCompleted**: Published once all questions (including manual) are scored.

## 6. Business Operations (Conceptual)
- **Op: Start Session**
    - **Logic**: Creates a session linked to the current `Published` version of the assessment.
- **Op: Save Progress**
    - **Logic**: Updates or inserts a `SubmissionAnswer`. Idempotent.
- **Op: Submit Final**
    - **Invariants Checked**: Session must be `Active`.
    - **Resulting State**: `Submitted`. Auto-grading logic is triggered.
