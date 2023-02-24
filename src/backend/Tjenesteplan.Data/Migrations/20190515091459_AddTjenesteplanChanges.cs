using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class AddTjenesteplanChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaktChangeAlternatives_VaktChangeRequestReplies_VaktChangeRequestReplyId",
                table: "VaktChangeAlternatives");

            migrationBuilder.CreateTable(
                name: "TjenesteplanChanges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Dagsplan = table.Column<int>(nullable: false),
                    TjenesteplanId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    VaktChangeRequestId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TjenesteplanChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TjenesteplanChanges_Tjenesteplaner_TjenesteplanId",
                        column: x => x.TjenesteplanId,
                        principalTable: "Tjenesteplaner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TjenesteplanChanges_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TjenesteplanChanges_VaktChangeRequests_VaktChangeRequestId",
                        column: x => x.VaktChangeRequestId,
                        principalTable: "VaktChangeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TjenesteplanChanges_TjenesteplanId",
                table: "TjenesteplanChanges",
                column: "TjenesteplanId");

            migrationBuilder.CreateIndex(
                name: "IX_TjenesteplanChanges_UserId",
                table: "TjenesteplanChanges",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TjenesteplanChanges_VaktChangeRequestId",
                table: "TjenesteplanChanges",
                column: "VaktChangeRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_VaktChangeAlternatives_VaktChangeRequestReplies_VaktChangeRequestReplyId",
                table: "VaktChangeAlternatives",
                column: "VaktChangeRequestReplyId",
                principalTable: "VaktChangeRequestReplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaktChangeAlternatives_VaktChangeRequestReplies_VaktChangeRequestReplyId",
                table: "VaktChangeAlternatives");

            migrationBuilder.DropTable(
                name: "TjenesteplanChanges");

            migrationBuilder.AddForeignKey(
                name: "FK_VaktChangeAlternatives_VaktChangeRequestReplies_VaktChangeRequestReplyId",
                table: "VaktChangeAlternatives",
                column: "VaktChangeRequestReplyId",
                principalTable: "VaktChangeRequestReplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
