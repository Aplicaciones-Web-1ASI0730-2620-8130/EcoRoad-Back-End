using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialAssets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "monitoring_points",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    project_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    section_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    kilometer_post = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    latitude = table.Column<double>(type: "double", nullable: false),
                    longitude = table.Column<double>(type: "double", nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monitoring_points", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sensors",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    serial_number = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    model = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    monitoring_point_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true),
                    project_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true),
                    section_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true),
                    calibrated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    calibration_valid_until = table.Column<DateTime>(type: "datetime", nullable: true),
                    activated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sensors", x => x.id);
                    table.ForeignKey(
                        name: "FK_sensors_monitoring_points_monitoring_point_id",
                        column: x => x.monitoring_point_id,
                        principalTable: "monitoring_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sensor_assignments",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    sensor_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    monitoring_point_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    assigned_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    released_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sensor_assignments", x => x.id);
                    table.ForeignKey(
                        name: "FK_sensor_assignments_monitoring_points_monitoring_point_id",
                        column: x => x.monitoring_point_id,
                        principalTable: "monitoring_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sensor_assignments_sensors_sensor_id",
                        column: x => x.sensor_id,
                        principalTable: "sensors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_monitoring_points_code",
                table: "monitoring_points",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_monitoring_points_project_id",
                table: "monitoring_points",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_monitoring_points_section_id_name",
                table: "monitoring_points",
                columns: new[] { "section_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sensor_assignments_monitoring_point_id",
                table: "sensor_assignments",
                column: "monitoring_point_id");

            migrationBuilder.CreateIndex(
                name: "IX_sensor_assignments_sensor_id",
                table: "sensor_assignments",
                column: "sensor_id");

            migrationBuilder.CreateIndex(
                name: "IX_sensors_code",
                table: "sensors",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sensors_monitoring_point_id",
                table: "sensors",
                column: "monitoring_point_id");

            migrationBuilder.CreateIndex(
                name: "IX_sensors_project_id",
                table: "sensors",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_sensors_serial_number",
                table: "sensors",
                column: "serial_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sensors_status",
                table: "sensors",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sensor_assignments");

            migrationBuilder.DropTable(
                name: "sensors");

            migrationBuilder.DropTable(
                name: "monitoring_points");
        }
    }
}
