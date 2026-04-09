# Domain Specification: Diary Management Core

## 1. Context
- **Module**: DiaryManagement
- **Scope**: Documentation and validation of lesson delivery and student progress records.

## 2. Core Ubiquitous Language
- **Diary Entry**: A single record associated with a Lesson/Period.
- **Content Body**: The rich-text/block-based description of the session.
- **Submission**: The act of freezing the entry for review.
- **Approval**: The final verification by a superior.

## 3. Domain Model Hierarchy

- **Aggregate Root: DiaryEntry**
    - **Purpose**: To provide a permanent record of what was taught and any observations made.
    - **Invariants**: 
        - Must reference a valid `SlotId` (from Scheduling).
        - Cannot be modified once in `Approved` state.
        - Must have a non-empty `SubjectId`.
    - **States**: `Draft`, `Submitted`, `Approved`, `Rejected`.

## 4. Value Objects
- **PeriodReference**: (SlotId).
- **SubjectReference**: (SubjectId).
- **BlockContent**: Enriched body (delegated to SharedKernel model).

## 5. Domain Events
- **DiaryEntrySubmitted**: Published on submission.
- **DiaryEntryApproved**: Published on approval.
- **AttachmentAdded**: Published when a file is linked to the entry.

## 6. Business Operations (Conceptual)
- **Op: Create Entry**
    - **Invariants Checked**: User must be the teacher assigned to the Slot.
- **Op: Submit Entry**
    - **Invariants Checked**: Body must not be empty.
- **Op: Approve Entry**
    - **Invariants Checked**: User must have `ApproveDiary` permission and not be the author.
