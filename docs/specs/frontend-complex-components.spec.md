# Specification: Frontend Complex Components

## 1. Overview
- **Category**: Component Design / Advanced Interactive Features
- **Scope**: Implementation details for the three core complex views: Documents, Diagrams, and Presentations.

## 2. Document Editor (`components/documents/`)
- **Library**: Tiptap (Headless ProseMirror).
- **Core Features**:
    - **Formatting**: Bold, Italic, Headings (1-3), Lists.
    - **Custom Blocks**: `DiagramBlock` (Allows embedding a Diagram by reference), `AttachmentBlock`.
    - **Auto-Save**: Hook-based debounced logic (2 seconds) sending the current JSON content to the API.
- **Render State**:
    - `Editable`: Full toolbar and input focus.
    - `Read-Only`: No toolbar, purely for viewing.

## 3. Diagram Canvas (`components/diagrams/`)
- **Library**: React Flow.
- **Core Features**:
    - **Node Types**: `LexNode` (Configurable via domain data).
    - **Interactions**: Drag nodes, connect edges, zoom/pan.
    - **Real-Time Sync**: Pushes node position changes to SignalR (Debounced) so other viewers see the movement.
- **Persistence**: Nodes and Edges are stored in the target module's DB schema (denormalized).

## 4. Presentation Viewer (`components/presentation/`)
- **Library**: React + Framer Motion.
- **Philosophy**: Lightweight, slide-based display of domain data.
- **Core Features**:
    - **Fullscreen Mode**: Using the Browser Fullscreen API.
    - **Navigation**: Keyboard arrows + On-screen progress indicator.
    - **AnimatePresence**: Smooth slide transitions (Cross-fade or Slide).
    - **Slide Types**: Title Slide, Content Block Slide, Diagram Slide (Embeds a read-only DiagramCanvas).

## 5. UI Integration (shadcn/ui)
- These complex components MUST use the design tokens (CSS Variables) from `globals.css` to ensure they match the platform's theme.
- **Icons**: Lucide React.
- **Theming**: Automatic Dark/Light mode switching support.

## 6. Performance
- **Lazy Loading**: Use `next/dynamic` to load Tiptap and React Flow only when the user enters an editor route, keeping the initial bundle size small.
- **Memoization**: Heavy nodes in React Flow are wrapped in `React.memo` to prevent re-renders on every pan event.
