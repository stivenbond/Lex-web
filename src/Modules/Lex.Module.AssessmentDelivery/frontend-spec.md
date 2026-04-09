# Frontend Specification: AssessmentDelivery Views

## 1. Overview
- **Parent Module**: AssessmentDelivery
- **Target Pages**: 
    - `/delivery/player/[sessionId]` (The student's testing environment)
    - `/delivery/results/[sessionId]` (Result summary for students)
    - `/delivery/grading/[sessionId]` (Teacher's manual grading interface)

## 2. Views & Components

### Secure Student Player
- **Function**: Provide a distraction-free UI for taking assessments.
- **Features**:
    - **Header**: Assessment title and "Finish" button.
    - **Dynamic Navigation**: Tabbed or sequential view of sections and questions.
    - **Question Renderer**: Polymorphic component that displays the question content (HTML/BlockContent) and the appropriate input field (Radio buttons for MCQ, Textarea for Short Answer, etc.).
    - **Autosave Engine**: Background service that debounces `PUT` requests to `SaveAnswer`.
    - **Connection Monitor**: Discreet indicator showing if data is successfully syncing to the server.

### Student Results View
- **Function**: Display scored results once grading is finalized.
- **Features**:
    - **Score Summary**: Percentage and pass/fail indicator.
    - **Review List**: Detailed list of questions showing student ansers, correct answers, and teacher feedback for essays.

### Proctor Dashboard (Grading View)
- **Function**: Allow teachers to manually grade subjective questions.
- **Features**:
    - **Answer Preview**: Large view of the student's essay text or uploaded file.
    - **Rubric Helper**: Collapsible panel showing the grading criteria from the snapshot.
    - **Score Input**: Numeric input field for entering the final score for that specific question.

## 3. Data Fetching & State
- **Store**: `usePlayerStore` (Zustand).
- **Hooks**:
    - `useSession(id)`: Fetches data and initializes autosave.
    - `useGradingQueue()`: List of items for the teacher review.
- **Fidelity**: The player must handle page refreshes gracefully using local storage as a secondary cache for unsynced answers.

## 4. UI Library & Styling
- **Shared Components**: `Progress` (progress bar for test completion), `Dialog` (for final submission confirmation).
- **Layout**: Simplified layout with no external links or distraction sidebars.

## 5. Security
- **Strict Mode**: Optional logic to detect if the student leaves the browser tab (to be implemented via `Page Visibility API`).
- **Read-only Results**: Ensure the player cannot be re-opened once a session is `Submitted`.
