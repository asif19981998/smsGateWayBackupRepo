using Microsoft.EntityFrameworkCore.Migrations;

namespace mysms.Migrations
{
    public partial class removeonpropertiesfromobjectsmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "objects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                table: "objects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
