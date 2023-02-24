using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class AddChosenVaktChangeAlternativeColumnToVaktChangeRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VaktChangeChosenAlternativeId",
                table: "VaktChangeRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VaktChangeRequests_VaktChangeChosenAlternativeId",
                table: "VaktChangeRequests",
                column: "VaktChangeChosenAlternativeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VaktChangeRequests_VaktChangeAlternatives_VaktChangeChosenAlternativeId",
                table: "VaktChangeRequests",
                column: "VaktChangeChosenAlternativeId",
                principalTable: "VaktChangeAlternatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaktChangeRequests_VaktChangeAlternatives_VaktChangeChosenAlternativeId",
                table: "VaktChangeRequests");

            migrationBuilder.DropIndex(
                name: "IX_VaktChangeRequests_VaktChangeChosenAlternativeId",
                table: "VaktChangeRequests");

            migrationBuilder.DropColumn(
                name: "VaktChangeChosenAlternativeId",
                table: "VaktChangeRequests");
        }
    }
}
