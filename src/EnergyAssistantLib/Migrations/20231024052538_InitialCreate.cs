using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UlfenDk.EnergyAssistant.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    SpotPriceRecordId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Hour = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    RawPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    RegularPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    ReducedPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsPrediction = table.Column<bool>(type: "INTEGER", nullable: false),
                    Region = table.Column<string>(type: "TEXT", nullable: false),
                    Source = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.SpotPriceRecordId);
                });

            migrationBuilder.CreateTable(
                name: "Usages",
                columns: table => new
                {
                    EnergyUsageRecordId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Hour = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    EntityId = table.Column<string>(type: "TEXT", nullable: true),
                    Source = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usages", x => x.EnergyUsageRecordId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prices_Hour",
                table: "Prices",
                column: "Hour");

            migrationBuilder.CreateIndex(
                name: "IX_Usages_Hour",
                table: "Usages",
                column: "Hour");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Usages");
        }
    }
}
