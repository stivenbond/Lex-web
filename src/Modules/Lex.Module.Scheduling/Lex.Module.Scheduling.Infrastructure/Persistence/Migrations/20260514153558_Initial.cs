using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lex.Module.Scheduling.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "scheduling");

            migrationBuilder.CreateTable(
                name: "academic_years",
                schema: "scheduling",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_academic_years", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "classrooms",
                schema: "scheduling",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    capacity = table.Column<int>(type: "integer", nullable: true, comment: "Number of students the classroom can accommodate"),
                    facilities = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Comma-separated list of facilities (e.g., Projector, Whiteboard)"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_classrooms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sections",
                schema: "scheduling",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    grade = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Academic level/grade (e.g., 9, 10, 11, 12)"),
                    student_count = table.Column<int>(type: "integer", nullable: true, comment: "Estimated or actual student count in this section"),
                    class_teacher_id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "ID or identifier of the class teacher/head teacher"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sections", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "terms",
                schema: "scheduling",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    academic_year_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sequence_number = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_terms", x => x.id);
                    table.ForeignKey(
                        name: "f_k_terms_academic_years_academic_year_id",
                        column: x => x.academic_year_id,
                        principalSchema: "scheduling",
                        principalTable: "academic_years",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "slots",
                schema: "scheduling",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    term_id = table.Column<int>(type: "integer", nullable: false),
                    day_of_week = table.Column<int>(type: "integer", nullable: false, comment: "0=Sunday, 1=Monday, ..., 6=Saturday"),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    duration_minutes = table.Column<int>(type: "integer", nullable: false, comment: "Duration in minutes (e.g., 45 for 45-minute periods)"),
                    slot_number = table.Column<int>(type: "integer", nullable: false, comment: "Sequential slot number within the day (1, 2, 3, etc.)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_slots", x => x.id);
                    table.ForeignKey(
                        name: "f_k_slots_terms_term_id",
                        column: x => x.term_id,
                        principalSchema: "scheduling",
                        principalTable: "terms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "periods",
                schema: "scheduling",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slot_id = table.Column<int>(type: "integer", nullable: false),
                    section_id = table.Column<int>(type: "integer", nullable: false),
                    classroom_id = table.Column<int>(type: "integer", nullable: false),
                    subject = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    teacher_id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "ID of the teacher assigned to teach this period (reference to identity system)"),
                    teacher_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Cached display name of the teacher for performance/readability"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Optional notes (e.g., Lab session, Practical assessment)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_periods", x => x.id);
                    table.ForeignKey(
                        name: "f_k_periods_classrooms_classroom_id",
                        column: x => x.classroom_id,
                        principalSchema: "scheduling",
                        principalTable: "classrooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_periods_sections_section_id",
                        column: x => x.section_id,
                        principalSchema: "scheduling",
                        principalTable: "sections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_periods_slots_slot_id",
                        column: x => x.slot_id,
                        principalSchema: "scheduling",
                        principalTable: "slots",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_academic_years_is_active",
                schema: "scheduling",
                table: "academic_years",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "i_x_academic_years_name",
                schema: "scheduling",
                table: "academic_years",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_academic_years_year",
                schema: "scheduling",
                table: "academic_years",
                column: "year",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_classrooms_is_active",
                schema: "scheduling",
                table: "classrooms",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "i_x_classrooms_name",
                schema: "scheduling",
                table: "classrooms",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_periods_classroom_id",
                schema: "scheduling",
                table: "periods",
                column: "classroom_id");

            migrationBuilder.CreateIndex(
                name: "i_x_periods_is_active",
                schema: "scheduling",
                table: "periods",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "i_x_periods_section_id",
                schema: "scheduling",
                table: "periods",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "i_x_periods_subject",
                schema: "scheduling",
                table: "periods",
                column: "subject");

            migrationBuilder.CreateIndex(
                name: "i_x_periods_teacher_id",
                schema: "scheduling",
                table: "periods",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "ix_unique_period_per_classroom_slot",
                schema: "scheduling",
                table: "periods",
                columns: new[] { "slot_id", "classroom_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_unique_period_per_section_slot",
                schema: "scheduling",
                table: "periods",
                columns: new[] { "slot_id", "section_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_unique_period_per_teacher_slot",
                schema: "scheduling",
                table: "periods",
                columns: new[] { "slot_id", "teacher_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_sections_class_teacher_id",
                schema: "scheduling",
                table: "sections",
                column: "class_teacher_id");

            migrationBuilder.CreateIndex(
                name: "i_x_sections_grade",
                schema: "scheduling",
                table: "sections",
                column: "grade");

            migrationBuilder.CreateIndex(
                name: "i_x_sections_is_active",
                schema: "scheduling",
                table: "sections",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "i_x_sections_name",
                schema: "scheduling",
                table: "sections",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_slots_term_id_slot_number",
                schema: "scheduling",
                table: "slots",
                columns: new[] { "term_id", "slot_number" });

            migrationBuilder.CreateIndex(
                name: "ix_unique_slot_per_day_time",
                schema: "scheduling",
                table: "slots",
                columns: new[] { "term_id", "day_of_week", "start_time" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_terms_academic_year_id_sequence_number",
                schema: "scheduling",
                table: "terms",
                columns: new[] { "academic_year_id", "sequence_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_terms_is_active",
                schema: "scheduling",
                table: "terms",
                column: "is_active");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "periods",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "classrooms",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "sections",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "slots",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "terms",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "academic_years",
                schema: "scheduling");
        }
    }
}
