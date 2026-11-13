using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddCompliance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "compliance_reports",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    scope = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    subject_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    requested_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    requested_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    generated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    content_hash = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    digital_signature = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                    monitoring_record_ids = table.Column<string>(type: "json", nullable: false),
                    alert_ids = table.Column<string>(type: "json", nullable: false),
                    incident_ids = table.Column<string>(type: "json", nullable: false),
                    field_evidence_ids = table.Column<string>(type: "json", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compliance_reports", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "environmental_statuses",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    scope = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    subject_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    standing = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_environmental_statuses", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_environmental_statuses_scope_subject_id",
                table: "environmental_statuses",
                columns: new[] { "scope", "subject_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "compliance_reports");

            migrationBuilder.DropTable(
                name: "environmental_statuses");
        }
    }
}
