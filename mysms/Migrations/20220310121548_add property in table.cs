using Microsoft.EntityFrameworkCore.Migrations;

namespace mysms.Migrations
{
    public partial class addpropertyintable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNo",
                table: "dailyReportOpening",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNo",
                table: "dailyReportClosing",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNo",
                table: "dailyReportOpening");

            migrationBuilder.DropColumn(
                name: "PhoneNo",
                table: "dailyReportClosing");
        }
    }
}
