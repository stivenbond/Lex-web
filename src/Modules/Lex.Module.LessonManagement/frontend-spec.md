# Frontend Specification: LessonManagement Views

## 1. Overview
- **Parent Module**: LessonManagement
- **Target Pages**: 
    - `/lessons/library` (The index of all plans)
    - `/lessons/editor/[id]` (The rich-text planning interface)

## 2. Views & Components

### Lesson Library
- **Function**: Paged list or grid of all lesson plans owned by the teacher or shared with the teacher.
- **Features**:
    - Search by title.
    - Filter by `Subject`, `Status` (Draft vs Published), and `Date`.
    - "Use Template" action: Clone an existing published lesson into a new draft.

### Lesson Editor (Tiptap)
- **Function**: The primary interface for lesson design.
- **Components**:
    - **Editable Canvas**: Full-width Tiptap editor support for `BlockContent` types.
    - **Resource Panel**:
        - List of attached files.
        - "Attach More" button (links to ObjectStorage file picker).
        - Drag-and-drop support: Drag a resource into the editor canvas to create an "Embed" or "Link" block.
    - **Metadata Sidebar**: Change subject, title, and target academic levels.
    - **Version History**: View previously published versions of this plan.

### Resource Panel (Side Drawer)
- **Function**: Context-aware access to resources while editing.
- **Features**:
    - Quick-preview for PDF and images.
    - Action to "Open in new tab" for full documents.

## 3. Data Fetching & State
- **Store**: `useLessonStore` (Zustand).
- **Hooks**:
    - `useLessonPlan(id)`: Fetches full plan and resources.
    - `useSubjectLibrary(subjectId)`: Paged fetching for the library view.
- **Real-time**:
    - Auto-save integration (debounced `PUT` calls).
    - Status badges update automatically when a plan is published.

## 4. UI Library & Styling
- **Shared Components**: `DataTable` (for library), `Sheet` (for resource panel), `Alert` (for immutable warnings).
- **Typography**: Optimized for long-form reading and writing.

## 5. Security
- Read-only mode activated if `Status == Published`.
- Only show "Publish" button if the user has `LessonPermissions.Publish`.
