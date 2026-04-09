# API Specification: Notifications Module

## 1. Overview
- **Parent Module**: Notifications
- **Base Route**: `/api/notifications`
- **Purpose**: Manage user alerts, preferences, and real-time synchronization.

## 2. API Endpoints

### Inbox Management
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/` | `GetNotificationsForUser` | Fetch paged history. |
| `GET` | `/unread-count` | `GetUnreadCount` | Get summary of pending alerts. |
| `POST` | `/{id}/read` | `MarkRead` | Mark a specific alert as read. |
| `POST` | `/read-all` | `MarkRead` | Mark all alerts as read. |

### Settings
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/preferences` | - | Fetch user's notification settings. |
| `PUT` | `/preferences` | `UpdatePreferences` | Update channel settings. |

### Administration
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/templates` | - | List all notification templates. |
| `PUT` | `/templates/{id}` | - | Update a template's Liquid content. |

## 3. Request/Response Shapes

### Resource: NotificationDto
```json
{
  "id": "guid",
  "type": "Assessment_New",
  "title": "New Assessment Assigned",
  "message": "You have a new assessment in Math.",
  "createdAt": "iso-date",
  "readAt": "iso-date | null",
  "data": {
    "assessmentId": "guid",
    "subjectName": "Math"
  }
}
```

## 4. Security & Authorization
- **Ownership**: Every `/api/notifications/*` call is automatically scoped to the `UserId` from the JWT token.
- **Admin Rights**: Only users with `CanManageTemplates` can access the `/templates` endpoints.

## 5. Integration with YARP
- Match: `/api/notifications/{**catch-all}`
- Destination: `http://localhost:5000` (In-process host).
