# Module Specification: Scheduling

## 1. Overview
- **Name**: Scheduling
- **Purpose**: Manage the academic calendar, including years, terms, periods, and the allocation of classes to specific time slots.
- **Schema**: `scheduling.*`
- **Primary Stakeholders**: Teachers (view/manage schedule), Students (view schedule), Admins (setup academic year).

## 2. Domain Boundaries
List the main Aggregate Roots and Entities managed by this module.

- **Aggregate Root: AcademicYear**: Represents a full school year (e.g., 2026-2027).
- **Entity: Term**: A subdivision of an academic year (e.g., Semester 1).
- **Entity: Period**: A defined time block in a day (e.g., Period 1: 08:30 - 09:20).
- **Aggregate Root: Slot**: A specific intersection of a Period, Classroom, and Date.
- **Entity: ClassAllocation**: Assigns a Class and Teacher to a Slot.

## 3. Module Responsibilities
- Define the academic temporal structure (Years, Terms, Periods).
- Maintain the master timetable (Slots and Allocations).
- Provide schedule lookups for classes and teachers.
- Publish events when slots are assigned or removed for downstream modules (Diary, Lessons).

## 4. Integration & Dependencies
- **Inbound Events**: None (Master module).
- **Outbound Events**: 
    - `AcademicYearCreatedEvent`
    - `SlotAssignedEvent`
    - `SlotRemovedEvent`
- **External Dependencies**: None.

## 5. Security & Authorization
- **Permission Constants**: `SchedulingPermissions.cs`
- **Policies**: 
    - `CanManageCalendar`: Required to create years/terms/periods.
    - `CanAssignSlots`: Required to modify the timetable.
    - `CanViewSchedule`: General access to read the calendar.

## 6. Cross-Module Interactions
- `DiaryManagement` uses `SlotAssignedEvent` to create placeholder entries.
- `LessonManagement` references `Slot` IDs to associate lesson plans with specific delivery times.
