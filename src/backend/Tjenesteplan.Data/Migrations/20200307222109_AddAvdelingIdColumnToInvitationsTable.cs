using Microsoft.EntityFrameworkCore.Migrations;

namespace Tjenesteplan.Data.Migrations
{
    public partial class AddAvdelingIdColumnToInvitationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvdelingId",
                table: "Invitations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_AvdelingId",
                table: "Invitations",
                column: "AvdelingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Avdelinger_AvdelingId",
                table: "Invitations",
                column: "AvdelingId",
                principalTable: "Avdelinger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Avdelinger_AvdelingId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_AvdelingId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "AvdelingId",
                table: "Invitations");
        }
    }
}
