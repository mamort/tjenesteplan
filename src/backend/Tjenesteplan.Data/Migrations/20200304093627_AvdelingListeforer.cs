using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class AvdelingListeforer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListeforerId",
                table: "Avdelinger",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Avdelinger_ListeforerId",
                table: "Avdelinger",
                column: "ListeforerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Avdelinger_Users_ListeforerId",
                table: "Avdelinger",
                column: "ListeforerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avdelinger_Users_ListeforerId",
                table: "Avdelinger");

            migrationBuilder.DropIndex(
                name: "IX_Avdelinger_ListeforerId",
                table: "Avdelinger");

            migrationBuilder.DropColumn(
                name: "ListeforerId",
                table: "Avdelinger");
        }
    }
}
