using Microsoft.EntityFrameworkCore.Migrations;

namespace TcpClientrReader.Migrations
{
    public partial class addsmsinboxrawtablenamechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_smsInbox_Raws",
                table: "smsInbox_Raws");

            migrationBuilder.RenameTable(
                name: "smsInbox_Raws",
                newName: "smsInboxRaw");

            migrationBuilder.AddPrimaryKey(
                name: "PK_smsInboxRaw",
                table: "smsInboxRaw",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_smsInboxRaw",
                table: "smsInboxRaw");

            migrationBuilder.RenameTable(
                name: "smsInboxRaw",
                newName: "smsInbox_Raws");

            migrationBuilder.AddPrimaryKey(
                name: "PK_smsInbox_Raws",
                table: "smsInbox_Raws",
                column: "ID");
        }
    }
}
