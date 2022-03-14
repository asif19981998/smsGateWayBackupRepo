using Microsoft.EntityFrameworkCore.Migrations;

namespace TcpClientrReader.Migrations
{
    public partial class smsinboxtablenamechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_smsInboxes",
                table: "smsInboxes");

            migrationBuilder.RenameTable(
                name: "smsInboxes",
                newName: "smsInbox");

            migrationBuilder.AddPrimaryKey(
                name: "PK_smsInbox",
                table: "smsInbox",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_smsInbox",
                table: "smsInbox");

            migrationBuilder.RenameTable(
                name: "smsInbox",
                newName: "smsInboxes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_smsInboxes",
                table: "smsInboxes",
                column: "ID");
        }
    }
}
