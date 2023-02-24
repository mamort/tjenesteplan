using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class RemoveTjenesteplanUserRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tjenesteplaner_Users_UserId",
                table: "Tjenesteplaner");

            migrationBuilder.DropIndex(
                name: "IX_Tjenesteplaner_UserId",
                table: "Tjenesteplaner");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tjenesteplaner");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Tjenesteplaner",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tjenesteplaner_UserId",
                table: "Tjenesteplaner",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tjenesteplaner_Users_UserId",
                table: "Tjenesteplaner",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
