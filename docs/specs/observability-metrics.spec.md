# Specification: Lex Platform Business Telemetry

## 1. Overview
- **Category**: Observability
- **Scope**: Definition of domain-specific metrics to be emitted by each module via OpenTelemetry.
- **Convention**: `lex.{module}.{metric_name}`

## 2. Global Infrastructure Metrics
| Metric Name | Type | Description |
| :--- | :--- | :--- |
| `lex.infra.commands_total` | Counter | Total MediatR commands processed. |
| `lex.infra.events_published` | Counter | Total cross-module events published. |
| `lex.infra.db_connection_pool` | Gauge | Active connections to the Postgres instance. |

## 3. Module Specific Metrics

### Diary & Scheduling
- `lex.diary.entries_created`: Counter.
- `lex.diary.conflict_detected`: Counter (Indicates scheduling friction).
- `lex.scheduling.appointments_completed`: Counter.

### Lesson Management
- `lex.lessons.blocks_total`: Gauge (Distribution of block types: text vs media).
- `lex.lessons.published_total`: Counter.
- `lex.lessons.export_docx_duration`: Histogram (Latency of Word generation).

### Assessment System
- `lex.assessments.created_total`: Counter.
- `lex.assessments.avg_score_percentage`: Histogram (Aggregated performance per organization).
- `lex.assessments.grading_wait_time`: Histogram (Time from student submission to grade release).

### ImportExport
- `lex.importexport.jobs_total`: Counter (Tagged by `type: import | export`).
- `lex.importexport.mapping_failure`: Counter (Indicates manual intervention needed on schemas).
- `lex.importexport.job_duration`: Histogram.

## 4. Implementation Details
- **Provider**: `IMeterFactory` from `.NET 10`.
- **Export Target**: Forwarded via OpenTelemetry Collector to the Seq instance.
- **Aggregation**: Gauges and Counters should be tagged by `OrganizationId` where applicable.
