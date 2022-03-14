using Microsoft.EntityFrameworkCore.Migrations;

namespace mysms.Migrations
{
    public partial class addportsettingtableandporttableseeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PortSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Port_1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Port_2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Port_3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Port_4 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PortSettings",
                columns: new[] { "Id", "Port_1", "Port_2", "Port_3", "Port_4" },
                values: new object[] { 1, "Bank 1", "Bank 2", "Bank 3", "Jwl 1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortSettings");
        }
    }
}
