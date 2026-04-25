# Module Specification: Scheduling

## 1. Overview
- **Name**: Scheduling
- **Purpose**: Core backbone for time-table management, period definitions, and resource/class allocation.
- **Schema**: `scheduling.*`
- **Primary Stakeholders**: Teachers (view/edit slots), Admins (create years/terms), Students (view timetable).

## 2. Domain Boundaries
- **Aggregate Root: AcademicYear**: Owns Terms and Holidays.
- **Aggregate Root: ScheduleBoard**: Owns the collective allocation of Slots to Classes and Teachers.
- **Entity: Period**: A time boundary within a day (e.g., Period 1, 09:00 - 10:00).
- **Entity: Slot**: An assignment of a Teacher, a Class, and a Subject to a specific Period and Room on a specific Date.

## 3. Module Responsibilities
- Definition of the academic calendar (Years, Terms, Non-teaching days).
- Management of master timetables.
- Validation of scheduling conflicts (Room double-booking, Teacher over-subscription).

## 4. Integration & Dependencies
- **Inbound Events**: None (Primary module).
- **Outbound Events**: 
    - `AcademicYearCreatedEvent`
    - `SlotAssignedEvent`
    - `SlotRemovedEvent`
- **External Dependencies**: None.

## 5. Security & Authorization
- **Permission Constants**: `SchedulingPermissions.cs`
- **Policies**: `RequireSchedulingAdmin`, `RequireTeacherView`.

## 6. Cross-Module Interactions
- **DiaryManagement**: Subscribes to `SlotAssignedEvent` to prepopulate daily diary entries.
- **LessonManagement**: References `SlotId` to link lesson plans to specific teaching instances.
