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
                name: "CarnotSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    User = table.Column<string>(type: "TEXT", nullable: false),
                    ApiKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarnotSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElOverblikSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    MeteringPointId = table.Column<string>(type: "TEXT", nullable: false),
                    ElectricHeatingMeteringPointId = table.Column<string>(type: "TEXT", nullable: false),
                    ApiToken = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElOverblikSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnergiDataServiceSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergiDataServiceSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Region = table.Column<string>(type: "TEXT", nullable: false),
                    HasElectricHeating = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeAssistantSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeAssistantSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NordPoolSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NordPoolSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    HasElectricHeating = table.Column<bool>(type: "INTEGER", nullable: false),
                    Start = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    End = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FixedFees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    FeePeriodId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixedFees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedFees_Periods_FeePeriodId",
                        column: x => x.FeePeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HourlyFees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Start = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    FeePeriodId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourlyFees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HourlyFees_Periods_FeePeriodId",
                        column: x => x.FeePeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Hour = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Region = table.Column<string>(type: "TEXT", nullable: false),
                    Source = table.Column<string>(type: "TEXT", nullable: false),
                    RawPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsPrediction = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    FeePeriodId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_Periods_FeePeriodId",
                        column: x => x.FeePeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Usages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Hour = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    EntityId = table.Column<string>(type: "TEXT", nullable: false),
                    Source = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    FeePeriodId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usages_Periods_FeePeriodId",
                        column: x => x.FeePeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeesPerUnit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    AmountPerUnit = table.Column<decimal>(type: "TEXT", nullable: false),
                    Application = table.Column<int>(type: "INTEGER", nullable: false),
                    FeePerUnitId = table.Column<long>(type: "INTEGER", nullable: false),
                    HourlyFeePeriodId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeesPerUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeesPerUnit_HourlyFees_HourlyFeePeriodId",
                        column: x => x.HourlyFeePeriodId,
                        principalTable: "HourlyFees",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "CarnotSettings",
                columns: new[] { "Id", "ApiKey", "IsEnabled", "User" },
                values: new object[] { 1L, "", false, "" });

            migrationBuilder.InsertData(
                table: "ElOverblikSettings",
                columns: new[] { "Id", "ApiToken", "ElectricHeatingMeteringPointId", "IsEnabled", "MeteringPointId" },
                values: new object[] { 1L, "", "", false, "" });

            migrationBuilder.InsertData(
                table: "EnergiDataServiceSettings",
                columns: new[] { "Id", "IsEnabled" },
                values: new object[] { 1L, true });

            migrationBuilder.InsertData(
                table: "GeneralSettings",
                columns: new[] { "Id", "HasElectricHeating", "Region" },
                values: new object[] { 1L, false, "dk2" });

            migrationBuilder.InsertData(
                table: "HomeAssistantSettings",
                columns: new[] { "Id", "IsEnabled", "Token", "Url" },
                values: new object[] { 1L, false, "", "http://supervisor/core" });

            migrationBuilder.InsertData(
                table: "NordPoolSettings",
                columns: new[] { "Id", "IsEnabled" },
                values: new object[] { 1L, false });

            migrationBuilder.CreateIndex(
                name: "IX_FeesPerUnit_HourlyFeePeriodId",
                table: "FeesPerUnit",
                column: "HourlyFeePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedFees_FeePeriodId",
                table: "FixedFees",
                column: "FeePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_HourlyFees_FeePeriodId",
                table: "HourlyFees",
                column: "FeePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_FeePeriodId",
                table: "Prices",
                column: "FeePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_Hour",
                table: "Prices",
                column: "Hour");

            migrationBuilder.CreateIndex(
                name: "IX_Usages_FeePeriodId",
                table: "Usages",
                column: "FeePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Usages_Hour",
                table: "Usages",
                column: "Hour");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarnotSettings");

            migrationBuilder.DropTable(
                name: "ElOverblikSettings");

            migrationBuilder.DropTable(
                name: "EnergiDataServiceSettings");

            migrationBuilder.DropTable(
                name: "FeesPerUnit");

            migrationBuilder.DropTable(
                name: "FixedFees");

            migrationBuilder.DropTable(
                name: "GeneralSettings");

            migrationBuilder.DropTable(
                name: "HomeAssistantSettings");

            migrationBuilder.DropTable(
                name: "NordPoolSettings");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Usages");

            migrationBuilder.DropTable(
                name: "HourlyFees");

            migrationBuilder.DropTable(
                name: "Periods");
        }
    }
}
