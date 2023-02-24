using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class AddUserIdToTjenesteplanWeeksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TjenesteplanUker",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TjenesteplanUker_UserId",
                table: "TjenesteplanUker",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TjenesteplanUker_Users_UserId",
                table: "TjenesteplanUker",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TjenesteplanUker_Users_UserId",
                table: "TjenesteplanUker");

            migrationBuilder.DropIndex(
                name: "IX_TjenesteplanUker_UserId",
                table: "TjenesteplanUker");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TjenesteplanUker");
        }
    }
}
