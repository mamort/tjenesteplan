using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class AddVaktChangeRequestReplies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaktChangeRequests_Users_UserId",
                table: "VaktChangeRequests");

            migrationBuilder.CreateTable(
                name: "VaktChangeRequestReplies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<int>(nullable: false),
                    NumberOfRemindersSent = table.Column<int>(nullable: false),
                    VaktChangeRequestId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaktChangeRequestReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VaktChangeRequestReplies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VaktChangeRequestReplies_VaktChangeRequests_VaktChangeRequestId",
                        column: x => x.VaktChangeRequestId,
                        principalTable: "VaktChangeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VaktChangeAlternatives",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    VaktChangeRequestReplyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaktChangeAlternatives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VaktChangeAlternatives_VaktChangeRequestReplies_VaktChangeRequestReplyId",
                        column: x => x.VaktChangeRequestReplyId,
                        principalTable: "VaktChangeRequestReplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VaktChangeAlternatives_VaktChangeRequestReplyId",
                table: "VaktChangeAlternatives",
                column: "VaktChangeRequestReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_VaktChangeRequestReplies_UserId",
                table: "VaktChangeRequestReplies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VaktChangeRequestReplies_VaktChangeRequestId",
                table: "VaktChangeRequestReplies",
                column: "VaktChangeRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_VaktChangeRequests_Users_UserId",
                table: "VaktChangeRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaktChangeRequests_Users_UserId",
                table: "VaktChangeRequests");

            migrationBuilder.DropTable(
                name: "VaktChangeAlternatives");

            migrationBuilder.DropTable(
                name: "VaktChangeRequestReplies");

            migrationBuilder.AddForeignKey(
                name: "FK_VaktChangeRequests_Users_UserId",
                table: "VaktChangeRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
