# API Reference

## Overview

Lex provides a comprehensive REST API with OpenAPI/Swagger documentation. All API routes follow RESTful conventions and enforce authentication/authorization at the endpoint level.

**Base URL:** `/api/` (relative to application origin)
**API Documentation:** `https://your-lex-instance/swagger/` or `https://your-lex-instance/swagger/v1/swagger.json`

## Authentication

### Bearer Token Authentication

All API requests must include a valid JWT token in the `Authorization` header:

```bash
Authorization: Bearer <jwt_token>
```

### Getting a Token

Tokens are issued by Keycloak during user login. The frontend handles token management automatically via the `tokenManager` service.

**Manual Token Requests (for testing):**
```bash
curl -X POST https://keycloak.your-lex-instance/realms/lex/protocol/openid-connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "client_id=lex-web" \
  -d "username=user@example.com" \
  -d "password=password" \
  -d "grant_type=password"
```

### Token Refresh

Tokens expire after 5 minutes. Use the refresh token to obtain a new access token:

```bash
curl -X POST https://keycloak.your-lex-instance/realms/lex/protocol/openid-connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "client_id=lex-web" \
  -d "refresh_token=<refresh_token>" \
  -d "grant_type=refresh_token"
```

## Error Handling

### Problem Details Response Format

All error responses follow the [RFC 7807 Problem Details](https://tools.ietf.org/html/rfc7807) specification:

```json
{
  "type": "https://lex.edu/problems/validation-error",
  "title": "One or more validation errors occurred",
  "status": 400,
  "detail": "The following fields have errors",
  "errors": {
    "Title": ["Title is required"],
    "SubjectId": ["Subject must be a valid GUID"]
  },
  "traceId": "0HN7H9QFHGVKL:00000001"
}
```

### Common Status Codes

| Code | Meaning | Scenario |
|------|---------|----------|
| 200 | OK | Successful GET/POST/PUT/PATCH |
| 201 | Created | Resource created successfully (POST) |
| 202 | Accepted | Long-running job started (async pattern) |
| 204 | No Content | Successful DELETE or empty response |
| 400 | Bad Request | Validation error or malformed request |
| 401 | Unauthorized | Missing or invalid token |
| 403 | Forbidden | User lacks required permissions |
| 404 | Not Found | Resource doesn't exist |
| 409 | Conflict | Business rule violation (e.g., duplicate entry) |
| 500 | Server Error | Unexpected error |

### Result<T> Wrapper

Successful API responses wrap the actual data in a Result envelope:

```json
{
  "isSuccess": true,
  "value": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "title": "Assessment 101",
    "status": "Draft"
  },
  "error": null
}
```

**Error Response:**
```json
{
  "isSuccess": false,
  "value": null,
  "error": {
    "code": "AssessmentNotFound",
    "message": "No assessment with ID 550e8400-e29b-41d4-a716-446655440000 exists"
  }
}
```

## API Modules

### Scheduling Module

**Base Path:** `/api/schedule`

| Method | Endpoint | Description | Permission |
|--------|----------|-------------|-----------|
| GET | `/` | Get user's schedule | scheduling.view |
| GET | `/class/{classId}` | Get class schedule | scheduling.view |
| GET | `/teacher/{teacherId}` | Get teacher schedule | scheduling.view |
| GET | `/periods/{date}` | Get periods for date | scheduling.view |
| POST | `/academic-years` | Create academic year | scheduling.manage |
| GET | `/academic-years` | List academic years | scheduling.view |
| POST | `/terms` | Create term | scheduling.manage |
| POST | `/periods` | Create period/slot | scheduling.manage |
| PATCH | `/periods/{periodId}` | Update period | scheduling.manage |
| DELETE | `/periods/{periodId}` | Delete period | scheduling.manage |

**Example Request:**
```bash
curl -H "Authorization: Bearer $TOKEN" \
  https://lex.local/api/schedule/class/1?startDate=2024-04-01&endDate=2024-04-30
```

**Example Response:**
```json
{
  "isSuccess": true,
  "value": [
    {
      "slotId": 1,
      "assignmentId": 1,
      "classId": 1,
      "teacherId": "teacher-123",
      "classroomId": 5,
      "subject": "Mathematics",
      "dayOfWeek": 1,
      "startTime": "09:00",
      "endTime": "10:00"
    }
  ]
}
```

---

### Lessons Module

**Base Path:** `/api/lessons`

| Method | Endpoint | Description | Permission |
|--------|----------|-------------|-----------|
| GET | `/` | List lessons | lessons.view |
| GET | `/{lessonId}` | Get lesson details | lessons.view |
| POST | `/` | Create lesson | lessons.create |
| PATCH | `/{lessonId}` | Update lesson | lessons.edit |
| DELETE | `/{lessonId}` | Delete lesson | lessons.delete |
| POST | `/{lessonId}/publish` | Publish lesson | lessons.publish |
| POST | `/{lessonId}/resources` | Upload resource | lessons.resources.manage |
| GET | `/{lessonId}/resources` | List resources | lessons.view |
| DELETE | `/{lessonId}/resources/{resourceId}` | Delete resource | lessons.resources.manage |

**Example Request (Create Lesson):**
```bash
curl -X POST -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Introduction to Algebra",
    "subjectId": "subject-123",
    "classId": "class-456",
    "periodId": 10,
    "content": "<p>Rich text content...</p>",
    "status": "Draft"
  }' \
  https://lex.local/api/lessons
```

---

### Assessment Creation Module

**Base Path:** `/api/assessments`

| Method | Endpoint | Description | Permission |
|--------|----------|-------------|-----------|
| GET | `/` | List assessments | assessment_creation.view |
| GET | `/{assessmentId}` | Get assessment | assessment_creation.view |
| POST | `/` | Create assessment | assessment_creation.create |
| PATCH | `/{assessmentId}` | Update assessment | assessment_creation.edit |
| DELETE | `/{assessmentId}` | Delete assessment | assessment_creation.delete |
| POST | `/{assessmentId}/publish` | Publish & create snapshot | assessment_creation.publish |
| GET | `/{assessmentId}/questions` | Get questions | assessment_creation.view |
| POST | `/{assessmentId}/questions` | Add question | assessment_creation.edit |
| PATCH | `/{assessmentId}/questions/{questionId}` | Update question | assessment_creation.edit |
| DELETE | `/{assessmentId}/questions/{questionId}` | Delete question | assessment_creation.edit |
| PATCH | `/{assessmentId}/questions/reorder` | Reorder questions | assessment_creation.edit |
| GET | `/question-bank` | List questions in bank | assessment_creation.question_bank |
| POST | `/question-bank` | Add to question bank | assessment_creation.question_bank |

**Example Request (Create Assessment):**
```bash
curl -X POST -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Q1 Mathematics Exam",
    "subjectId": "subject-123",
    "classId": "class-456",
    "totalPoints": 100,
    "status": "Draft",
    "instructions": "Complete all questions..."
  }' \
  https://lex.local/api/assessments
```

---

### Assessment Delivery Module

**Base Path:** `/api/delivery`

| Method | Endpoint | Description | Permission |
|--------|----------|-------------|-----------|
| POST | `/sessions` | Start assessment session | assessment_delivery.submit |
| GET | `/sessions/{sessionId}` | Get session status | assessment_delivery.submit |
| POST | `/sessions/{sessionId}/answers` | Submit answer | assessment_delivery.submit |
| POST | `/sessions/{sessionId}/finish` | Submit assessment | assessment_delivery.submit |
| GET | `/results/{sessionId}` | Get results | assessment_delivery.view_results |
| GET | `/submissions/{assessmentId}` | List submissions (teacher) | assessment_delivery.grade |
| POST | `/submissions/{submissionId}/grade` | Grade submission | assessment_delivery.grade |

**Example Request (Start Session):**
```bash
curl -X POST -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "snapshotId": "snapshot-123"
  }' \
  https://lex.local/api/delivery/sessions
```

**Response (202 Accepted - Long Running):**
```json
{
  "isSuccess": true,
  "value": {
    "sessionId": "session-456",
    "status": "Active"
  }
}
```

---

### Diary Management Module

**Base Path:** `/api/diary`

| Method | Endpoint | Description | Permission |
|--------|----------|-------------|-----------|
| GET | `/` | List entries | diary.view |
| GET | `/{entryId}` | Get entry | diary.view |
| POST | `/` | Create entry | diary.create |
| PATCH | `/{entryId}` | Update entry | diary.edit |
| DELETE | `/{entryId}` | Delete entry | diary.delete |
| POST | `/{entryId}/submit` | Submit for approval | diary.submit |
| POST | `/{entryId}/approve` | Approve entry (admin) | diary.approve |
| POST | `/{entryId}/attachments` | Upload attachment | diary.attachments |
| DELETE | `/{entryId}/attachments/{attachmentId}` | Delete attachment | diary.attachments |

---

### Notifications Module

**Base Path:** `/api/notifications`

| Method | Endpoint | Description | Permission |
|--------|----------|-------------|-----------|
| GET | `/` | Get user's notifications | notifications.view |
| POST | `/{notificationId}/read` | Mark as read | notifications.read |
| POST | `/read-all` | Mark all as read | notifications.read |
| DELETE | `/{notificationId}` | Delete notification | notifications.view |
| GET | `/count` | Get unread count | notifications.view |
| GET | `/preferences` | Get preferences | notifications.view |
| PATCH | `/preferences` | Update preferences | notifications.preferences.manage |

---

### File Storage Module

**Base Path:** `/api/files`

| Method | Endpoint | Description | Permission |
|--------|----------|-------------|-----------|
| POST | `/upload` | Upload file | files.upload |
| GET | `/{fileId}` | Download file | (resource owner or admin) |
| DELETE | `/{fileId}` | Delete file | files.delete |
| GET | `/{fileId}/info` | Get file metadata | (resource owner or admin) |
| POST | `/batch-upload` | Upload multiple files | files.upload |

**Example Request (Upload File):**
```bash
curl -X POST -H "Authorization: Bearer $TOKEN" \
  -F "file=@document.pdf" \
  https://lex.local/api/files/upload
```

---

### Reporting Module

**Base Path:** `/api/reports`

| Method | Endpoint | Description | Permission |
|--------|----------|-------------|-----------|
| GET | `/` | List available reports | reporting.view |
| GET | `/{reportId}` | Get report data | reporting.view |
| GET | `/dashboards` | Get dashboard definitions | reporting.view |
| POST | `/{reportId}/export` | Export report | reporting.export |
| GET | `/analytics/class/{classId}` | Class performance | reporting.view |
| GET | `/analytics/student/{studentId}` | Student progress | reporting.view |

---

## Admin Endpoints

**Base Path:** `/api/admin`

| Method | Endpoint | Description | Permission |
|--------|----------|-------------|-----------|
| GET | `/users` | List users | users.manage |
| POST | `/users` | Create user | users.manage |
| GET | `/users/{userId}` | Get user details | users.manage |
| PATCH | `/users/{userId}` | Update user | users.manage |
| DELETE | `/users/{userId}` | Delete user | users.manage |
| POST | `/users/{userId}/reset-password` | Reset password | users.manage |
| POST | `/users/bulk-import` | Bulk import users | users.manage |
| GET | `/classes` | List classes | classes.manage |
| POST | `/classes` | Create class | classes.manage |
| PATCH | `/classes/{classId}` | Update class | classes.manage |
| DELETE | `/classes/{classId}` | Delete class | classes.manage |
| GET | `/audit-logs` | View audit logs | admin |
| GET | `/settings` | Get system settings | admin |
| PATCH | `/settings` | Update settings | admin |
| GET | `/health` | System health check | (public) |

---

## Async Job Pattern (202 Accepted)

Some operations return a `202 Accepted` response with a location to check status:

```bash
# Request
curl -X POST -H "Authorization: Bearer $TOKEN" \
  -F "file=@large-file.xlsx" \
  https://lex.local/api/import/assessments

# Response (202 Accepted)
HTTP/1.1 202 Accepted
Location: /api/jobs/job-123/status
Content-Type: application/json

{
  "isSuccess": true,
  "value": {
    "jobId": "job-123",
    "status": "Processing"
  }
}

# Check Status
curl -H "Authorization: Bearer $TOKEN" \
  https://lex.local/api/jobs/job-123/status

# Response
{
  "isSuccess": true,
  "value": {
    "jobId": "job-123",
    "status": "Processing",
    "percentComplete": 45,
    "message": "Processing item 45 of 100..."
  }
}

# When complete (status: "Completed")
{
  "isSuccess": true,
  "value": {
    "jobId": "job-123",
    "status": "Completed",
    "result": {
      "totalProcessed": 100,
      "successCount": 98,
      "failureCount": 2,
      "errors": [...]
    }
  }
}
```

## WebSocket Integration (SignalR)

Real-time features use WebSocket via SignalR:

**Hub URL:** `/hubs/notifications`

**Available Methods:**
- `OnNotificationReceived` — New notification arrived
- `OnJobStatusUpdated` — Async job progress update
- `OnAssessmentSessionUpdated` — Assessment session state change
- `OnPresenceUpdated` — User online/offline status

**Example (JavaScript):**
```javascript
import * as SignalR from "@microsoft/signalr";

const connection = new SignalR.HubConnectionBuilder()
  .withUrl("/hubs/notifications", {
    accessTokenFactory: () => tokenManager.accessToken
  })
  .build();

connection.on("OnNotificationReceived", (notification) => {
  console.log("New notification:", notification);
});

await connection.start();
```

---

## Rate Limiting

API endpoints are rate-limited to prevent abuse:

**Default Limits:**
- Anonymous users: 20 requests per minute
- Authenticated users: 100 requests per minute  
- Admin users: 1000 requests per minute

**Rate Limit Headers:**
```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 87
X-RateLimit-Reset: 1609462800
```

When limit exceeded:
```json
{
  "type": "https://lex.edu/problems/rate-limit-exceeded",
  "status": 429,
  "title": "Too Many Requests",
  "detail": "Rate limit exceeded. Try again after 60 seconds."
}
```

---

## API Pagination

List endpoints support pagination:

**Query Parameters:**
- `page` — Page number (1-based, default: 1)
- `pageSize` — Items per page (default: 20, max: 100)
- `sortBy` — Sort field
- `sortDirection` — "asc" or "desc"

**Example:**
```bash
curl -H "Authorization: Bearer $TOKEN" \
  "https://lex.local/api/assessments?page=2&pageSize=50&sortBy=createdAt&sortDirection=desc"
```

**Response:**
```json
{
  "isSuccess": true,
  "value": {
    "items": [...],
    "pageNumber": 2,
    "pageSize": 50,
    "totalCount": 250,
    "totalPages": 5,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}
```

---

## Swagger/OpenAPI Documentation

Interactive API documentation is available at:

- **Dev:** http://localhost:5000/swagger/
- **Production:** https://lex.your-institution.edu/swagger/

**JSON Schema:** `{url}/swagger/v1/swagger.json`

Use Swagger UI to:
- Browse all endpoints
- View request/response schemas
- Try out API calls (with authentication)
- Generate client libraries

---

## Client SDK

A TypeScript/JavaScript client can be auto-generated from the OpenAPI spec:

```bash
# Generate client
npx openapi-generator-cli generate -i https://lex.local/swagger/v1/swagger.json -g typescript-fetch -o ./src/generated

# Use generated client
import { AssessmentsApi, Configuration } from "./generated";

const config = new Configuration({
  accessToken: tokenManager.accessToken
});
const api = new AssessmentsApi(config);
const assessments = await api.getAssessments();
```

---

## Deprecation & Versioning

API versions are indicated in the URL:
- v1 (current): `/api/v1/*`
- Legacy routes: `/api/*` (maintained for 6 months during migration)

Deprecated endpoints return:
```json
{
  "isSuccess": false,
  "error": {
    "code": "EndpointDeprecated",
    "message": "This endpoint is deprecated. Use /api/v1/assessments instead."
  }
}
```

---

## Support

For API issues:
- Check [Swagger documentation](https://lex.local/swagger/)
- Review [authentication guide](./AUTHENTICATION.md)
- Submit issue on [GitHub](https://github.com/your-org/lex/issues)
- Email: api-support@lex.edu
