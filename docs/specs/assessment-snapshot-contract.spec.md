# Specification: Assessment Snapshot Contract (Handoff)

## 1. Overview
- **Category**: Cross-Module Data Contract
- **Source Module**: AssessmentCreation
- **Consumer Module**: AssessmentDelivery
- **Purpose**: Define the immutable data structure used to deliver an assessment to students.

## 2. Technical Strategy
- **Format**: JSON
- **Persistence**: Stored in `assessment_creation.snapshots` and delivered as a single unit to the delivery module via `AssessmentPublishedEvent`.

## 3. Data Structure

### Root object
```json
{
  "assessmentId": "guid",
  "version": 1,
  "title": "string",
  "publishedAt": "iso-date",
  "totalPoints": 100,
  "sections": [ ... ],
  "hash": "sha256-signature"
}
```

### Section object
```json
{
  "id": "guid",
  "title": "string",
  "order": 1,
  "questions": [ ... ]
}
```

### Question object (Polymorphic)
Each question includes a `type` discriminator and a `specifics` object.
```json
{
  "id": "guid",
  "type": "MCQ | ShortAnswer | Essay | FileUpload",
  "content": "Rich text / HTML / BlockContent",
  "points": 10,
  "order": 1,
  "specifics": {
    /* For MCQ */
    "options": [
      { "id": "guid", "text": "...", "isCorrect": true }
    ],
    /* For ShortAnswer */
    "expectedRegex": "^[0-9]+$",
    "isCaseSensitive": false
  }
}
```

## 4. Integrity & Versioning
- **Calculated Hash**: The `hash` field represents a SHA256 checksum of the serialized `sections` array.
- **Delivery**: The `AssessmentDelivery` module MUST verify the hash before allowing student attempts.
- **Immutability**: Once a snapshot is generated and signed, it cannot be modified. Any correction requires a new `version` (Version 2).

## 5. Implementation Notes
- The `AssessmentCreation` module handles the transformation from its relational schema (`assessment_creation.questions`, etc.) into this flat JSON contract.
- The `AssessmentDelivery` module stores this JSON in its own schema (`delivery.assessments`) to ensure it remains independent of schema changes in the creation module.
