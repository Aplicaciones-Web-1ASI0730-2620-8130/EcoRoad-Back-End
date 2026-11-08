using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AlertingAndRiskEvaluation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alerts");

            migrationBuilder.DropTable(
                name: "alert_rules");

            migrationBuilder.CreateTable(
                name: "deviation_assessments",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    assessed_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    is_critical = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    measured_value = table.Column<double>(type: "double", nullable: false),
                    measurement_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    threshold_value = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deviation_assessments", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "alerts",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    acknowledged_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    acknowledged_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    assessment_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    message = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    monitoring_point_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    raised_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    severity = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alerts", x => x.id);
                    table.ForeignKey(
                        name: "FK_alerts_deviation_assessments_assessment_id",
                        column: x => x.assessment_id,
                        principalTable: "deviation_assessments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_alerts_assessment_id",
                table: "alerts",
                column: "assessment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_alerts_monitoring_point_id",
                table: "alerts",
                column: "monitoring_point_id");

            migrationBuilder.CreateIndex(
                name: "IX_alerts_status",
                table: "alerts",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_deviation_assessments_measurement_id",
                table: "deviation_assessments",
                column: "measurement_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alerts");

            migrationBuilder.DropTable(
                name: "deviation_assessments");

            migrationBuilder.CreateTable(
                name: "alert_rules",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    critical_excess = table.Column<double>(type: "double", nullable: false),
                    limit = table.Column<double>(type: "double", nullable: false),
                    parameter = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    time_band = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
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
                    acknowledged_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    acknowledged_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    alert_rule_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    limit_value = table.Column<double>(type: "double", nullable: false),
                    measured_value = table.Column<double>(type: "double", nullable: false),
                    parameter = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    raised_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    severity = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    time_band = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    work_front_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
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
    }
}
