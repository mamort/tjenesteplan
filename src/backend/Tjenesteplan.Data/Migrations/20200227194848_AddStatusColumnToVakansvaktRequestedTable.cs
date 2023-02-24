using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class AddStatusColumnToVakansvaktRequestedTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "VakansvaktRequests");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "VakansvaktRequests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "VakansvaktRequests");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "VakansvaktRequests",
                nullable: false,
                defaultValue: false);
        }
    }
}
