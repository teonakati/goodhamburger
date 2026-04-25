using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace goodhamburger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ORDER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TOTAL = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    DISCOUNT = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PRICE = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    TYPE = table.Column<int>(type: "integer", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "PRODUCT",
                columns: new[] { "ID", "CREATED_AT", "NAME", "PRICE", "TYPE" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 24, 0, 0, 0, 0, DateTimeKind.Utc), "X-Burguer", 5m, 1 },
                    { 2, new DateTime(2026, 4, 24, 0, 0, 0, 0, DateTimeKind.Utc), "X-Bacon", 7m, 1 },
                    { 3, new DateTime(2026, 4, 24, 0, 0, 0, 0, DateTimeKind.Utc), "X-Egg", 4.5m, 1 },
                    { 4, new DateTime(2026, 4, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Batata Frita", 2m, 2 },
                    { 5, new DateTime(2026, 4, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Refrigerante", 2.5m, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ORDER");

            migrationBuilder.DropTable(
                name: "PRODUCT");
        }
    }
}
