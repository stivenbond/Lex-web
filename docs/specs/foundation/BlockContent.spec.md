# Domain Specification: BlockContent System

## 1. Context
- **Module**: SharedKernel
- **Scope**: Uniform representation of rich content (Diary, Lessons, Assessments) across the platform.

## 2. Core Ubiquitous Language
- **Content Block**: The atomic unit of content (Text, Image, Link, etc.).
- **Block Model**: The polymorphic structure that allows different modules to consume the same content types.
- **Tiptap Renderer**: The frontend component that converts BlockContent JSON into HTML/React components.

## 3. Domain Model Hierarchy

- **Interface: IContentBlock<TData>**
    - **Purpose**: Generic base for all block types.
    - **Properties**:
        - `Id`: Versioned identifier.
        - `CreatedDate`: Timestamp.
        - `Metadata`: Key-value pairs for customization (e.g., "alignment", "border").
        - `Content`: The specific data model (TData).

- **Entity: TextBlock**
    - **Data**: `TextData` (string, TextStyle).
    - **Style Variants**: Title, H1, H2, H3, Body, Caption.

- **Entity: AttachmentBlock**
    - **Data**: `AttachmentData` (Uri, FileType, Caption).
    - **Role**: References an object in `ObjectStorage`.

- **Entity: LinkBlock**
    - **Data**: `LinkData` (Url, DisplayName).

- **Entity: DiagramBlock (Custom)**
    - **Purpose**: Embedded React Flow diagram data.

## 4. Value Objects
- **TextStyle**: Enum for typography scaling.
- **FileType**: Raster, Vector, Video, Audio classification.
- **DynamicInteractivity**: Engine types for scriptable blocks (DSL, JS).

## 5. Domain Events
N/A (Blocks are usually embedded within Aggregate Roots like `DiaryEntry` or `LessonPlan`).

## 6. Business Operations (Conceptual)
- **Op: Serialization**
    - **Outcome**: Content is stored as JSON in a single PostgreSQL column (typically `jsonb` or `text`) across modules.
- **Op: Versioning Strategy**
    - **Rule**: Block schemas are additive. Removing a field requires a migration or a "deprecated" handling in the renderer.
