# Module Specification: DiaryManagement

## 1. Overview
- **Name**: DiaryManagement
- **Purpose**: Manage daily records of educational activities, subject delivery, and observations for specific classes and periods.
- **Schema**: `diary.*`
- **Primary Stakeholders**: Teachers (maintain diary), Admins (approve diary entries).

## 2. Domain Boundaries
List the main Aggregate Roots and Entities managed by this module.

- **Aggregate Root: DiaryEntry**: The primary record for a class-period session.
- **Value Object: PeriodReference**: A reference to a `Slot` or `Period` ID in the Scheduling module.
- **Value Object: SubjectReference**: A reference to the subject being taught.

## 3. Module Responsibilities
- Allow teachers to record what happened during a scheduled slot.
- Support rich content via `BlockContent`.
- Manage the workflow from creation to submission and final approval.
- Handle file attachments associated with diary entries.

## 4. Integration & Dependencies
- **Inbound Events**: 
    - `SlotAssignedEvent` (Scheduling): Prepare a diary entry draft (optional).
- **Outbound Events**: 
    - `DiaryEntrySubmittedEvent`
    - `DiaryEntryApprovedEvent`
- **External Dependencies**: 
    - `ObjectStorage` (via `IObjectStorageService` for attachments).

## 5. Security & Authorization
- **Permission Constants**: `DiaryPermissions.cs`
- **Policies**: 
    - `CanManageDiary`: Standard teacher access.
    - `CanApproveDiary`: Required for head-of-department or admin.

## 6. Cross-Module Interactions
- **Scheduling**: `DiaryEntry` holds an ID reference to a `Slot` in Scheduling. Direct joins are forbidden.
- **ObjectStorage**: All attachments are managed via the `ObjectStorage` module.
- **Notifications**: Approvals may trigger notifications to the teacher.
