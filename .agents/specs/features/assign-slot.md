# Feature Specification: Assign Class to Slot

## 1. Feature Overview
- **Parent Module**: Scheduling
- **Description**: Places a class and teacher into a specific period on a specific day.
- **Type**: Command
- **User Story**: As a Scheduler, I want to assign a teacher and a class to a period so that the timetable is populated.

## 2. Request Representation
- **Request Class**: `AssignSlotCommand`
- **Payload Structure**:
    - `PeriodId` (Guid)
    - `ClassId` (Guid)
    - `TeacherId` (Guid)
    - `SubjectId` (Guid)
    - `RoomId` (Guid)
    - `Date` (DateTime)
- **Validation Rules**:
    - All IDs must correspond to existing entities.
    - Date must be within an active Term.

## 3. Business Logic (The Slice)
- **Trigger**: REST POST `/api/scheduling/slots`
- **Domain Logic**:
    - Checks for Teacher availability (cross-slot check).
    - Checks for Room availability.
    - Checks for Class availability.
- **Side Effects**: Publishes `SlotAssignedEvent`.

## 4. Persistence
- **Affected Tables**: `scheduling.slots`
- **Repository Methods**: `IScheduleRepository.AddSlotAsync()`

## 5. Domain Objects (High-Level)
- **ScheduleSlot**: The core entity representing the assignment.

## 6. API / UI Contracts
- **Route**: `POST /api/scheduling/slots`
- **Response**: `Result<Guid>`

## 7. Security
- **Required Permission**: `SchedulingPermissions.AssignSlots`
- **Auth Policy**: `RequireAdmin`
