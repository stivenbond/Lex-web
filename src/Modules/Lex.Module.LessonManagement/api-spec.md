# API Specification: LessonManagement Module

## 1. Overview
- **Parent Module**: LessonManagement
- **Base Route**: `/api/lessons`
- **Purpose**: Manage the lifecycle of lesson plans, including creation, publication, and resource management.

## 2. API Endpoints

### Lesson Plan Management
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/` | `CreateLessonPlan` | Create a new lesson plan draft. |
| `PUT` | `/{id}` | `UpdateLessonPlan` | Update draft content and title. |
| `GET` | `/{id}` | `GetLessonPlanById` | Fetch full lesson plan details. |
| `DELETE` | `/{id}` | `DeleteLessonPlan` | Remove a draft lesson plan. |

### Lifecycle & Resources
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/{id}/publish` | `PublishLessonPlan` | Finalize a plan and make it immutable. |
| `POST` | `/{id}/resources` | `AttachResource` | Link a file asset to the lesson plan. |
| `DELETE` | `/{id}/resources/{resId}` | `RemoveResource` | Detach a resource from the plan. |

### Feeds & Library
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/subject/{subjectId}` | `GetLessonPlansForSubject` | List all plans for a subject (paged). |
| `GET` | `/period/{periodId}` | `GetLessonPlansForPeriod` | Fetch the plan assigned to a specific period. |
| `GET` | `/latest` | - | Returns recently modified lesson plans. |

## 3. Request/Response Shapes

### Resource: LessonPlanFullDto
```json
{
  "id": "guid",
  "title": "string",
  "subjectId": "guid",
  "status": "Draft | Published | Rejected",
  "body": { "blocks": [...] },
  "resources": [
    { "id": "guid", "fileId": "guid", "displayName": "string" }
  ],
  "publishedAt": "datetime | null"
}
```

## 4. Security & Authorization
- **Policies**:
    - `TeacherOnly` for all management and library endpoints.
- **Rules**:
    - Users can only edit plans they created (unless they have `Admin` permissions).
    - Published plans are read-only for all users (versioning is used for edits).

## 5. Integration with YARP
- Match: `/api/lessons/{**catch-all}`
- Destination: `http://localhost:5000` (In-process host).
