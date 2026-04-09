# Domain Specification: File Processing Jobs

## 1. Context
- **Module**: FileProcessing
- **Scope**: Lifecycle management of asynchronous file transformations and extraction tasks.

## 2. Core Ubiquitous Language
- **ProcessingJob**: A managed task unit for a specific file operation.
- **Worker**: The background process or service executing the task.
- **Extraction**: The process of pulling semantic content (text) from a file.
- **JobQueue**: The prioritized list of pending processing jobs.

## 3. Domain Model Hierarchy

- **Aggregate Root: ProcessingJob**
    - **Purpose**: Tracks a single file operation from enqueue to completion.
    - **Invariants**:
        - Must encompass a valid `FileId` reference.
        - Must record `StartTime` and `CompletionTime`.
        - Transitions are strictly linear: `Queued` -> `Processing` -> (`Completed` | `Failed`).
- **Entity: ProcessingResult**
    - **Purpose**: Store the outcome. For PDF extraction, this includes the raw text and confidence metrics.

## 4. Value Objects
- **JobStatus**: Enum (`Queued`, `Processing`, `Completed`, `Failed`).
- **JobType**: Enum (`PdfTextExtraction`).

## 5. Domain Events
- **FileProcessingStarted**: Published when a worker picks up the job.
- **FileProcessingCompleted**: Published with the extraction result.
- **FileProcessingFailed**: Published with error details for retry logic.

## 6. Business Operations (Conceptual)
- **Op: EnqueueJob**
    - **Logic**: Creates a job in `Queued` state. Triggered by a new file upload.
- **Op: UpdateProgress**
    - **Logic**: Updates the percentage completion. Used for SignalR push.
- **Op: FinalizeJob**
    - **Invariants Checked**: Result must be present if status is `Completed`.
