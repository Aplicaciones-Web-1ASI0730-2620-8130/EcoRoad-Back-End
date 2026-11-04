using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialIncidents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "environmental_incidents",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    company_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    alert_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    project_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    section_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    title = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    severity = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    opened_by = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    opened_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    responsible_user_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true),
                    responsible_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    assigned_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    attended_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    closed_by = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true),
                    closure_notes = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    closed_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_environmental_incidents", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "corrective_actions",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    incident_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    remediation_progress = table.Column<int>(type: "int", nullable: false),
                    assigned_by = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    assigned_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    started_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    executed_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_corrective_actions", x => x.id);
                    table.ForeignKey(
                        name: "FK_corrective_actions_environmental_incidents_incident_id",
                        column: x => x.incident_id,
                        principalTable: "environmental_incidents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "field_evidences",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    incident_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    registered_by_user_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    file_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    notes = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    recorded_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_field_evidences", x => x.id);
                    table.ForeignKey(
                        name: "FK_field_evidences_environmental_incidents_incident_id",
                        column: x => x.incident_id,
                        principalTable: "environmental_incidents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_corrective_actions_incident_id",
                table: "corrective_actions",
                column: "incident_id");

            migrationBuilder.CreateIndex(
                name: "IX_environmental_incidents_alert_id",
                table: "environmental_incidents",
                column: "alert_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_environmental_incidents_code",
                table: "environmental_incidents",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_environmental_incidents_company_id_status",
                table: "environmental_incidents",
                columns: new[] { "company_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_field_evidences_incident_id",
                table: "field_evidences",
                column: "incident_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "corrective_actions");

            migrationBuilder.DropTable(
                name: "field_evidences");

            migrationBuilder.DropTable(
                name: "environmental_incidents");
        }
    }
}
