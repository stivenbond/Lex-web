# Domain Specification: Scheduling Core

## 1. Context
- **Module**: Scheduling
- **Scope**: Temporal organization and physical resource (classroom) allocation for the educational institution.

## 2. Core Ubiquitous Language
- **Academic Year**: The highest level grouping (e.g., Aug-May).
- **Term**: A semester or trimester within a year.
- **Period**: A template for a daily time block (e.g., "Period 2" is always 09:30-10:20).
- **Slot**: A realization of a Period on a specific Date in a specific Classroom.
- **Allocation**: The binding of a Class (students) and a Teacher to a Slot.

## 3. Domain Model Hierarchy

- **Aggregate Root: AcademicYear**
    - **Purpose**: To manage the overarching calendar and ensure terms do not overlap.
    - **Invariants**: 
        - Terms must be contained within the year's start/end dates.
        - Terms must not overlap.

- **Entity: Period**
    - **Purpose**: Define the daily rhythm.
    - **Invariants**: Start time must be before end time.

- **Aggregate Root: Slot**
    - **Purpose**: The unit of attendance and scheduling.
    - **Invariants**: 
        - A classroom cannot have two slots at the same time on the same date.
        - A teacher cannot be allocated to two slots at the same time.
        - A class (students) cannot be allocated to two slots at the same time.

## 4. Value Objects
- **TimeRange**: Start and End times.
- **DateRange**: Start and End dates.

## 5. Domain Events
- **AcademicYearCreated**: Published when a new year is defined.
- **SlotAssigned**: Published when a class is allocated to a slot.
- **SlotRemoved**: Published when an allocation is cancelled.

## 6. Business Operations (Conceptual)
- **Op: Create Academic Year**
    - **Resulting State**: New `AcademicYear` with child `Term` entities.
- **Op: Assign Class to Slot**
    - **Invariants Checked**: Teacher availability, Room availability, Class availability.
    - **Resulting State**: `Slot` is marked as `Allocated`.
