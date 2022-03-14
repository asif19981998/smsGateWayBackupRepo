using Microsoft.EntityFrameworkCore.Migrations;

namespace mysms.Migrations
{
    public partial class changecontacttablenamechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Close",
                table: "contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Watch",
                table: "contacts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Close",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "Watch",
                table: "contacts");
        }
    }
}
