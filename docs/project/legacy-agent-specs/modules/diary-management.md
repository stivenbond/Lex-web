# Module Specification: DiaryManagement

## 1. Overview
- **Name**: DiaryManagement
- **Purpose**: Managing daily class diaries, subject coverage, and pedagogical record-keeping.
- **Schema**: `diary.*`
- **Primary Stakeholders**: Teachers (update diaries), Admins (approve diaries).

## 2. Domain Boundaries
- **Aggregate Root: DiaryEntry**: Represents a teacher's record for a specific subject and class on a specific date/period.
- **Value Object: BlockContent**: The structured body of the diary entry (text, images, diagrams).

## 3. Module Responsibilities
- Creation and editing of daily diaries.
- Capturing what was taught (subject coverage).
- Workflow for submission and approval of diaries.
- Handling attachments via ObjectStorage reference.

## 4. Integration & Dependencies
- **Inbound Events**: 
    - `SlotAssignedEvent` (Scheduling): Automatically creates a draft DiaryEntry stub.
- **Outbound Events**:
    - `DiaryEntrySubmittedEvent`
    - `DiaryEntryApprovedEvent`
- **External Dependencies**: Uses `IObjectStorageService` for attachments.

## 5. Security & Authorization
- **Permission Constants**: `DiaryPermissions.cs`
- **Policies**: `RequireDiaryOwner`, `RequireDiaryApprover`.

## 6. Cross-Module Interactions
- **Scheduling**: Links to `SlotId` (FK relationship only, no direct DB joins).
- **Notifications**: Subscribes to `DiaryEntryApprovedEvent` to notify the teacher.
