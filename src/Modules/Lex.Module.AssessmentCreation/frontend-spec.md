# Frontend Specification: AssessmentCreation Views

## 1. Overview
- **Parent Module**: AssessmentCreation
- **Target Pages**: 
    - `/assessments/library` (Teacher's authored exams)
    - `/assessments/builder/[id]` (The visual authoring environment)
    - `/assessments/bank` (The question library)

## 2. Views & Components

### Assessment Builder (Visual)
- **Function**: Drag-and-drop environment for structuring exams.
- **Components**:
    - **Structure Tree**: Left sidebar showing the hierarchy of Sections and Questions.
    - **Canvas**: Centering the active Section/Question for editing.
    - **React Flow / Dnd-Kit Builder**: Visual representation of the assessment flow, allowing users to drag questions between sections.
    - **Properties Panel**: Right sidebar for configuring scoring rules, question types, and metadata.

### Question Editor
- **Function**: Specialized editors for each question type.
- **Variants**:
    - **MCQ Editor**: Add/Remove options, toggle "Is Correct".
    - **Essay Editor**: Tiptap instance for the inquiry text + Rubric builder.
    - **Short Answer Editor**: Input field for the regex/expected string.

### Question Bank Drawer
- **Function**: Searchable access to the shared question library while building an exam.
- **Features**:
    - "Add to Section" button on each bank item.
    - Filter by tags (e.g., #Geometry, #Calculus).

## 3. Data Fetching & State
- **Store**: `useAssessmentBuilderStore` (Zustand).
- **Hooks**:
    - `useAssessment(id)`: Fetches structure.
    - `useQuestionBank(search)`: Search functionality.
- **Persistence**: Auto-save integration (saving current draft state on every structural change).

## 4. UI Library & Styling
- **Shared Components**: `Accordion` (for sections), `Dialog` (for question banks), `Skeleton` (for loading builder states).
- **Layout**: Three-pane layout (Navigation | Canvas | Properties).
- **Icons**: Lucide icons for question types (e.g., `CircleDot` for MCQ, `FileText` for Essay).

## 5. Security
- Read-only builder if the assessment is `Published`.
- Enforcement of `CanManageAssessments` at the page route level.
