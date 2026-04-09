# API Specification: AssessmentDelivery Module

## 1. Overview
- **Parent Module**: AssessmentDelivery
- **Base Route**: `/api/delivery`
- **Purpose**: Expose endpoints for students taking assessments and teachers monitoring progress and results.

## 2. API Endpoints

### Student Experience (The Player)
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/sessions` | `StartSession` | Start or resume an assessment attempt. |
| `GET` | `/sessions/active/{assessmentId}` | `GetSessionForStudent` | Fetch current progress and assessment structure. |
| `PUT` | `/sessions/{id}/answers/{qId}` | `SaveAnswer` | (Autosave) Record student input for a question. |
| `POST` | `/sessions/{id}/submit` | `SubmitSession` | Finish and submit the exam. |

### Results & Monitoring
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/sessions/{id}/results` | `GetSessionResults` | Fetch scored data and teacher feedback. |
| `GET` | `/sessions/{id}/status` | - | Poll current status (Active, Submitted, Graded). |

### Administration (Proctoring)
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/proctor/pending` | - | List submissions awaiting manual grading. |
| `POST` | `/proctor/{sessionId}/grade` | `GradeSubmission` | (Approve/Score) Manually grade essay questions. |

## 3. Request/Response Shapes

### Resource: SessionProgressDto
```json
{
  "sessionId": "guid",
  "assessmentTitle": "string",
  "snapshot": { "sections": [...] },
  "answers": [
    { "questionId": "guid", "value": { ... } }
  ],
  "status": "Active"
}
```

## 4. Security & Authorization
- **Student Scoping**: All `/sessions/*` endpoints (except GET lists) must verify that the `SessionId` belongs to the `ICurrentUser.UserId`.
- **Integrity**: Answers can only be saved if the session status is `Active`.

## 5. Integration with YARP
- Match: `/api/delivery/{**catch-all}`
- Destination: `http://localhost:5000` (In-process host).
