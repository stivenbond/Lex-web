# Frontend Specification: DiaryManagement Views

## 1. Overview
- **Parent Module**: DiaryManagement
- **Target Pages**: 
    - `/diary/feed/[classId]` (Student/Parent view)
    - `/diary/editor/[id]` (Teacher editor)
    - `/diary/review` (Supervisor dashboard)

## 2. Views & Components

### Class Diary Feed
- **Function**: Infinite scroll or paged list of diary entries for a class.
- **Components**:
    - **DiaryCard**: Shows date, subject, snippet of content, and attachment icons.
    - **DateFilter**: Jump to a specific week or month.
    - **SubjectFilter**: Toggle between subjects (Science, Math, etc.).

### Teacher's Diary Editor
- **Function**: Full-page editor for lesson documentation.
- **Components**:
    - **Tiptap Editor**: Using the `BlockContent` extensions (paragraphs, images, diagrams).
    - **Attachment Sidebar**: 
        - Drag-and-drop zone for file uploads.
        - Status indicator for uploads (In-progress -> Done).
        - List of current attachments with delete buttons.
    - **Context Bar**: Displays slot info (Class 10A, Period 2, Monday 14th).
    - **StatusBar**: Tracks "Draft", "Submitted", or "Approved" status.

### Supervisor Review Dashboard
- **Function**: List of entries requiring approval.
- **Components**:
    - **ApprovalList**: Filterable table of `Submitted` entries.
    - **QuickPreview**: Modal showing the entry content without full navigation.
    - **CommentModal**: For providing feedback during rejection.

## 3. Data Fetching & State
- **Store**: `useDiaryStore` (Zustand).
- **Hooks**:
    - `useDiaryEntry(id)`: Fetch full entry.
    - `useFileUploader()`: Encapsulates the multi-stage upload to `ObjectStorage` and subsequent ID linkage.
- **Auto-save**: The editor implementation will use a debounced `PUT /api/diary/{id}` call.

## 4. UI Library & Styling
- **Shared Components**: `Card`, `Badge` (for status), `Button` from `shadcn/ui`.
- **Editor Styling**: Clean, focused interface similar to Notion or Medium.
- **File Icons**: Category-based icons (PDF, Image, Doc) for attachments.

## 5. Security
- Use `usePermissions()` to enable the "Submit" and "Approve" buttons appropriately.
- Ensure the editor is read-only if the entry is already `Approved`.
