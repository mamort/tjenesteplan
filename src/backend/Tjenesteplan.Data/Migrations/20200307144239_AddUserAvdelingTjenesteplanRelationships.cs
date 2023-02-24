using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class AddUserAvdelingTjenesteplanRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvdelingId",
                table: "Tjenesteplaner",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserAvdelinger",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    AvdelingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAvdelinger", x => new { x.UserId, x.AvdelingId });
                    table.ForeignKey(
                        name: "FK_UserAvdelinger_Avdelinger_AvdelingId",
                        column: x => x.AvdelingId,
                        principalTable: "Avdelinger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAvdelinger_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tjenesteplaner_AvdelingId",
                table: "Tjenesteplaner",
                column: "AvdelingId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAvdelinger_AvdelingId",
                table: "UserAvdelinger",
                column: "AvdelingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tjenesteplaner_Avdelinger_AvdelingId",
                table: "Tjenesteplaner",
                column: "AvdelingId",
                principalTable: "Avdelinger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tjenesteplaner_Avdelinger_AvdelingId",
                table: "Tjenesteplaner");

            migrationBuilder.DropTable(
                name: "UserAvdelinger");

            migrationBuilder.DropIndex(
                name: "IX_Tjenesteplaner_AvdelingId",
                table: "Tjenesteplaner");

            migrationBuilder.DropColumn(
                name: "AvdelingId",
                table: "Tjenesteplaner");
        }
    }
}
