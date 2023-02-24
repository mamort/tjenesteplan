using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class AddVaktChangeRequestsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tjenesteplaner_Users_UserId",
                table: "Tjenesteplaner");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Tjenesteplaner",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "VaktChangeRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequestRegisteredDate = table.Column<DateTime>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TjenesteplanId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaktChangeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VaktChangeRequests_Tjenesteplaner_TjenesteplanId",
                        column: x => x.TjenesteplanId,
                        principalTable: "Tjenesteplaner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VaktChangeRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VaktChangeRequests_TjenesteplanId",
                table: "VaktChangeRequests",
                column: "TjenesteplanId");

            migrationBuilder.CreateIndex(
                name: "IX_VaktChangeRequests_UserId",
                table: "VaktChangeRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tjenesteplaner_Users_UserId",
                table: "Tjenesteplaner",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tjenesteplaner_Users_UserId",
                table: "Tjenesteplaner");

            migrationBuilder.DropTable(
                name: "VaktChangeRequests");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Tjenesteplaner",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tjenesteplaner_Users_UserId",
                table: "Tjenesteplaner",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
