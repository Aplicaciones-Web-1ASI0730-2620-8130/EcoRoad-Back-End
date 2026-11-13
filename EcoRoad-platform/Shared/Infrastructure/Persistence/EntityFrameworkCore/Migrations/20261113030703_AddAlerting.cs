using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddAlerting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alert_rules",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    parameter = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    time_band = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    limit = table.Column<double>(type: "double", nullable: false),
                    critical_excess = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alert_rules", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "alerts",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    work_front_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    alert_rule_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    parameter = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    time_band = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    measured_value = table.Column<double>(type: "double", nullable: false),
                    limit_value = table.Column<double>(type: "double", nullable: false),
                    severity = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    raised_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    acknowledged_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    acknowledged_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alerts", x => x.id);
                    table.ForeignKey(
                        name: "FK_alerts_alert_rules_alert_rule_id",
                        column: x => x.alert_rule_id,
                        principalTable: "alert_rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_alert_rules_parameter_time_band",
                table: "alert_rules",
                columns: new[] { "parameter", "time_band" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_alerts_alert_rule_id",
                table: "alerts",
                column: "alert_rule_id");

            migrationBuilder.CreateIndex(
                name: "IX_alerts_status",
                table: "alerts",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_alerts_work_front_id",
                table: "alerts",
                column: "work_front_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alerts");

            migrationBuilder.DropTable(
                name: "alert_rules");
        }
    }
}
