# API Specification: Reporting Module

## 1. Overview
- **Parent Module**: Reporting
- **Base Route**: `/api/reporting`
- **Purpose**: Expose query-optimized data for dashboards, summaries, and analytical charts.

## 2. API Endpoints

### Dashboards
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/dashboards/teacher` | `GetTeacherDashboard` | Summary of class performance for a teacher. |
| `GET` | `/dashboards/admin` | - | System-wide usage and health metrics. |

### Reports
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/students/{id}/progress` | `GetStudentProgress` | Individual student performance trends. |
| `GET` | `/classes/{id}/performance` | - | Detailed breakdown per class/subject. |

### Global Stats
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/stats/trends` | - | Time-series data for global or subject metrics. |

## 3. Request/Response Shapes

### Resource: TeacherDashboardDto
```json
{
  "classes": [
    {
      "classId": "guid",
      "className": "string",
      "averageGrade": 85.5,
      "pendingDiaries": 3,
      "lastCompletedLesson": "string"
    }
  ],
  "recentEvents": [ ... ]
}
```

### Resource: StudentProgressDto (Chart Optimized)
```json
{
  "summary": { "currentGrade": 82.0, "rank": "Top 10%" },
  "trends": [
    { "date": "2026-04-01", "grade": 78.0 },
    { "date": "2026-04-08", "grade": 82.0 }
  ]
}
```

## 4. Security & Authorization
- **Read-Only**: This API is strictly read-only. No `POST`/`PUT`/`PATCH` endpoints exist except for internal admin sync triggers.
- **Scoping**: All queries are implicitly scoped to the user's organizational context (e.g., a teacher only sees reports for their assigned classes).

## 5. Integration with YARP
- Match: `/api/reporting/{**catch-all}`
- Destination: `http://localhost:5000` (In-process host).
