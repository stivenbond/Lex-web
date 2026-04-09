# API Specification: Scheduling Module

## 1. Overview
- **Parent Module**: Scheduling
- **Base Route**: `/api/scheduling`
- **Purpose**: Expose scheduling management and lookup capabilities to the frontend and external consumers.

## 2. API Endpoints

### Academic Year & Structure
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/academic-years` | `CreateAcademicYear` | Define a new academic year (e.g., 2026-2027). |
| `POST` | `/terms` | `CreateTerm` | Add a term (Semester/Trimester) to an academic year. |
| `POST` | `/periods` | `CreatePeriod` | Define a time block (e.g., "Period 1: 08:30-09:20"). |

### Timetable Management
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/slots/assign` | `AssignClassToSlot` | Assign a Class and Teacher to a specific Period/Date/Classroom. |
| `PATCH` | `/slots/{id}` | `UpdateSlot` | Modify an existing slot allocation. |
| `DELETE` | `/slots/{id}` | `RemoveSlot` | Cancel/Remove a slot allocation. |

### Lookups & Queries
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/schedule/class/{classId}` | `GetScheduleForClass` | Returns the weekly timetable grid for a class. |
| `GET` | `/schedule/teacher/{teacherId}` | `GetScheduleForTeacher` | Returns the weekly timetable grid for a teacher. |
| `GET` | `/periods/date/{date}` | `GetPeriodsForDate` | List all defined periods for a given day. |

## 3. Request/Response Shapes

### Error Handling
All endpoints return `Result<T>`. On failure, a `ProblemDetails` (RFC 7807) response is returned:
```json
{
  "type": "https://lex.edu/errors/validation",
  "title": "Validation Error",
  "status": 400,
  "detail": "Academic year must start before it ends.",
  "errors": { "EndDate": ["Must be after StartDate"] }
}
```

### Example: AssignClassToSlot
- **Request**:
```json
{
  "periodId": "guid",
  "classId": "guid",
  "teacherId": "guid",
  "classroomId": "guid",
  "date": "2026-04-10"
}
```
- **Response**: `Result<Guid>` (The created Slot ID).

## 4. Security & Authorization
- Endpoints use the standard `[Authorize(Policy = SchedulingPermissions.View)]` or `SchedulingPermissions.Manage`.
- `ICurrentUser` is used for teacher-specific lookups to ensure they can only see their own schedule unless they have `AppAdmin` or `Scheduling.ReadAll` role.

## 5. Integration with YARP
The Host API (`Lex.API`) routes traffic to this module via YARP:
- Match: `/api/scheduling/{**catch-all}`
- Destination: `http://localhost:5000` (or the microservice URL after extraction).
