using Microsoft.EntityFrameworkCore.Migrations;

namespace TcpClientrReader.Migrations
{
    public partial class sidnamechangeaddinsmsinbox : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SId",
                table: "smsInboxes",
                newName: "SmsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SmsId",
                table: "smsInboxes",
                newName: "SId");
        }
    }
}
