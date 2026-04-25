# Domain Specification: Diary Records

## 1. Context
- **Module**: DiaryManagement
- **Scope**: Recording historical teaching actions and subject progression.

## 2. Core Ubiquitous Language
- **DiaryEntry**: The central record for a specific teaching event.
- **SubjectCoverage**: A description of the topics/curriculum covered in that entry.
- **SubmissionState**: The workflow state (Draft, Submitted, Approved, Rejected).

## 3. Domain Model Hierarchy

- **Aggregate Root: DiaryEntry**
    - **Purpose**: Tracks what happened in a specific time slot.
    - **Invariants**:
        - Cannot be submitted if the body content is empty.
        - Cannot be edited once Approved.
        - Must reference a valid `SlotId` (from Scheduling).

## 4. Value Objects
- **BlockContent**: The rich-text structure (defined in SharedKernel).
- **AttachmentReference**: Pointer to a file in ObjectStorage (FileId, FileName).

## 5. Domain Events
- **DiaryEntrySubmittedEvent**: Triggered when a teacher finishes the record.
- **DiaryEntryApprovedEvent**: Triggered after admin review.

## 6. Business Operations (Conceptual)
- **Op: RecordLesson**: Updates the draft content.
- **Op: SubmitForReview**: Moves the entry to the Submitted state.
- **Op: Approve**: Finalizes the entry and makes it read-only.
