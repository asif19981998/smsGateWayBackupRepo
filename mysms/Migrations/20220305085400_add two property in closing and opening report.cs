using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mysms.Migrations
{
    public partial class addtwopropertyinclosingandopeningreport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ArcStopTime",
                table: "dailyReportOpening",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SmsStopTime",
                table: "dailyReportOpening",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ArcStopTime",
                table: "dailyReportClosing",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SmsStopTime",
                table: "dailyReportClosing",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArcStopTime",
                table: "dailyReportOpening");

            migrationBuilder.DropColumn(
                name: "SmsStopTime",
                table: "dailyReportOpening");

            migrationBuilder.DropColumn(
                name: "ArcStopTime",
                table: "dailyReportClosing");

            migrationBuilder.DropColumn(
                name: "SmsStopTime",
                table: "dailyReportClosing");
        }
    }
}
