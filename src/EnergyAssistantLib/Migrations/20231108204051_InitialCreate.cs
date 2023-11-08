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
                name: "FeePeriod",
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
                    table.PrimaryKey("PK_FeePeriod", x => x.Id);
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
                name: "MonthPeriods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    From = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    To = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthPeriods", x => x.Id);
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
                name: "YearPeriods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Vat = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FixedFee",
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
                    table.PrimaryKey("PK_FixedFee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedFee_FeePeriod_FeePeriodId",
                        column: x => x.FeePeriodId,
                        principalTable: "FeePeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HourlyFeePeriod",
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
                    table.PrimaryKey("PK_HourlyFeePeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HourlyFeePeriod_FeePeriod_FeePeriodId",
                        column: x => x.FeePeriodId,
                        principalTable: "FeePeriod",
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
                        name: "FK_Prices_FeePeriod_FeePeriodId",
                        column: x => x.FeePeriodId,
                        principalTable: "FeePeriod",
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
                        name: "FK_Usages_FeePeriod_FeePeriodId",
                        column: x => x.FeePeriodId,
                        principalTable: "FeePeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyIntervals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Start = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    MonthPeriodId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyIntervals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyIntervals_MonthPeriods_MonthPeriodId",
                        column: x => x.MonthPeriodId,
                        principalTable: "MonthPeriods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FeePerUnit",
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
                    table.PrimaryKey("PK_FeePerUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeePerUnit_HourlyFeePeriod_HourlyFeePeriodId",
                        column: x => x.HourlyFeePeriodId,
                        principalTable: "HourlyFeePeriod",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Fees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    DailyIntervalId = table.Column<long>(type: "INTEGER", nullable: true),
                    MonthPeriodId = table.Column<long>(type: "INTEGER", nullable: true),
                    MonthPeriodId1 = table.Column<long>(type: "INTEGER", nullable: true),
                    MonthPeriodId2 = table.Column<long>(type: "INTEGER", nullable: true),
                    YearPeriodId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fees_DailyIntervals_DailyIntervalId",
                        column: x => x.DailyIntervalId,
                        principalTable: "DailyIntervals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fees_MonthPeriods_MonthPeriodId",
                        column: x => x.MonthPeriodId,
                        principalTable: "MonthPeriods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fees_MonthPeriods_MonthPeriodId1",
                        column: x => x.MonthPeriodId1,
                        principalTable: "MonthPeriods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fees_MonthPeriods_MonthPeriodId2",
                        column: x => x.MonthPeriodId2,
                        principalTable: "MonthPeriods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fees_YearPeriods_YearPeriodId",
                        column: x => x.YearPeriodId,
                        principalTable: "YearPeriods",
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
                name: "IX_DailyIntervals_MonthPeriodId",
                table: "DailyIntervals",
                column: "MonthPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_FeePerUnit_HourlyFeePeriodId",
                table: "FeePerUnit",
                column: "HourlyFeePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_DailyIntervalId",
                table: "Fees",
                column: "DailyIntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_MonthPeriodId",
                table: "Fees",
                column: "MonthPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_MonthPeriodId1",
                table: "Fees",
                column: "MonthPeriodId1");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_MonthPeriodId2",
                table: "Fees",
                column: "MonthPeriodId2");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_YearPeriodId",
                table: "Fees",
                column: "YearPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedFee_FeePeriodId",
                table: "FixedFee",
                column: "FeePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_HourlyFeePeriod_FeePeriodId",
                table: "HourlyFeePeriod",
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
                name: "FeePerUnit");

            migrationBuilder.DropTable(
                name: "Fees");

            migrationBuilder.DropTable(
                name: "FixedFee");

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
                name: "HourlyFeePeriod");

            migrationBuilder.DropTable(
                name: "DailyIntervals");

            migrationBuilder.DropTable(
                name: "YearPeriods");

            migrationBuilder.DropTable(
                name: "FeePeriod");

            migrationBuilder.DropTable(
                name: "MonthPeriods");
        }
    }
}
