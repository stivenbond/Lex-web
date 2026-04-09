# Specification: Notification Event Mapping

## 1. Overview
- **Category**: Cross-Module Integration Routing
- **Parent Module**: Notifications
- **Purpose**: Define exactly which domain events from other modules trigger user-facing notifications, who receives them, and the template used.

## 2. Event Mapping Table

| Source Module | Event Name | Recipient(s) | Template Key | Channel(s) |
| :--- | :--- | :--- | :--- | :--- |
| **Diary** | `DiaryEntrySubmitted` | Parent/Supervisor | `Diary_ReviewRequired` | In-App, Email |
| **Diary** | `DiaryEntryApproved` | Teacher | `Diary_Approved` | In-App |
| **Scheduling** | `SlotAssigned` | Teacher | `Schedule_SlotAssigned` | In-App |
| **Lessons** | `LessonPublished` | Students in Class | `Lesson_Ready` | In-App (Push) |
| **Assessments** | `AssessmentPublished` | Students assigned | `Assessment_New` | In-App, Email |
| **Assessments** | `AssessmentAttemptFinished` | Teacher | `Assessment_SubmissionReceived` | In-App |
| **Processing** | `FileProcessingFailed` | File Owner | `File_ProcessingFailed` | In-App, Email |

## 3. Variable Injection Logic
Each consumer in the `Notifications` module will be responsible for extracting the necessary metadata from the event to populate the template.

**Example: LessonPublished**
- **Trigger**: `LessonPlanPublishedEvent`
- **Data Extracted**: `LessonTitle`, `SubjectName`, `ClassName`.
- **Logic**: Query the `Scheduling` or `Core` module to find all students currently enrolled in the affected Class, then dispatch a notification to each.

## 4. Implementation Strategy
- Use **MassTransit Consumers** within the `Lex.Module.Notifications.Infrastructure` project.
- Each consumer corresponds to one or more events from the table above.
- The consumer calls the internal `INotificationDispatcher` which handles preferences and delivery.

## 5. Security Note
- The mapping logic must respect user privacy. For example, `AssessmentAttemptFinished` should never be sent to anyone except the student's assigned teacher or an admin.
