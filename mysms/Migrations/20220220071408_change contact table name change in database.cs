using Microsoft.EntityFrameworkCore.Migrations;

namespace mysms.Migrations
{
    public partial class changecontacttablenamechangeindatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_contacts",
                table: "contacts");

            migrationBuilder.RenameTable(
                name: "contacts",
                newName: "objects");

            migrationBuilder.RenameIndex(
                name: "IX_contacts_PhoneNo",
                table: "objects",
                newName: "IX_objects_PhoneNo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_objects",
                table: "objects",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_objects",
                table: "objects");

            migrationBuilder.RenameTable(
                name: "objects",
                newName: "contacts");

            migrationBuilder.RenameIndex(
                name: "IX_objects_PhoneNo",
                table: "contacts",
                newName: "IX_contacts_PhoneNo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contacts",
                table: "contacts",
                column: "Id");
        }
    }
}
