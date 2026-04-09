# Feature Specification: Assign Class to Slot

## 1. Feature Overview
- **Parent Module**: Scheduling
- **Description**: Allocate a specific class (group of students) and a teacher to a predefined timetable slot.
- **Type**: Command
- **User Story**: As an Admin/Scheduler, I want to assign a class to a period so that students and teachers know when and where to meet.

## 2. Request Representation
- **Request Class**: `AssignClassToSlotCommand`
- **Payload Structure**: 
    - `PeriodId` (Guid)
    - `ClassId` (Guid)
    - `TeacherId` (Guid)
    - `ClassroomId` (Guid)
    - `Date` (Date)
- **Validation Rules**: 
    - All IDs must be valid.
    - Date must be within a defined Term.

## 3. Business Logic (The Slice)
- **Trigger**: `REST POST /api/scheduling/allocations`
- **Domain Logic**: 
    - Check for conflicts: Teacher, Class, or Room already booked at this Period/Date?
    - Call `Slot.Allocate(...)`.
- **Side Effects**: Publishes `SlotAssignedEvent`.

## 4. Persistence
- **Affected Tables**: `scheduling.slots`, `scheduling.class_allocations`
- **Repository Methods**: `GetWithAllocationsAsync`, `SaveAsync`.

## 5. Domain Objects (High-Level)
- **Slot**: Aggregate Root.
- **ClassAllocation**: Entity within Slot.

## 6. API / UI Contracts
- **Route**: `POST /api/scheduling/allocations`
- **Response**: `Result<Guid>` (The Slot ID).

## 7. Security
- **Required Permission**: `SchedulingPermissions.AssignSlots`
- **Auth Policy**: `RequireAdmin` or `RequireScheduler`
