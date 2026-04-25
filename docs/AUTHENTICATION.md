# Authentication & Authorization Guide

## Overview

Lex uses **OpenID Connect (OIDC) with Keycloak** for authentication and **role-based access control (RBAC)** with permissions for authorization. This document defines:

- How users authenticate
- Role definitions and responsibilities
- Permission matrix for each role
- What each role can and cannot do across the platform
- Implementation details for developers

## Authentication Flow

### User Login

1. User navigates to `https://lex.your-institution.edu`
2. Next.js application checks for valid JWT token in session storage
3. If no valid token exists, redirects to Keycloak login page
4. User enters credentials (username/password)
5. Keycloak validates and returns JWT containing:
   - `sub` (user ID)
   - `email` 
   - `name`
   - `roles` (array of role names)
   - `permissions` (array of specific permissions)
   - `tenant_id` (institution ID, for multi-tenancy)
6. Token stored in secure HTTP-only cookie (expires: 5 minutes)
7. Refresh token stored in secure HTTP-only cookie (expires: 30 days)
8. User redirected to dashboard

### Token Refresh

- Access tokens expire after 5 minutes
- Frontend automatically refreshes using refresh token
- If refresh token expired, user must log in again
- Logout clears both tokens

### Security Features

- **PKCE** — Authorization code flow with proof key for code exchange (prevents authorization code interception)
- **Same-Site Cookies** — Prevents CSRF attacks
- **HTTP-Only Cookies** — Prevents JavaScript-based token theft
- **HTTPS Only** — All communication encrypted in transit
- **Token Rotation** — Refresh tokens rotated on each refresh

## Role Definitions

### 1. App Admin (`AppAdmin`)

**Purpose:** System-wide administration and configuration

**Responsibilities:**
- Manage all users and assign roles
- Configure system-wide settings and branding
- Perform system maintenance and updates
- View all reports and analytics
- Manage integrations (Google, SMTP, etc.)
- Configure backup and disaster recovery
- Access audit logs
- Manage extensions and custom fields

**Permissions:**
- Access: Full system access, bypass all permission checks
- Special Policy: `AdminOnly`
- Cannot: Cannot be locked out (has bypass mechanism)

**Typical Users:**
- IT Director
- System Administrator
- Head of ICT Department

---

### 2. Institution Admin (`InstitutionAdmin`)

**Purpose:** Institution-wide management (in multi-tenant setups)

**Responsibilities:**
- Manage users within the institution
- Create and manage classes, sections, subjects
- Assign teachers to classes
- View institution-wide reports
- Configure institution branding
- Manage bulk import/export operations
- Configure SMTP and notification settings per institution
- Review audit logs for the institution

**Permissions:**
- `institutionadmin.users.manage` — Create, edit, delete users
- `institutionadmin.classes.manage` — Manage class structure
- `institutionadmin.reporting.view` — View institutional reports
- `institutionadmin.settings.configure` — Configure institution settings
- `institutionadmin.import_export.manage` — Manage bulk operations
- Access: Institution-level data only (cannot access other institutions)
- Policy: `InstitutionAdminOnly`

**Typical Users:**
- School Principal
- Vice Principal (Academic)
- Registrar
- Examination Officer

---

### 3. Teacher (`Teacher`)

**Purpose:** Create and deliver curriculum, assess student learning

**Responsibilities:**
- Create and manage lesson plans and resources
- Create, publish, and deliver assessments
- Enter and submit lesson diary entries
- Grade student submissions
- View student progress
- Manage own content and submissions
- Upload and attach resources

**Permissions:**

**Scheduling:**
- `scheduling.view` — View academic calendar and timetables
- `scheduling.query` — Query own schedule

**Lessons:**
- `lessons.create` — Create lesson plans
- `lessons.edit` — Edit own lesson plans
- `lessons.delete` — Delete own lesson plans
- `lessons.publish` — Publish lesson plans
- `lessons.view` — View all lesson plans in own classes
- `lessons.resources.manage` — Upload and attach resources

**Assessments (Creation):**
- `assessment_creation.create` — Create assessments
- `assessment_creation.edit` — Edit own draft assessments
- `assessment_creation.delete` — Delete own unpublished assessments
- `assessment_creation.publish` — Publish assessments
- `assessment_creation.view` — View all assessments in own classes
- `assessment_creation.question_bank.manage` — Create and manage question bank

**Assessments (Delivery):**
- `assessment_delivery.view` — View assessment results and submissions
- `assessment_delivery.grade` — Grade subjective questions
- `assessment_delivery.proctoring` — Proctor assessment sessions

**Diary:**
- `diary.create` — Create diary entries
- `diary.edit` — Edit own diary entries
- `diary.delete` — Delete own diary entries
- `diary.submit` — Submit diary for approval
- `diary.view` — View own diary entries
- `diary.attachments` — Attach files to diary

**Notifications:**
- `notifications.view` — View notifications
- `notifications.read` — Mark notifications as read
- `notifications.preferences.manage` — Configure notification settings

**Access Policy:** `TeacherOnly`

**Cannot:**
- Create or manage users
- View other teachers' content
- Configure system settings
- View or delete other teachers' assessments
- Edit published assessments
- Impersonate students

**Typical Users:**
- Teachers
- Lecturers
- Instructors
- Subject Coordinators (with additional department-level permissions)

---

### 4. Student (`Student`)

**Purpose:** Participate in learning and submit assessments

**Responsibilities:**
- View course schedule and lesson materials
- Participate in class activities
- Submit assessments
- View grades and feedback
- Track academic progress

**Permissions:**

**Scheduling:**
- `scheduling.view` — View own timetable
- `scheduling.query` — Query own schedule periods

**Lessons:**
- `lessons.view` — View lesson materials and resources
- `lessons.resources.download` — Download lesson attachments

**Assessments (Delivery):**
- `assessment_delivery.submit` — Take and submit assessments
- `assessment_delivery.view_results` — View own results and grades
- `assessment_delivery.view_feedback` — View feedback on submissions

**Diary:**
- `diary.view` — View lesson diary entries (read-only)

**Notifications:**
- `notifications.view` — View notifications
- `notifications.read` — Mark notifications as read

**Profile:**
- `profile.view` — View own profile
- `profile.edit` — Edit own profile information
- `profile.password.change` — Change own password

**Access Policy:** `StudentOnly`

**Cannot:**
- Create or edit any content
- View other students' submissions or grades
- Access administrative features
- Upload or delete files
- Modify assessments or lessons
- See other students' personal information
- Change own role

**Typical Users:**
- Students
- Pupils
- Learners

---

## Permission Matrix

### Complete Permission Reference

#### Core Permissions by Module

| Permission | Admin | Inst Admin | Teacher | Student |
|-----------|:-----:|:----------:|:-------:|:-------:|
| **Scheduling** |
| scheduling.view | ✅ | ✅ | ✅ | ✅ |
| scheduling.manage | ✅ | ✅ | ❌ | ❌ |
| **Lessons** |
| lessons.create | ✅ | ✅ | ✅ | ❌ |
| lessons.edit | ✅ | ✅ | ✅* | ❌ |
| lessons.delete | ✅ | ✅ | ✅* | ❌ |
| lessons.publish | ✅ | ✅ | ✅ | ❌ |
| lessons.view | ✅ | ✅ | ✅ | ✅ |
| **Assessment Creation** |
| assessment_creation.create | ✅ | ✅ | ✅ | ❌ |
| assessment_creation.edit | ✅ | ✅ | ✅* | ❌ |
| assessment_creation.publish | ✅ | ✅ | ✅ | ❌ |
| assessment_creation.view | ✅ | ✅ | ✅ | ❌ |
| assessment_creation.question_bank | ✅ | ✅ | ✅ | ❌ |
| **Assessment Delivery** |
| assessment_delivery.submit | ✅ | ❌ | ❌ | ✅ |
| assessment_delivery.grade | ✅ | ✅ | ✅ | ❌ |
| assessment_delivery.view | ✅ | ✅ | ✅ | ✅* |
| **Diary** |
| diary.create | ✅ | ✅ | ✅ | ❌ |
| diary.edit | ✅ | ✅ | ✅* | ❌ |
| diary.approve | ✅ | ✅ | ✅ | ❌ |
| diary.view | ✅ | ✅ | ✅ | ✅ |
| **Notifications** |
| notifications.manage | ✅ | ✅ | ❌ | ❌ |
| notifications.view | ✅ | ✅ | ✅ | ✅ |
| **Reporting** |
| reporting.view | ✅ | ✅ | ✅* | ❌ |
| reporting.export | ✅ | ✅ | ✅* | ❌ |
| **File Management** |
| files.upload | ✅ | ✅ | ✅ | ❌ |
| files.delete | ✅ | ✅ | ✅* | ❌ |
| **User Management** |
| users.manage | ✅ | ✅ | ❌ | ❌ |
| users.view | ✅ | ✅ | ❌ | ❌ |

**Legend:**
- ✅ = Full permission
- ✅* = Limited (own content or within scope only)
- ❌ = No permission

### Resource-Level Access Control

In addition to role-based permissions, resource-level access is enforced:

**Teachers can only:**
- View lessons/assessments they created
- View/grade submissions from their classes
- Edit their own diary entries
- Delete their own unpublished content

**Students can only:**
- View lessons from their enrolled classes
- Submit assessments assigned to them
- View their own grades and feedback
- Not see other students' work

**Institution Admins can:**
- View all resources within their institution
- Not view resources from other institutions (multi-tenant)

## Implementation Details

### JWT Claim Structure

```json
{
  "sub": "550e8400-e29b-41d4-a716-446655440000",
  "email": "john.doe@school.edu",
  "name": "John Doe",
  "roles": ["Teacher", "InstitutionAdmin"],
  "permissions": [
    "lessons.create",
    "lessons.edit",
    "assessment_creation.create",
    "assessment_delivery.grade",
    "diary.create"
  ],
  "tenant_id": "institution-123",
  "iat": 1609459200,
  "exp": 1609462800,
  "iss": "https://keycloak.lex.local/realms/lex"
}
```

### Backend Authorization

**Attribute-Based:**
```csharp
[Authorize(Policy = "TeacherOnly")]
[RequirePermission("lessons.create")]
public IActionResult CreateLesson(CreateLessonCommand command)
{
    // Only users with Teacher role AND lessons.create permission can access
}
```

**Dynamic:**
```csharp
if (!await authorizationService.AuthorizeAsync(User, lessonId, "OwnerOrAdmin"))
{
    return Forbid();
}
```

### Frontend Authorization

**Route Guards:**
```typescript
// Only Teachers can access
<Route path="/assessments/create" component={CreateAssessment} policy="TeacherOnly" />

// Only Students can access
<Route path="/assessments/take/:id" component={TakeAssessment} policy="StudentOnly" />
```

**Permission Checks:**
```typescript
const { hasPermission } = useAuth();

if (hasPermission("assessment_creation.publish")) {
  // Show publish button
}
```

**Component Hiding:**
```typescript
<ProtectedComponent policy="TeacherOnly" fallback={<UnauthorizedView />}>
  <AssessmentBuilder />
</ProtectedComponent>
```

## Multi-Tenancy

Lex supports multi-tenant deployments where:
- Each institution is a separate tenant
- Data is isolated at the database schema level
- Institution Admins manage users within their institution
- App Admins manage across all institutions
- Users are assigned a `tenant_id` claim in their JWT

## Audit & Compliance

### Audit Logging

Every action is logged with:
- User ID and role
- Action performed
- Timestamp
- IP address
- Result (success/failure)
- Affected resources

**Examples:**
- User "john.doe@school.edu" (Teacher) created Assessment "Mathematics Final Exam" (2024-04-01 14:30)
- User "admin@school.edu" (InstitutionAdmin) assigned 25 students to Class 10-A (2024-04-01 15:45)
- Failed login attempt from IP 192.168.1.100 (2024-04-01 16:00)

### Permission Change Audit Trail

When permissions or roles are modified:
- Change is logged with who made it
- Previous and new values recorded
- Effective immediately for existing tokens (requires logout/login for new token)
- Notification sent to affected user

## Security Best Practices

### For Administrators

1. **Strong Passwords**
   - Minimum 12 characters
   - Mix of uppercase, lowercase, numbers, special characters
   - Unique to Lex (not reused across systems)

2. **Regular Password Rotation**
   - Change every 90 days
   - Enforced for admin accounts

3. **Principle of Least Privilege**
   - Assign minimum permissions needed for role
   - Use Teacher role for teachers, not Admin
   - Audit role assignments quarterly

4. **Session Management**
   - Access tokens: 5 minute lifetime
   - Refresh tokens: 30 day lifetime
   - Sessions expire after 30 days of inactivity
   - Force logout on suspicious activity

### For Users

1. **Never Share Credentials**
   - Each user gets unique login
   - Passwords never stored in plaintext
   - No password sharing for role assumption

2. **Use Institution-Provided Device**
   - Avoid public/shared computers for sensitive work
   - Use VPN for remote access

3. **Report Security Issues**
   - Suspect account activity: Contact IT immediately
   - Discover vulnerability: Report to security@lex.edu

## Troubleshooting

### User Cannot Login
- Verify user is assigned to correct institution
- Check if user account is disabled
- Ensure user has a valid role assigned
- Check password complexity requirements

### Permission Denied Error
- Verify user has required role
- Check permission was assigned to role
- Confirm user's institution/tenant match resource
- Review audit logs for permission changes

### Token Expiration Issues
- Clear browser cookies and refresh
- Verify system clock is synchronized
- Check refresh token not expired (30 days)
- Try logging out and back in

## Additional Resources

- [API Documentation](./API.md) — API endpoints and authorization headers
- [Self-Hosting Guide](./SELF-HOSTING.md) — Keycloak configuration
- [Architecture Guide](./architecture.md) — System design
- [Keycloak Administration](https://www.keycloak.org/documentation) — Official Keycloak docs
