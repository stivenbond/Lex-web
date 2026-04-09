# Specification: Google Calendar Synchronization

## 1. Overview
- **Category**: Third-Party Service Integration / Event Synchronization
- **Parent Module**: GoogleIntegration
- **Feature Area**: Calendar / Scheduling
- **Purpose**: Mirror Lex platform schedule slots (lectures, labs) into the teacher's personal Google Calendar.

## 2. Technical Strategy
- **Reactive Pattern**: Listen for `SlotAssignedEvent` and `SlotRemovedEvent` from the `Scheduling` module.
- **Mapping**: Map a Lex `Slot` to a Google `Event`.

## 3. Implementation Details

### Sync Logic
1.  **Consumer**: `SchedulingEventsConsumer` in `GoogleIntegration.Infrastructure`.
2.  **Lookup**: Retrieve the `GoogleAccountLink` for the teacher ID found in the event.
3.  **Operation**: 
    - **Created**: Call `events.insert` on Google Calendar API. Store the generated `GoogleEventId` in the `SyncOperation` entity for future updates.
    - **Deleted**: Call `events.delete` using the stored `GoogleEventId`.
4.  **Data Mapping**:
    - **Summary**: `[Lex] - {SubjectName} ({ClassCode})`
    - **Start/End**: Exact match from Lex Slot.
    - **Description**: Contains a deep link back to the Lex Lesson Plan for that slot.

### Handling Conflicts
- **Single Source of Truth**: The Lex platform is the source of truth. If an event is modified on the Google side, Lex will overwrite it during the next sync pulse (One-way sync).

## 4. Configuration & Settings
- **Scopes**: `https://www.googleapis.com/auth/calendar.events`.
- **Primary Calendar**: Always sync to the user's "primary" calendar by default.

## 5. Resilience
- **Batching**: If multiple slots are assigned at once (e.g., semester import), use the Google API `batch` request system.
- **Retry Policy**: Exponential backoff for `503 Service Unavailable` errors.
