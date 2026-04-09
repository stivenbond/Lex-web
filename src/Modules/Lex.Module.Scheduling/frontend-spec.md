# Frontend Specification: Scheduling Views

## 1. Overview
- **Parent Module**: Scheduling
- **Target Pages**: 
    - `/scheduling/timetable` (Main grid)
    - `/scheduling/slots/[id]` (Detail view)

## 2. Views & Components

### Weekly Timetable Grid
- **Function**: Display a 5-day or 7-day grid of periods and class allocations.
- **Layout**:
    - **Y-Axis**: Time Periods (Period 1, Period 2, etc.).
    - **X-Axis**: Dates of the current week.
- **Interactions**:
    - Clicking a cell opens the "Assign Class" modal if empty.
    - Clicking an existing slot navigates to the Slot Detail view.
    - Drag-and-drop support: Move a class allocation between periods (calls `UpdateSlot`).
    - Filter: Toggle between "Teacher View" and "Class View".

### Slot Detail View
- **Function**: Show comprehensive info about a specific scheduled period.
- **Data Displayed**:
    - Subject Name.
    - Assigned Teacher.
    - Classroom location.
    - Link to the current Lesson Plan (from `LessonManagement` module).
    - Quick actions: "Mark Attendance", "View Diary Entry".

### Admin Tools (Modal)
- **Function**: Setup the academic temporal structure.
- **Components**:
    - **Academic Year Wizard**: Setup start/end dates and vacations.
    - **Period Editor**: Define school start/end time and duration of each period.

## 3. Data Fetching & State
- **Store**: `useSchedulingStore` (Zustand).
- **Hooks**:
    - `useWeeklySchedule(entityId, type)`: Fetches data for the grid.
    - `usePeriods(date)`: Helper for period definitions.
- **Real-time**: Listen for `SlotAssignedEvent` and `SlotRemovedEvent` via SignalR to refresh the grid without manual reload.

## 4. UI Library & Styling
- **Grid Component**: Custom CSS Grid or `shadcn/ui` based calendar component.
- **Drag-and-Drop**: `dnd-kit` or native HTML5 DnD.
- **Animations**: `framer-motion` for smooth layout transitions when filtering views.

## 5. Security
- Use `usePermissions()` hook.
- Only show "Assign" / "Delete" buttons to users with `SchedulingPermissions.Manage`.
