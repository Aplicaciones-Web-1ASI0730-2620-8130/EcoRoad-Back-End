using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCommercial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "company_accounts",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    code = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    segment = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_accounts", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "subscriptions",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    company_account_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    plan = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    billing_cycle = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    selected_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    activated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    current_period_end = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscriptions", x => x.id);
                    table.ForeignKey(
                        name: "FK_subscriptions_company_accounts_company_account_id",
                        column: x => x.company_account_id,
                        principalTable: "company_accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_company_accounts_code",
                table: "company_accounts",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_subscriptions_company_account_id",
                table: "subscriptions",
                column: "company_account_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "subscriptions");

            migrationBuilder.DropTable(
                name: "company_accounts");
        }
    }
}
