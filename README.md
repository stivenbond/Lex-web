# Lex — Comprehensive Learning Management System

![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![License](https://img.shields.io/badge/license-Proprietary-blue)
![Platform](https://img.shields.io/badge/platform-On--Premise-orange)

## 🎯 Project Overview

**Lex** is an open-source, self-hosted learning management system (LMS) designed for educational institutions. It provides a complete platform for managing curriculum delivery, student assessments, and institutional reporting—all deployable on a single server with no cloud dependencies.

Built by educators and engineers, Lex respects institutional autonomy: your data stays on your servers.

## 🚀 Key Features

### For Teachers & Educators

- **📚 Comprehensive Curriculum Management**
  - Create and organize lesson plans with rich content (text, images, embedded diagrams)
  - Schedule academic periods, terms, and timetables
  - Manage lesson resources and share materials with students
  - Teacher lesson diary with approval workflows and file attachments

- **📝 Rich Assessment Tools**
  - Drag-and-drop assessment builder with multiple question types:
    - Multiple choice (MCQ) with single/multiple correct answers
    - Short-answer questions with regex validation
    - Essays and file upload submissions
  - Question bank for reusable questions across assessments
  - Snapshot publishing for secure assessment delivery
  - Auto-grading for objective questions
  - Manual grading dashboard for subjective responses

- **📊 Submission & Grading**
  - Secure student assessment delivery with proctor controls
  - Real-time submission tracking
  - Automated grading for multiple-choice and short-answer questions
  - Manual grading interface for essays and file uploads
  - Detailed feedback and score reporting

### For Students

- **🎓 Learning Experience**
  - View course schedule and lesson materials
  - Participate in timetabled lessons with real-time notifications
  - Take secure assessments in a distraction-free environment
  - Submit work with file attachments
  - Track progress and view grades

### For Administrators

- **⚙️ Complete System Control**
  - Manage users, roles, and permissions
  - Configure academic calendar (years, terms, periods)
  - System-wide reporting and analytics dashboards
  - Bulk import/export of assessments and data
  - Real-time notifications and alerts

- **🔗 Integration Capabilities**
  - Google Calendar synchronization
  - Google Drive integration for file management
  - Extensible event-driven architecture

### For IT & DevOps

- **☁️ On-Premise Deployment**
  - Single Docker Compose deployment
  - PostgreSQL database (open-source)
  - Self-hosted MinIO for object storage (S3-compatible)
  - No mandatory cloud dependencies
  - Infrastructure as code with Helm charts for Kubernetes

- **📈 Scalability**
  - Modular monolith architecture—extract modules to microservices as needed
  - Event-driven communication between modules
  - Horizontal scaling without code changes
  - Performance monitoring and observability built-in

## 📋 System Architecture

### Technology Stack

**Backend:**
- **.NET 10** — Modern, performant application framework
- **Entity Framework Core** — Object-relational mapping
- **PostgreSQL** — Primary data store
- **MassTransit** — Event-driven communication
- **Keycloak** — Enterprise identity management

**Frontend:**
- **Next.js 15** — React meta-framework with server-side rendering
- **React 19** — Modern UI library
- **Tailwind CSS** — Utility-first styling
- **shadcn/ui** — High-quality component library
- **TanStack Query** — Server state management
- **Zustand** — Client state management

**Infrastructure:**
- **Docker & Docker Compose** — Containerization
- **PostgreSQL** — Relational database
- **MinIO** — Self-hosted S3-compatible object storage
- **Keycloak** — OIDC identity provider
- **SignalR** — Real-time communication

### Module Organization

Lex is built as a **modular monolith** with strictly isolated modules, enforced by architecture tests:

| Module | Responsibility | Key Entities |
|--------|---|---|
| **ObjectStorage** | File management, virus scanning, storage providers | File records, storage metadata |
| **Scheduling** | Academic calendar, timetables, slot assignments | Academic Year, Term, Period, Slot, Class |
| **DiaryManagement** | Teacher lesson diaries with approval workflows | Diary Entry, Attachments, Approval Status |
| **LessonManagement** | Lesson plans and learning resources | Lesson Plan, Lesson Resource, Subject Reference |
| **AssessmentCreation** | Assessment authoring and question management | Assessment, Question, Section, Question Bank |
| **AssessmentDelivery** | Student-facing assessment delivery and grading | Delivery Session, Submission, Submission Answer |
| **FileProcessing** | Background file processing (e.g., PDF text extraction) | Processing Job, Processing Status |
| **Notifications** | In-app and email notifications for all events | Notification, Notification Template, Delivery Status |
| **Reporting** | Cross-module dashboards and analytics | Read models, aggregated reports |
| **GoogleIntegration** | Calendar and Drive synchronization | Sync tasks, access tokens, mappings |
| **ImportExport** | Bulk import/export of assessments and data | Import job, export job, batch operations |

## 📦 Self-Hosting Guide

### For Non-Technical Administrators

**Why Self-Host?**
- ✅ Complete data ownership and privacy
- ✅ Compliant with GDPR, FERPA, and local regulations
- ✅ No reliance on external SaaS providers
- ✅ Customizable to institutional needs
- ✅ Zero per-user licensing fees

**Infrastructure Requirements**
- Single server with 4+ CPU cores, 8+ GB RAM
- 100+ GB storage for files and database
- Linux or Windows Server
- Internet connectivity (for initial setup and Google integration)

**Installation (One Command)**
```bash
# Download and run the bootstrap script
curl -fsSL https://lex.example.com/install.sh | bash

# Or download and run locally
./install.sh
```

This automatically:
1. Validates system requirements
2. Sets up Docker and Docker Compose
3. Configures PostgreSQL and MinIO
4. Initializes Keycloak with default users
5. Deploys the Lex application
6. Runs database migrations

**Post-Installation**
- Access admin panel: `https://your-server/admin`
- Default admin credentials: `admin@lex.local` / `AdminPassword123!` (change immediately)
- Configure SMTP for email notifications
- Customize branding and logo
- Set up SSL certificates (Let's Encrypt recommended)

**Ongoing Maintenance**
- Automated daily backups (configurable)
- System health dashboard with alerts
- One-click updates with zero downtime
- Log aggregation and analysis tools
- Database query optimization recommendations

**Security Features**
- Role-based access control (RBAC)
- Field-level encryption for sensitive data
- Audit logs for all user actions
- Rate limiting and DDoS protection
- Regular security updates via auto-patching

**Support & Documentation**
- Community forum: `https://community.lex.edu`
- Documentation: `https://docs.lex.edu`
- Self-hosted troubleshooting guide included
- Video tutorials for common tasks

## 🔐 User Roles & Permissions

Lex defines four primary roles with specific capabilities:

| Role | Responsibilities | Access Level |
|------|---|---|
| **App Admin** | System administration, user management, system configuration | Full system access |
| **Institution Admin** | Institutional settings, bulk user management, reporting | Institution-wide access |
| **Teacher** | Create lessons, assessments, manage diary, submit grades | Own content + assigned classes |
| **Student** | View schedule, submit assessments, view grades | Own submissions only |

See [AUTHENTICATION.md](./docs/AUTHENTICATION.md) for detailed permission matrix and role definitions.

## 🚀 Getting Started

### Developers

**Prerequisites:**
- .NET 10 SDK
- Node.js 20+
- Docker & Docker Compose
- PostgreSQL 15+

**Setup:**
```bash
# Clone repository
git clone https://github.com/your-org/lex.git
cd lex

# Backend
dotnet build
dotnet ef database update

# Frontend
cd client
npm install
npm run dev

# Start infrastructure
docker-compose up -d

# Run application
dotnet run --project src/Host/Lex.API
```

**Frontend:**
```bash
cd client
npm run dev    # Development mode
npm run build  # Production build
npm run test   # Run tests
```

### Administrators

See [SELF-HOSTING.md](./docs/SELF-HOSTING.md) for comprehensive self-hosting setup and maintenance guide.

## 📚 Documentation

- **[Architecture](./docs/architecture.md)** — System design and principles
- **[Authentication & Authorization](./docs/AUTHENTICATION.md)** — Roles, permissions, and access control
- **[Self-Hosting Guide](./docs/SELF-HOSTING.md)** — Deployment and operations
- **[API Documentation](./docs/API.md)** — REST API reference
- **[Frontend Routes](./docs/FRONTEND-ROUTES.md)** — UI navigation and components
- **[Deployment](./docs/DEPLOYMENT.md)** — Docker, Kubernetes, and update procedures
- **[ADRs](./docs/adr/)** — Architectural decision records
- **[Specifications](./docs/specs/)** — Detailed feature specifications

## 🏗️ Implementation Status

### Phase 0: Foundation ✅
- [x] SharedKernel with domain abstractions
- [x] Authentication & authorization framework
- [x] Architecture tests (enforce boundaries)
- [x] Event-driven infrastructure

### Phase 1-3: Core Modules (Spec Complete, Implementation In Progress)
- [x] Scheduling module (database migrations pending)
- [x] DiaryManagement module (database migrations pending)
- [x] LessonManagement module (database migrations pending)

### Phase 4-5: Assessment Modules (Spec Complete, Implementation In Progress)
- [x] AssessmentCreation module (database migrations pending)
- [x] AssessmentDelivery module (database migrations pending)

### Phase 6-9: Supporting Modules
- [x] FileProcessing, Notifications, Reporting, Google Integration, Import/Export

### Pending Implementation
- Database schema migrations for all modules
- API controller endpoints
- Frontend UI components
- Integration test suite
- End-to-end testing

## 🤝 Contributing

We welcome contributions! See [CONTRIBUTING.md](./CONTRIBUTING.md) for guidelines.

## 📄 License

[Specify your license here]

## 💬 Support

- **Community**: [Discord Server Link]
- **Issues**: [GitHub Issues](https://github.com/your-org/lex/issues)
- **Email**: support@lex.edu

---

**Built with ❤️ for educators by engineers**

Last Updated: April 2026
