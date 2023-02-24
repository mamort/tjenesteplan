using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class AddAndChangeDagsplanColumnsToVakansvaktRequestedTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dagsplan",
                table: "VakansvaktRequests");

            migrationBuilder.AddColumn<int>(
                name: "CurrentDagsplan",
                table: "VakansvaktRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequestedDagsplan",
                table: "VakansvaktRequests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentDagsplan",
                table: "VakansvaktRequests");

            migrationBuilder.DropColumn(
                name: "RequestedDagsplan",
                table: "VakansvaktRequests");

            migrationBuilder.AddColumn<int>(
                name: "Dagsplan",
                table: "VakansvaktRequests",
                nullable: false,
                defaultValue: 0);
        }
    }
}
