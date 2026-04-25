# Domain Specification: Assessment Design

## 1. Context
- **Module**: AssessmentCreation
- **Scope**: Structural design of academic evaluation tasks.

## 2. Core Ubiquitous Language
- **Assessment**: The top-level entity representing a test.
- **Section**: A logical grouping of questions within an assessment.
- **QuestionVariant**: The specific behavior of a question (MCQ, Logic, Essay).
- **Snapshot**: A read-only, versioned representation of the assessment at the time of publishing.

## 3. Domain Model Hierarchy

- **Aggregate Root: Assessment**
    - **Purpose**: Manages the life-cycle and layout of a test.
    - **Entities**: `AssessmentSection`, `Question`.
    - **Invariants**: 
        - Total points must equal the sum of question points.
        - Must have at least one question to be published.

- **Aggregate Root: QuestionBank**
    - **Purpose**: Library for reusable questions across assessments.

## 4. Question Types (Discriminators)
- **MCQ**: Options list + Correct answer index.
- **Short Answer**: Regex or string match for auto-grading.
- **Essay**: Long-form text (Manual grading).
- **FileUpload**: Requires student to upload a file (Manual grading).

## 5. Domain Events
- **AssessmentPublishedEvent**: Marks the creation of a snapshot.
- **AssessmentArchivedEvent**: Removes assessment from active use.

## 6. Business Operations (Conceptual)
- **Op: ReorderLayout**: Changes the sequence of sections and questions.
- **Op: Snapshot**: Deep clones the current structure into a immutable JSON format.
- **Op: Publish**: Final validation and trigger for snapshot event.
