using Microsoft.EntityFrameworkCore.Migrations;

namespace mysms.Migrations
{
    public partial class make_contact_phoneno_unique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNo",
                table: "contacts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_contacts_PhoneNo",
                table: "contacts",
                column: "PhoneNo",
                unique: true,
                filter: "[PhoneNo] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_contacts_PhoneNo",
                table: "contacts");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNo",
                table: "contacts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
