# Specification: Assessment Grading Workflow

## 1. Overview
- **Category**: Business Logic / Workflow
- **Parent Module**: AssessmentDelivery
- **Component Name**: Grading Engine
- **Description**: Define the logic for scoring student submissions based on the assessment snapshot.

## 2. Technical Strategy
- **Trigger**: `AssessmentAttemptSubmitted` event or command.
- **Engine Logic**: A background job or handler that iterates through each `SubmissionAnswer`.

## 3. Implementation Details

### Auto-Grading Logic (Objective Questions)
| Question Type | Logic |
| :--- | :--- |
| **MCQ** | If `Answer.SelectedOptionId` matches `Snapshot.CorrectOptionId`, full points. Otherwise 0 (or negative if defined). |
| **Short Answer** | If `Answer.Text` matches `Snapshot.ExpectedValue` (or regex), full points. |

### Manual Grading Logic (Subjective Questions)
| Question Type | Status Change | Workflow |
| :--- | :--- | :--- |
| **Essay** | `AwaitingGrading` | Teacher opens the submission in the Proctor Dashboard, reviews the text, and enters a score (0 to MaxPoints). |
| **FileUpload** | `AwaitingGrading` | Teacher downloads the file from ObjectStorage, reviews, and enters score. |

### Finalizing Results
- **Partial Score**: A submission summary shows the current score (Auto-graded only).
- **Final Score**: The submission is marked as `FullyGraded` only after all subjective questions have been scored by a teacher.

## 4. Configuration & Settings
- **GradingService**: Encapsulates the polymorphic grading strategy for each question type.

## 5. Resilience & Performance
- **Consistency**: The engine must use the `AssessmentSnapshot` stored at the start of the session, even if the assessment is deleted in the Creation module.
- **Concurrency**: Multiple submissions can be graded in parallel using MassTransit consumers.

## 6. Integration Points
- **Outbound Events**: `AssessmentGradingCompleted` published once the final score is calculated.

## 7. Migration & Deployment
- **Verification**: Unit tests covering all question type variants with edge cases (empty answers, partial MCQ matches).
