using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class AddVaktChangeRequestDagsplanColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Dagsplan",
                table: "VaktChangeRequests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dagsplan",
                table: "VaktChangeRequests");
        }
    }
}
