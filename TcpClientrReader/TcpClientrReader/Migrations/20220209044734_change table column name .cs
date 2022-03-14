using Microsoft.EntityFrameworkCore.Migrations;

namespace TcpClientrReader.Migrations
{
    public partial class changetablecolumnname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Index",
                table: "smsInboxes",
                newName: "SIndex");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SIndex",
                table: "smsInboxes",
                newName: "Index");
        }
    }
}
