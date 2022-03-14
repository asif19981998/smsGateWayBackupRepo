using Microsoft.EntityFrameworkCore.Migrations;

namespace mysms.Migrations
{
    public partial class changeincontactform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contacts_groups_GroupId",
                table: "contacts");

            migrationBuilder.DropIndex(
                name: "IX_contacts_GroupId",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "contacts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "contacts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_contacts_GroupId",
                table: "contacts",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_contacts_groups_GroupId",
                table: "contacts",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
