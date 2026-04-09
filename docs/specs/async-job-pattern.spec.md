# Specification: Async Job Pattern Integration

## 1. Overview
- **Category**: Cross-Cutting Concern / Architecture Pattern
- **Parent Module**: FileProcessing (Implementation host)
- **Description**: Define the "202 Accepted + SignalR" pattern for long-running jobs to provide a responsive user experience.

## 2. Technical Strategy
- **Protocol**: HTTP 202 for initiation, SignalR (WebSockets) for real-time state updates.
- **Components**: `JobStatusHub` in `Lex.Infrastructure`, `useAsyncJob()` hook in Frontend.

## 3. Implementation Details

### Initiation Flow
1.  **Request**: Client dispatches a command that triggers a long-running job.
2.  **Immediate Response**: API returns `202 Accepted` with a `JobId` (GUID) and a `Location` header to the status endpoint.
3.  **Background Work**: The job starts in a MassTransit consumer.

### Real-time Progress (SignalR)
- Every significant step in the `FileProcessing` worker dispatches a `JobProgressUpdated` message to Redis.
- The `JobStatusHub` listens to this and broadcasts to the specific caller:
    ```json
    {
      "jobId": "guid",
      "percentComplete": 45,
      "message": "Extracting text from page 5...",
      "status": "Processing"
    }
    ```

### Completion & Polling
- **Terminal State**: Once the job is `Completed` or `Failed`, a final SignalR message is sent.
- **Polling Fallback**: If the SignalR connection is lost, the frontend uses the `Location` header to poll the job status every 5 seconds.

## 4. Configuration & Settings
- **Redis**: Required for backplane status synchronization.
- **SignalR Hub**: `/hubs/job-status`.

## 5. Resilience & Performance
- **Connection Loss**: The `useAsyncJob` hook must handle automatic reconnection to the hub.
- **Idempotency**: Polling the status endpoint must be side-effect free and allow for high-frequency checks.

## 6. Integration Points
- Used by: `FileProcessing`, `ImportExport`, `ReportGeneration`.

## 7. Migration & Deployment
- **Verification**: Simulate a 30-second PDF extraction and verify the browser console shows progress percentage increments.
