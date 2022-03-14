using Microsoft.EntityFrameworkCore.Migrations;

namespace mysms.Migrations
{
    public partial class groupidaddinobjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "objects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_objects_GroupId",
                table: "objects",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_objects_groups_GroupId",
                table: "objects",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_objects_groups_GroupId",
                table: "objects");

            migrationBuilder.DropIndex(
                name: "IX_objects_GroupId",
                table: "objects");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "objects");
        }
    }
}
