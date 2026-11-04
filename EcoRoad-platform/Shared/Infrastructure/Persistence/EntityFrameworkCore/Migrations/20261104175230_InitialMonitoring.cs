using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialMonitoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "threshold_profiles",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    parameter_type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    kind = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    warning_threshold = table.Column<double>(type: "double", nullable: true),
                    critical_threshold = table.Column<double>(type: "double", nullable: true),
                    minimum = table.Column<double>(type: "double", nullable: true),
                    optimal_minimum = table.Column<double>(type: "double", nullable: true),
                    optimal_maximum = table.Column<double>(type: "double", nullable: true),
                    maximum = table.Column<double>(type: "double", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_threshold_profiles", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "environmental_measurements",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    project_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    section_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    monitoring_point_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true),
                    sensor_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true),
                    recorded_by = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true),
                    parameter_type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    value = table.Column<double>(type: "double", nullable: false),
                    unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    source = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    recorded_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    threshold_profile_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_environmental_measurements", x => x.id);
                    table.ForeignKey(
                        name: "FK_environmental_measurements_threshold_profiles_threshold_profile_id",
                        column: x => x.threshold_profile_id,
                        principalTable: "threshold_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_environmental_measurements_project_id_parameter_type_recorded_at",
                table: "environmental_measurements",
                columns: new[] { "project_id", "parameter_type", "recorded_at" });

            migrationBuilder.CreateIndex(
                name: "IX_environmental_measurements_sensor_id",
                table: "environmental_measurements",
                column: "sensor_id");

            migrationBuilder.CreateIndex(
                name: "IX_environmental_measurements_threshold_profile_id",
                table: "environmental_measurements",
                column: "threshold_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_threshold_profiles_parameter_type",
                table: "threshold_profiles",
                column: "parameter_type",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "environmental_measurements");

            migrationBuilder.DropTable(
                name: "threshold_profiles");
        }
    }
}
