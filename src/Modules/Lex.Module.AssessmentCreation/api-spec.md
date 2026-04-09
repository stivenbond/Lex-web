# API Specification: AssessmentCreation Module

## 1. Overview
- **Parent Module**: AssessmentCreation
- **Base Route**: `/api/assessments`
- **Purpose**: Manage the full lifecycle of assessment authoring, from initial draft to final snapshot publication.

## 2. API Endpoints

### Assessment Management
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/` | `CreateAssessment` | Initialize a new draft assessment. |
| `GET` | `/{id}` | `GetAssessmentById` | Fetch the full assessment structure. |
| `PATCH` | `/{id}/reorder` | `ReorderContent` | Update order of sections and questions. |
| `POST` | `/{id}/publish` | `Publish` | Finalize and create an immutable snapshot. |

### Question Authoring
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/{id}/sections/{sId}/questions` | `AddQuestion` | Add a new question to a section. |
| `PUT` | `/{id}/questions/{qId}` | `UpdateQuestion` | Modify question content and scoring. |
| `DELETE` | `/{id}/questions/{qId}` | `RemoveQuestion` | Delete a question from the draft. |

### Question Bank
| Method | Route | Command/Query | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/bank` | `GetQuestionBank` | Search reusable questions. |
| `POST` | `/bank` | `AddBankQuestion` | Manually add a question to the shared bank. |

## 3. Request/Response Shapes

### Resource: AssessmentDetailsDto
```json
{
  "id": "guid",
  "title": "string",
  "status": "Draft | Published | Archived",
  "sections": [
    {
      "id": "guid",
      "title": "string",
      "questions": [
        {
          "id": "guid",
          "type": "MCQ",
          "points": 10,
          "content": "...",
          "specifics": { ... }
        }
      ]
    }
  ],
  "totalPoints": 50
}
```

## 4. Security & Authorization
- **Policies**: 
    - `TeacherOnly` for all management endpoints.
- **Ownership**: The module service will verify that the requesting user is the author of the assessment before allowing mutations.

## 5. Integration with YARP
- Match: `/api/assessments/{**catch-all}`
- Destination: `http://localhost:5000` (In-process host).
