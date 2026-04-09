# API Specification: DiaryManagement Module

## 1. Overview
- **Parent Module**: DiaryManagement
- **Base Route**: `/api/diary`
- **Purpose**: Manage teacher's diary entries, including content management, submission, and approval workflows.

## 2. API Endpoints

### Entry Management
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/` | `CreateDiaryEntry` | Create a new draft for a specific slot. |
| `PUT` | `/{id}` | `UpdateDiaryEntry` | Save content and attachments. |
| `GET` | `/{id}` | `GetDiaryEntryById` | Fetch full entry details. |
| `DELETE` | `/{id}` | `DeleteDiaryEntry` | Remove a draft entry. |

### Workflow
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/{id}/submit` | `SubmitDiaryEntry` | Submit for supervisor review. |
| `POST` | `/{id}/approve` | `ApproveDiaryEntry` | (Admin/Supervisor) Approve the entry. |
| `POST` | `/{id}/reject` | `RejectDiaryEntry` | (Admin/Supervisor) Reject with comments. |

### Feeds & Lists
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/class/{classId}` | `GetDiaryEntriesForClass` | Returns entry history for a class. |
| `GET` | `/date/{date}` | `GetDiaryEntriesForDate` | Returns all entries for a specific day. |
| `GET` | `/supervisor/pending` | `GetPendingApprovals` | List all entries awaiting review. |

## 3. Request/Response Shapes

### Resource: DiaryEntryDto
```json
{
  "id": "guid",
  "slotId": "guid",
  "subject": "string",
  "date": "2026-04-10T08:30:00Z",
  "status": "Draft | Submitted | Approved | Rejected",
  "body": { "blocks": [...] },
  "attachments": [
    { "fileId": "guid", "fileName": "string" }
  ]
}
```

## 4. Security & Authorization
- **Policies**: 
    - `TeacherOnly` for management endpoints.
    - `AdminOnly` for approval/rejection endpoints.
- **Ownership**: The module service will verify that the requesting user is the teacher assigned to the `SlotId` before allowing edits (unless Admin).

## 5. Integration with YARP
- Match: `/api/diary/{**catch-all}`
- Destination: `http://localhost:5000` (In-process host during development).
