using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class AddUserTjenesteplanerRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tjenesteplaner_TjenesteplanId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TjenesteplanId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TjenesteplanId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserTjenesteplan",
                columns: table => new
                {
                    TjenesteplanId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTjenesteplan", x => new { x.UserId, x.TjenesteplanId });
                    table.ForeignKey(
                        name: "FK_UserTjenesteplan_Tjenesteplaner_TjenesteplanId",
                        column: x => x.TjenesteplanId,
                        principalTable: "Tjenesteplaner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTjenesteplan_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTjenesteplan_TjenesteplanId",
                table: "UserTjenesteplan",
                column: "TjenesteplanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTjenesteplan");

            migrationBuilder.AddColumn<int>(
                name: "TjenesteplanId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TjenesteplanId",
                table: "Users",
                column: "TjenesteplanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tjenesteplaner_TjenesteplanId",
                table: "Users",
                column: "TjenesteplanId",
                principalTable: "Tjenesteplaner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
