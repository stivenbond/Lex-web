# Frontend Routes & Navigation Specification

## Overview

This document defines all frontend routes, their purposes, access control policies, and the components that render them. The frontend uses Next.js 15 App Router with segment-based organization.

## Route Organization

Routes are organized into two main groups:

1. **Authentication Routes** (`(auth)`) — Public routes for login/logout
2. **Application Routes** (`(app)`) — Protected routes for authenticated users

## Authentication Routes `/(auth)`

### Public Routes (No Authentication Required)

#### `/login`
- **Purpose:** User login page
- **Component:** `components/auth/LoginPage.tsx`
- **Access:** Public (unauthenticated users only)
- **Redirect:** If already logged in → `/dashboard`
- **Features:**
  - Email/username input
  - Password input
  - "Remember me" checkbox
  - Password reset link
  - Error messaging
  - Loading state during login

#### `/logout`
- **Purpose:** Clear session and redirect to login
- **Component:** `components/auth/LogoutPage.tsx`
- **Access:** Public
- **Action:** Clears tokens, invalidates session, redirects to `/login`

#### `/password-reset`
- **Purpose:** Request password reset
- **Component:** `components/auth/PasswordResetPage.tsx`
- **Access:** Public
- **Features:**
  - Email input
  - Verification email send
  - Confirmation message
  - Resend link

#### `/password-reset/[token]`
- **Purpose:** Reset password with token
- **Component:** `components/auth/PasswordResetForm.tsx`
- **Access:** Public
- **Parameters:**
  - `token` — Reset token from email
- **Features:**
  - New password input
  - Confirm password input
  - Password strength indicator
  - Submit button
  - Token validation

#### `/unauthorized`
- **Purpose:** User does not have permission to access resource
- **Component:** `components/auth/UnauthorizedPage.tsx`
- **Access:** Public (anyone can view)
- **Message:** "You don't have permission to access this resource"

## Application Routes `/(app)`

All routes under `(app)` require authentication and appropriate role permissions.

### Dashboard & Core Navigation

#### `/dashboard`
- **Purpose:** Main application dashboard
- **Component:** `pages/dashboard/Dashboard.tsx`
- **Policy:** Authenticated users (all roles)
- **Role-Specific Views:**
  - **App Admin:** System health, user management summary, recent activity
  - **Institution Admin:** Institution stats, user activity, pending approvals
  - **Teacher:** Today's lessons, upcoming assessments, pending submissions
  - **Student:** My courses, upcoming assessments, recent grades
- **Features:**
  - Quick stats cards
  - Activity timeline
  - Quick action buttons
  - Role-specific widgets

#### `/profile`
- **Purpose:** User profile and settings
- **Component:** `pages/profile/ProfilePage.tsx`
- **Policy:** Authenticated users (all roles)
- **Features:**
  - View/edit personal information
  - Change password
  - Configure notification preferences
  - Download personal data (GDPR)
  - Delete account (self-service)
  - Session management
  - Activity log

---

### Scheduling Module

#### `/schedule`
- **Purpose:** View academic calendar and timetable
- **Component:** `pages/schedule/SchedulePage.tsx`
- **Policy:** `scheduling.view`
- **Role Access:** Teacher ✅, Student ✅, Admin ✅
- **Features:**
  - Calendar view (day/week/month)
  - Timetable grid
  - Filter by class/teacher
  - Filter by date range
  - Search periods
  - Export to Google Calendar
  - Class room location
  - Period timings
  - Subject information

#### `/schedule/[periodId]`
- **Purpose:** View period/slot details
- **Component:** `components/schedule/PeriodDetail.tsx`
- **Policy:** `scheduling.view`
- **Parameters:**
  - `periodId` — Period ID
- **Features:**
  - Period info (time, location, class, teacher)
  - Teacher information and contact
  - Classroom details
  - Related lessons
  - Assignments
  - Resources

#### `/schedule/manage` (Admin/Institution Admin only)
- **Purpose:** Manage academic calendar and timetables
- **Component:** `pages/schedule/ManagePage.tsx`
- **Policy:** `scheduling.manage`
- **Role Access:** App Admin ✅, Institution Admin ✅
- **Features:**
  - Create academic year
  - Create terms and periods
  - Assign classes to slots
  - Assign teachers
  - Assign classrooms
  - View conflicts
  - Bulk import from CSV

---

### Lessons Module

#### `/lessons`
- **Purpose:** View and browse lessons
- **Component:** `pages/lessons/LessonsPage.tsx`
- **Policy:** `lessons.view`
- **Role Access:** Teacher ✅, Student ✅, Admin ✅
- **Features:**
  - Search lessons by name, subject, teacher
  - Filter by class/subject/date
  - Lesson cards with preview
  - Favorite lessons
  - Sort by date created/updated/name
  - Pagination

#### `/lessons/[lessonId]`
- **Purpose:** View lesson details and materials
- **Component:** `pages/lessons/LessonDetail.tsx`
- **Policy:** `lessons.view`
- **Parameters:**
  - `lessonId` — Lesson ID
- **Features:**
  - Lesson title, subject, date
  - Rich content (Tiptap editor display)
  - Lesson resources (files, links)
  - Download resources
  - Teacher information
  - Related assessments
  - Comments (if enabled)

#### `/lessons/create`
- **Purpose:** Create new lesson plan
- **Component:** `pages/lessons/CreateLesson.tsx`
- **Policy:** `lessons.create`
- **Role Access:** Teacher ✅, Admin ✅
- **Features:**
  - Title input
  - Subject selection
  - Date/period selection
  - Class selection
  - Rich text editor (Tiptap)
  - Resource upload
  - Save as draft
  - Publish button
  - Preview
  - Template selection

#### `/lessons/[lessonId]/edit`
- **Purpose:** Edit existing lesson plan
- **Component:** `pages/lessons/EditLesson.tsx`
- **Policy:** `lessons.edit`
- **Parameters:**
  - `lessonId` — Lesson ID
- **Access:** Lesson author or admin
- **Features:**
  - Same as create
  - Load existing data
  - Update revision history
  - Track changes
  - Cannot edit published lessons (create new version)

#### `/lessons/templates`
- **Purpose:** Browse lesson templates
- **Component:** `pages/lessons/TemplatesPage.tsx`
- **Policy:** `lessons.view`
- **Role Access:** Teacher ✅, Admin ✅
- **Features:**
  - Template gallery
  - Filter by subject
  - Use template button
  - Preview
  - Create from template

---

### Assessment Creation Module

#### `/assessments`
- **Purpose:** Browse and manage assessments
- **Component:** `pages/assessments/AssessmentsPage.tsx`
- **Policy:** `assessment_creation.view` (students view assigned only)
- **Role Access:** Teacher ✅ (all), Student ✅ (assigned only), Admin ✅
- **Features:**
  - Search assessments
  - Filter by status (draft, published, archived)
  - Filter by subject, class, date
  - Assessment cards with key info
  - Sort options
  - Pagination

#### `/assessments/create`
- **Purpose:** Create new assessment
- **Component:** `pages/assessments/CreateAssessment.tsx`
- **Policy:** `assessment_creation.create`
- **Role Access:** Teacher ✅, Admin ✅
- **Features:**
  - Title and description
  - Subject selection
  - Class/section selection
  - Assessment settings:
    - Total points
    - Passing score
    - Show answers after submission
    - Randomize questions
    - Display order
  - Question builder
  - Save draft
  - Preview
  - Publish

#### `/assessments/[assessmentId]`
- **Purpose:** View assessment details
- **Component:** `pages/assessments/AssessmentDetail.tsx`
- **Policy:** `assessment_creation.view` (authors) or `assessment_delivery.view` (students)
- **Parameters:**
  - `assessmentId` — Assessment ID
- **Role-Specific Views:**
  - **Teacher/Admin:** Edit button, view submissions, view statistics
  - **Student:** Take assessment button (if assigned and available)

#### `/assessments/[assessmentId]/edit`
- **Purpose:** Edit assessment (draft only)
- **Component:** `pages/assessments/EditAssessment.tsx`
- **Policy:** `assessment_creation.edit`
- **Access:** Author or admin
- **Features:**
  - Cannot edit published assessments
  - Question management (add/remove/reorder)
  - Question editor
  - Section management
  - Settings update
  - Preview
  - Publish

#### `/assessments/[assessmentId]/questions`
- **Purpose:** Manage assessment questions
- **Component:** `pages/assessments/QuestionManager.tsx`
- **Policy:** `assessment_creation.edit`
- **Features:**
  - Question list
  - Reorder with drag-and-drop
  - Edit question inline or modal
  - Delete question
  - Add question
  - Add from question bank

#### `/assessments/[assessmentId]/submissions`
- **Purpose:** View student submissions
- **Component:** `pages/assessments/SubmissionsPage.tsx`
- **Policy:** `assessment_delivery.view`
- **Role Access:** Teacher (own class) ✅, Admin ✅
- **Features:**
  - Submissions list (student, date, score, status)
  - Filter by student, status
  - Sort by date, score
  - Click to grade
  - Bulk export
  - Statistics

#### `/question-bank`
- **Purpose:** Browse and manage question bank
- **Component:** `pages/assessments/QuestionBankPage.tsx`
- **Policy:** `assessment_creation.question_bank`
- **Role Access:** Teacher ✅, Admin ✅
- **Features:**
  - Question search
  - Filter by subject, type, difficulty
  - Create new question
  - Edit own questions
  - Delete own questions
  - Preview questions
  - Tags management
  - Export questions
  - Import questions (CSV)

---

### Assessment Delivery Module

#### `/take-assessment/[sessionId]`
- **Purpose:** Secure assessment player for students
- **Component:** `pages/assessments/TakeAssessment.tsx`
- **Policy:** `assessment_delivery.submit`
- **Role Access:** Student ✅ (assigned), Teacher (proctor only)
- **Parameters:**
  - `sessionId` — Delivery session ID
- **Security:**
  - Full-screen mode required (enforced)
  - Copy/paste disabled
  - Browser dev tools disabled
  - Prevent tab switching (warning)
  - Auto-save answers every 30 seconds
  - Timer display
  - Question navigation
- **Features:**
  - Question display
  - Answer input (varies by question type)
  - Progress bar
  - Time remaining
  - Submit button (disabled until all required answered or time expired)
  - Review and confirm before final submit
  - Warnings on unsaved changes

#### `/assessments/[assessmentId]/sessions`
- **Purpose:** Start new assessment session
- **Component:** `pages/assessments/StartSessionPage.tsx`
- **Policy:** `assessment_delivery.submit`
- **Role Access:** Student ✅ (if assigned)
- **Features:**
  - Instructions display
  - Assessment rules
  - Acknowledge terms button
  - System requirements check
  - Start button

#### `/assessments/[assessmentId]/results`
- **Purpose:** View assessment results
- **Component:** `pages/assessments/ResultsPage.tsx`
- **Policy:** `assessment_delivery.view_results`
- **Role Access:** Student (own) ✅, Teacher ✅, Admin ✅
- **Features:**
  - Score display
  - Question-by-question review
  - Correct answers (if enabled by teacher)
  - Feedback and explanations
  - Submission time
  - Score percentage
  - Grade indicator

#### `/proctoring`
- **Purpose:** Proctor dashboard for monitoring live assessments
- **Component:** `pages/assessments/ProctoringDashboard.tsx`
- **Policy:** `assessment_delivery.proctoring`
- **Role Access:** Teacher ✅, Admin ✅
- **Features:**
  - Live assessment sessions
  - Student list with progress
  - Time remaining per student
  - Flag suspicious activity
  - End session button
  - Notes on student behavior
  - Real-time alerts

---

### Diary Module

#### `/diary`
- **Purpose:** View lesson diary
- **Component:** `pages/diary/DiaryPage.tsx`
- **Policy:** `diary.view`
- **Role Access:** Teacher (own) ✅, Student (read-only) ✅, Admin ✅
- **Features:**
  - Diary entries feed (paginated)
  - Filter by date, class, subject
  - Search entries
  - Entry cards with preview
  - View full entry link
  - Approval status badge

#### `/diary/[entryId]`
- **Purpose:** View diary entry details
- **Component:** `pages/diary/DiaryEntryDetail.tsx`
- **Policy:** `diary.view`
- **Features:**
  - Full entry content (Tiptap display)
  - Attachments
  - Metadata (date, class, teacher, status)
  - Approval history
  - Comments (if enabled)
  - Download entry as PDF

#### `/diary/create`
- **Purpose:** Create new diary entry
- **Component:** `pages/diary/CreateDiaryEntry.tsx`
- **Policy:** `diary.create`
- **Role Access:** Teacher ✅
- **Features:**
  - Date picker
  - Class selection
  - Subject selection
  - Rich text editor (Tiptap)
  - File attachments
  - Save draft
  - Submit for approval

#### `/diary/[entryId]/edit`
- **Purpose:** Edit diary entry (draft only)
- **Component:** `pages/diary/EditDiaryEntry.tsx`
- **Policy:** `diary.edit`
- **Access:** Author or admin
- **Cannot:** Edit submitted/approved entries

#### `/diary/approvals`
- **Purpose:** Approve pending diary entries
- **Component:** `pages/diary/ApprovalsPage.tsx`
- **Policy:** `diary.approve`
- **Role Access:** Admin ✅, Institution Admin ✅
- **Features:**
  - Pending entries list
  - Review entry
  - Approve/reject buttons
  - Comments on approval
  - Bulk actions
  - Filter by teacher, date

---

### Notifications

#### `/notifications`
- **Purpose:** View all notifications
- **Component:** `pages/notifications/NotificationsPage.tsx`
- **Policy:** `notifications.view`
- **Role Access:** All authenticated users ✅
- **Features:**
  - Notification list (paginated)
  - Filter by type, read/unread
  - Sort by date
  - Mark as read
  - Mark all as read
  - Delete notification
  - Notification preferences link

#### `/notifications/settings`
- **Purpose:** Configure notification preferences
- **Component:** `pages/notifications/PreferencesPage.tsx`
- **Policy:** `notifications.preferences.manage`
- **Role Access:** All authenticated users ✅
- **Features:**
  - Toggle notification types
  - Email vs in-app preference
  - Quiet hours setting
  - Frequency settings
  - Subject-specific preferences

---

### Reporting & Analytics

#### `/reports`
- **Purpose:** View reports and dashboards
- **Component:** `pages/reports/ReportsPage.tsx`
- **Policy:** `reporting.view`
- **Role Access:** Admin ✅, Institution Admin ✅, Teacher (own class) ✅
- **Features:**
  - Report list
  - Filter by type
  - Predefined dashboards
  - Custom report builder
  - Drill-down capabilities
  - Export options (PDF, CSV, Excel)

#### `/reports/[reportId]`
- **Purpose:** View specific report
- **Component:** `pages/reports/ReportDetail.tsx`
- **Policy:** `reporting.view`
- **Features:**
  - Report title and description
  - Visualizations (charts, tables)
  - Date range selector
  - Filter options
  - Export button
  - Share link
  - Scheduled report info

#### `/analytics/assessments`
- **Purpose:** Assessment analytics dashboard
- **Component:** `pages/reports/AssessmentAnalytics.tsx`
- **Policy:** `reporting.view`
- **Role Access:** Admin ✅, Institution Admin ✅, Teacher (own class) ✅
- **Features:**
  - Class-wise performance
  - Subject-wise analysis
  - Question-wise analytics
  - Time analysis
  - Difficulty analysis
  - Score distribution

#### `/analytics/students`
- **Purpose:** Student progress tracking
- **Component:** `pages/reports/StudentAnalytics.tsx`
- **Policy:** `reporting.view`
- **Role Access:** Admin ✅, Institution Admin ✅, Teacher (own class) ✅
- **Features:**
  - Student list with scores
  - Progress chart
  - Attendance (if tracked)
  - Engagement metrics
  - Trend analysis

---

### Administration

#### `/admin/users`
- **Purpose:** Manage users (system-wide)
- **Component:** `pages/admin/UsersPage.tsx`
- **Policy:** `users.manage`
- **Role Access:** App Admin ✅
- **Features:**
  - User list (paginated, searchable)
  - Filter by role, status
  - Create new user
  - Edit user
  - Reset password
  - Disable/enable account
  - Delete user
  - Bulk import (CSV)
  - Export users (CSV)

#### `/admin/institution-users`
- **Purpose:** Manage institution users
- **Component:** `pages/admin/InstitutionUsersPage.tsx`
- **Policy:** `users.manage`
- **Role Access:** Institution Admin ✅
- **Features:**
  - Same as users, but institution-scoped
  - Assign to classes
  - Assign subjects (for teachers)

#### `/admin/classes`
- **Purpose:** Manage classes
- **Component:** `pages/admin/ClassesPage.tsx`
- **Policy:** `institutionadmin.classes.manage`
- **Role Access:** Institution Admin ✅, Admin ✅
- **Features:**
  - Class list
  - Create class
  - Edit class
  - Assign students
  - Assign teachers
  - Manage subjects
  - Import from CSV

#### `/admin/settings`
- **Purpose:** System settings
- **Component:** `pages/admin/SettingsPage.tsx`
- **Policy:** Admin only
- **Role Access:** App Admin ✅
- **Features:**
  - Branding settings (logo, colors)
  - SMTP configuration
  - Backup settings
  - User registration settings
  - Integration settings
  - Security policies
  - Audit log viewer

#### `/admin/institution-settings`
- **Purpose:** Institution settings
- **Component:** `pages/admin/InstitutionSettingsPage.tsx`
- **Policy:** Institution Admin only
- **Role Access:** Institution Admin ✅
- **Features:**
  - Institution name and info
  - Branding customization
  - Email templates
  - User creation settings

#### `/admin/audit-logs`
- **Purpose:** View audit trail
- **Component:** `pages/admin/AuditLogsPage.tsx`
- **Policy:** Admin only
- **Role Access:** App Admin ✅
- **Features:**
  - Action log
  - Filter by user, action, date
  - Search
  - Export log
  - Real-time log viewer

---

## Route Structure in Code

```
client/app/
├── layout.tsx                          # Root layout with auth check
├── page.tsx                            # Home/redirect to dashboard
├── (auth)/
│   ├── layout.tsx                      # Auth layout (no sidebar)
│   ├── login/page.tsx
│   ├── logout/page.tsx
│   ├── password-reset/page.tsx
│   ├── password-reset/[token]/page.tsx
│   └── unauthorized/page.tsx
├── (app)/
│   ├── layout.tsx                      # App layout with sidebar, auth required
│   ├── dashboard/page.tsx
│   ├── profile/page.tsx
│   ├── schedule/
│   │   ├── page.tsx
│   │   └── [periodId]/page.tsx
│   ├── lessons/
│   │   ├── page.tsx
│   │   ├── [lessonId]/page.tsx
│   │   ├── [lessonId]/edit/page.tsx
│   │   ├── create/page.tsx
│   │   └── templates/page.tsx
│   ├── assessments/
│   │   ├── page.tsx
│   │   ├── [assessmentId]/page.tsx
│   │   ├── [assessmentId]/edit/page.tsx
│   │   ├── [assessmentId]/submissions/page.tsx
│   │   ├── [assessmentId]/sessions/page.tsx
│   │   ├── [assessmentId]/results/page.tsx
│   │   ├── create/page.tsx
│   │   └── question-bank/page.tsx
│   ├── take-assessment/[sessionId]/page.tsx
│   ├── diary/
│   │   ├── page.tsx
│   │   ├── [entryId]/page.tsx
│   │   ├── [entryId]/edit/page.tsx
│   │   ├── create/page.tsx
│   │   └── approvals/page.tsx
│   ├── notifications/
│   │   ├── page.tsx
│   │   └── settings/page.tsx
│   ├── reports/
│   │   ├── page.tsx
│   │   ├── [reportId]/page.tsx
│   │   ├── assessments/page.tsx
│   │   └── students/page.tsx
│   └── admin/
│       ├── users/page.tsx
│       ├── institution-users/page.tsx
│       ├── classes/page.tsx
│       ├── settings/page.tsx
│       ├── institution-settings/page.tsx
│       └── audit-logs/page.tsx
```

## Navigation & Sidebar

Main sidebar navigation for `(app)` layout:

**For Teachers:**
- 📊 Dashboard
- 📚 Lessons → Browse, Create
- 📝 Assessments → Browse, Create, Question Bank
- 📖 Diary → View, Create, Pending Approvals (if admin)
- 📅 Schedule
- 🔔 Notifications
- ⚙️ Profile

**For Students:**
- 📊 Dashboard
- 📚 Lessons
- 📝 Assessments → Assigned, Results
- 📖 Diary (view-only)
- 📅 Schedule
- 🔔 Notifications
- ⚙️ Profile

**For Admins:**
- 📊 Dashboard
- 📚 Lessons
- 📝 Assessments
- 📖 Diary
- 📅 Schedule
- 📈 Reports
- ⚙️ Admin → Users, Classes, Settings, Audit Logs
- 🔔 Notifications
- ⚙️ Profile

## Middleware & Guards

### Route Middleware
- **Authentication Check:** All `(app)` routes require valid JWT token
- **Token Refresh:** Automatic refresh on 401 response
- **Permission Check:** API calls validate permissions via `[Authorize]` attributes
- **Role-Based UI:** Components check roles/permissions and show/hide appropriately

### Session Timeout
- **Timeout:** 30 minutes of inactivity
- **Warning:** 5-minute warning before timeout
- **Action:** Auto-logout, redirect to login with message

## Error Handling

- **404 Not Found** → Show not found page with back button
- **403 Forbidden** → Redirect to `/unauthorized`
- **401 Unauthorized** → Redirect to `/login`
- **500 Server Error** → Show error page with support contact
- **Network Error** → Retry with exponential backoff, then error message
