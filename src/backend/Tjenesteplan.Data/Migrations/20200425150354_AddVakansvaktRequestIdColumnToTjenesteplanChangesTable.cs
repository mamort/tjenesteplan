using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class AddVakansvaktRequestIdColumnToTjenesteplanChangesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VakansvaktRequestId",
                table: "TjenesteplanChanges",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VakansvaktRequestId",
                table: "TjenesteplanChanges");
        }
    }
}
