using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RequestsApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    is_blocked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    id_client = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_requests_clients_id_client",
                        column: x => x.id_client,
                        principalTable: "clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "clients",
                columns: new[] { "Id", "is_blocked", "login", "password", "role" },
                values: new object[,]
                {
                    { 1, false, "master07", "12345", "Пользователь" },
                    { 2, false, "admin", "admin", "Администратор" },
                    { 3, true, "manager", "pass123", "Менеджер" }
                });

            migrationBuilder.InsertData(
                table: "requests",
                columns: new[] { "Id", "content", "created_at", "id_client", "title" },
                values: new object[,]
                {
                    { 1, "iPhone 12", new DateTime(2027, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Замена экрана" },
                    { 2, "Lenovo", new DateTime(2027, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Чистка ноутбука" },
                    { 3, "Windows 11", new DateTime(2027, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Установка ПО" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_requests_id_client",
                table: "requests",
                column: "id_client");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "requests");

            migrationBuilder.DropTable(
                name: "clients");
        }
    }
}
