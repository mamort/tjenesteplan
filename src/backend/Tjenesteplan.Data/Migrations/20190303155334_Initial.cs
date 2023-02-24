using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TjenesteplanUker",
                columns: table => new
                {
                    TjenesteplanId = table.Column<int>(nullable: false),
                    TjenesteplanUkeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TjenesteplanUker", x => new { x.TjenesteplanId, x.TjenesteplanUkeId });
                });

            migrationBuilder.CreateTable(
                name: "TjenesteplanUkedager",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TjenesteplanId = table.Column<int>(nullable: false),
                    TjenesteplanUkeId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Dagsplan = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TjenesteplanUkedager", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TjenesteplanUkedager_TjenesteplanUker_TjenesteplanId_TjenesteplanUkeId",
                        columns: x => new { x.TjenesteplanId, x.TjenesteplanUkeId },
                        principalTable: "TjenesteplanUker",
                        principalColumns: new[] { "TjenesteplanId", "TjenesteplanUkeId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    TjenesteplanId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tjenesteplaner",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    NumberOfWeeks = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tjenesteplaner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tjenesteplaner_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tjenesteplaner_UserId",
                table: "Tjenesteplaner",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TjenesteplanUkedager_TjenesteplanId_TjenesteplanUkeId",
                table: "TjenesteplanUkedager",
                columns: new[] { "TjenesteplanId", "TjenesteplanUkeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TjenesteplanId",
                table: "Users",
                column: "TjenesteplanId");

            migrationBuilder.AddForeignKey(
                name: "FK_TjenesteplanUker_Tjenesteplaner_TjenesteplanId",
                table: "TjenesteplanUker",
                column: "TjenesteplanId",
                principalTable: "Tjenesteplaner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tjenesteplaner_TjenesteplanId",
                table: "Users",
                column: "TjenesteplanId",
                principalTable: "Tjenesteplaner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tjenesteplaner_Users_UserId",
                table: "Tjenesteplaner");

            migrationBuilder.DropTable(
                name: "TjenesteplanUkedager");

            migrationBuilder.DropTable(
                name: "TjenesteplanUker");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Tjenesteplaner");
        }
    }
}
