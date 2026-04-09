# Domain Specification: File Transcription & Transformation

## 1. Context
- **Module**: FileProcessing
- **Scope**: Automation of media analysis and derivative content creation.

## 2. Core Ubiquitous Language
- **ProcessingJob**: A managed task that operates on a single file.
- **Worker**: The component executing the actual CPU-intensive logic.
- **Derivative**: A secondary file created from a primary (e.g., a thumbnail).

## 3. Domain Model Hierarchy

- **Aggregate Root: ProcessingJob**
    - **Purpose**: Tracks the lifecycle and auditing of a background task.
    - **State Machine**: `Pending` -> `Running` -> `Completed` | `Failed`.
    - **Invariants**: 
        - Must reference a valid `FileId` from ObjectStorage.
        - Must have a `JobType` (e.g., Thumbnail, OCR).

## 4. Job Types
- **ImgThumb**: Generates a 200x200 webp thumbnail.
- **PdfText**: Extracts searchable text content for the Reporting or Search modules.
- **VidProbe**: Retrieves duration and codec info.

## 5. Domain Events
- **FileProcessingCompletedEvent**: Contains pointers to derivatives or extracted metadata.
- **FileProcessingFailedEvent**: Contains the error reason for UI alerting.

## 6. Business Operations (Conceptual)
- **Op: Enqueue**: Creates the job record and triggers the background worker.
- **Op: ReportProgress**: (Optional) Updates the completion percentage for the UI.
- **Op: Finalize**: Records the output and marks the job done.
