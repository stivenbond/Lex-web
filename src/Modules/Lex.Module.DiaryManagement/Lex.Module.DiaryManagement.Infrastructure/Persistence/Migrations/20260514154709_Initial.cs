using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lex.Module.DiaryManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "diary");

            migrationBuilder.CreateTable(
                name: "diary_entries",
                schema: "diary",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_diary_entries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "diary_attachments",
                schema: "diary",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    diary_entry_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_diary_attachments", x => x.id);
                    table.ForeignKey(
                        name: "f_k_diary_attachments_diary_entries_diary_entry_id",
                        column: x => x.diary_entry_id,
                        principalSchema: "diary",
                        principalTable: "diary_entries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_diary_attachments_diary_entry_id",
                schema: "diary",
                table: "diary_attachments",
                column: "diary_entry_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "diary_attachments",
                schema: "diary");

            migrationBuilder.DropTable(
                name: "diary_entries",
                schema: "diary");
        }
    }
}
