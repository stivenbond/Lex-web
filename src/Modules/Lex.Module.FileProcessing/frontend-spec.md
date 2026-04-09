# Frontend Specification: FileProcessing UI Components

## 1. Overview
- **Parent Module**: FileProcessing
- **Target Pages**: integrated into `ObjectStorage` (File Explorer) and `Lesson/Diary` (Editors).

## 2. Views & Components

### File Processing Status Indicator
- **Function**: Provide visual feedback on the progress of background extraction.
- **Components**:
    - **ProgressIcon**: A spinner or progress circle displayed next to a PDF file name.
    - **StatusTooltip**: On hover, show "Extracting text... 45%" or "Failed to process".
    - **ResultBadge**: A "Text Extracted" icon that appears once processing is successful.

### Processing Details (Admin/Developer)
- **Function**: Detailed monitoring for troubleshooting.
- **Features**:
    - **JobLog**: A modal showing the step-by-step logs of the worker.
    - **RetryButton**: Manually re-trigger processing if it failed.

## 3. Data Fetching & State
- **Store**: `useProcessingStore` (Zustand).
- **Hooks**:
    - `useFileProcessingProgress(fileId)`: Subscribes to the `JobStatusHub` for a specific file and returns the real-time percent completion.
- **Workflow**:
    - When a file is uploaded, the UI automatically enters "Processing" state until a `Completed` or `Failed` message is received via SignalR.

## 4. UI Library & Styling
- **Animations**: `framer-motion` for the progress circle transition.
- **Icons**: `FileSearch` (Processing), `FileCheck` (Completed), `FileWarning` (Failed).

## 5. Security
- Only authors of a file or Admins can see the detailed processing logs.
