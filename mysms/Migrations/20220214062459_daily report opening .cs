using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mysms.Migrations
{
    public partial class dailyreportopening : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dailyReportClosing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ARC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SMS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dailyReportClosing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dailyReportOpening",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ARC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SMS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dailyReportOpening", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dailyReportClosing");

            migrationBuilder.DropTable(
                name: "dailyReportOpening");
        }
    }
}
