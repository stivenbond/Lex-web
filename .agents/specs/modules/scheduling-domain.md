# Domain Specification: Scheduling Core

## 1. Context
- **Module**: Scheduling
- **Scope**: Core temporal modeling for academic structures (Years, Terms) and daily operation (Periods, Slots).

## 2. Core Ubiquitous Language
- **AcademicYear**: A high-level container for a full year of education, spanning multiple terms.
- **Term**: A subdivision of an academic year (e.g., Spring Term).
- **Period**: A reusable template for a time block in a school day (e.g., "Period 1: 08:30-09:30").
- **Slot**: The intersection of a **Period**, a **Class**, a **Teacher**, a **Subject**, and a **Date**. This is the unit of work.

## 3. Domain Model Hierarchy

- **Aggregate Root: AcademicYear**
    - **Purpose**: Defines the boundaries of the school year.
    - **Entities**: `Term` (List of terms within the year).
    - **Invariants**: 
        - Terms must not overlap.
        - The start date must precede the end date.

- **Aggregate Root: PeriodTemplate**
    - **Purpose**: Defines what a standard "day" looks like in the institution.
    - **Entities**: `Period`.
    - **Invariants**: 
        - Periods must not overlap in time.
        - Each period must have a unique identifier (e.g., Name/Sequence).

- **Aggregate Root: ScheduleSlot**
    - **Purpose**: Represents a single instance of a teaching event.
    - **Invariants**:
        - A Teacher cannot be in two slots at the same time.
        - A Classroom cannot host two slots at the same time.
        - A Class cannot have two slots at the same time.

## 4. Value Objects
- **TimeRange**: Composition of StartTime and EndTime.
- **DateRange**: Composition of StartDate and EndDate.

## 5. Domain Events
- **AcademicYearCreatedEvent**: Triggered when a new year is finalized.
- **SlotAssignedEvent**: Triggered when a teacher/class is scheduled into a period.
- **SlotRemovedEvent**: Triggered when a scheduled event is cancelled.

## 6. Business Operations (Conceptual)
- **Op: SetupAcademicYear**: Initializes the year and terms.
- **Op: GenerateSlotsFromTemplate**: (Future) Batch generates slots based on a master timetable.
- **Op: AssignSlot**: Creates a specific teaching instance.
