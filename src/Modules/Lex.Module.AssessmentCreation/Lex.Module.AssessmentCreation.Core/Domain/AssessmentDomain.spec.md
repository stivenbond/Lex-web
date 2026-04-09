# Domain Specification: Assessment Authoring

## 1. Context
- **Module**: AssessmentCreation
- **Scope**: Modeling of structural evaluation units and reusable question libraries.

## 2. Core Ubiquitous Language
- **Assessment**: A complete evaluation unit composed of structured sections.
- **QuestionBank**: A central library of individual questions that can be referenced by assessments.
- **Section**: A thematic grouping of questions within an assessment.
- **Question**: A single inquiry with specific scoring rules and content.
- **Snapshot**: An immutable, JSON-serialized version of a published assessment.

## 3. Domain Model Hierarchy

- **Aggregate Root: Assessment**
    - **Purpose**: Manage the structure and settings of a test/exam.
    - **Invariants**:
        - Must have at least one section before publishing.
        - Total points must be the sum of all question points.
        - Status must be `Draft` to allow structural changes.
- **Entity: Section**
    - **Purpose**: Organize questions into logical parts (e.g., "Grammar", "Reading").
- **Aggregate Root: QuestionBank**
    - **Purpose**: Manage a searchable repository of questions categorized by tags, subjects, and types.
- **Entity: Question** (Polymorphic)
    - **Purpose**: Represent various inquiry types.
    - **Types**:
        - **MCQ**: Options with one or many correct answers.
        - **ShortAnswer**: String-based comparison with optional regex support.
        - **Essay**: Rich-text response with a rubric for manual grading.
        - **FileUpload**: Requires student to upload an asset.

## 4. Value Objects
- **ScoringRules**: Defines points per question, negative marking, and grading type.
- **QuestionMetadata**: Tags, difficulty level, and subject reference.

## 5. Domain Events
- **AssessmentPublished**: Published when the teacher finalizes the design.
- **QuestionAddedToBank**: Published when a new question is authored or captured from an assessment.

## 6. Business Operations (Conceptual)
- **Op: Add Question to Assessment**
    - **Logic**: Can either create a new "local" question or link a "banked" question.
- **Op: Reorder Content**
    - **Logic**: Updates the `Order` property on sections and questions.
- **Op: Publish**
    - **Invariants Checked**: All MCQs must have at least one correct answer. All questions must have a points value > 0.
