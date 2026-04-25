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
                    SUBTOTAL = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    DISCOUNT = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    TOTAL = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "ORDER_PRODUCT",
                columns: table => new
                {
                    ORDER_ID = table.Column<int>(type: "integer", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "integer", nullable: false),
                    PRICE = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER_PRODUCT", x => new { x.ORDER_ID, x.PRODUCT_ID });
                    table.ForeignKey(
                        name: "FK_ORDER_PRODUCT_ORDER_ORDER_ID",
                        column: x => x.ORDER_ID,
                        principalTable: "ORDER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ORDER_PRODUCT_PRODUCT_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
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
                    { 5, new DateTime(2026, 4, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Refrigerante", 2.5m, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_PRODUCT_PRODUCT_ID",
                table: "ORDER_PRODUCT",
                column: "PRODUCT_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ORDER_PRODUCT");

            migrationBuilder.DropTable(
                name: "ORDER");

            migrationBuilder.DropTable(
                name: "PRODUCT");
        }
    }
}
