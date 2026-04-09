# API Specification: FileProcessing Module

## 1. Overview
- **Parent Module**: FileProcessing
- **Base Route**: `/api/processing`
- **Purpose**: Internal and external monitoring of file processing jobs.

## 2. API Endpoints

### Job Monitoring
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/jobs/{id}` | `GetJobStatusById` | Fetch current status, progress, and result if completed. |
| `GET` | `/jobs/file/{fileId}` | `GetJobsByFileId` | List all processing tasks associated with a file. |
| `DELETE` | `/jobs/{id}` | `CancelJob` | Attempt to stop a queued or processing job. |

### Manual Trigger (Admin/Internal)
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/jobs/reprocess/{fileId}` | `EnqueueProcessingJob` | Manually restart processing for a file. |

## 3. Request/Response Shapes

### Resource: ProcessingJobDto
```json
{
  "id": "guid",
  "fileId": "guid",
  "type": "PdfTextExtraction",
  "status": "Queued | Processing | Completed | Failed",
  "percentComplete": 45,
  "message": "Step 2 of 3...",
  "result": {
    "extractedText": "...",
    "error": null
  },
  "queuedAt": "datetime",
  "completedAt": "datetime | null"
}
```

## 4. Security & Authorization
- **Internal Access**: Most endpoints are for internal system use or read-only monitoring for the file's owner.
- **Rules**:
    - Users can only view jobs for files they have `View` permission on (checked via ObjectStorage ownership).

## 5. Integration with YARP
- Match: `/api/processing/{**catch-all}`
- Destination: `http://localhost:5000` (In-process host).
