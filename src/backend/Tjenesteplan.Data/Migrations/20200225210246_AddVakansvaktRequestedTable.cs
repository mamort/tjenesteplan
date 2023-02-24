using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class AddVakansvaktRequestedTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VakansvaktRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TjenesteplanId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    OriginalLegeId = table.Column<int>(nullable: false),
                    CoveredByLegeId = table.Column<int>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Approved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VakansvaktRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VakansvaktRequests_Users_CoveredByLegeId",
                        column: x => x.CoveredByLegeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VakansvaktRequests_Users_OriginalLegeId",
                        column: x => x.OriginalLegeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VakansvaktRequests_Tjenesteplaner_TjenesteplanId",
                        column: x => x.TjenesteplanId,
                        principalTable: "Tjenesteplaner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VakansvaktRequests_CoveredByLegeId",
                table: "VakansvaktRequests",
                column: "CoveredByLegeId");

            migrationBuilder.CreateIndex(
                name: "IX_VakansvaktRequests_OriginalLegeId",
                table: "VakansvaktRequests",
                column: "OriginalLegeId");

            migrationBuilder.CreateIndex(
                name: "IX_VakansvaktRequests_TjenesteplanId",
                table: "VakansvaktRequests",
                column: "TjenesteplanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VakansvaktRequests");
        }
    }
}
