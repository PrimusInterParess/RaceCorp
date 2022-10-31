using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceCorp.Data.Migrations
{
    public partial class usertracepropertyrace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RaceId",
                table: "ApplicationUserTrace",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTrace_RaceId",
                table: "ApplicationUserTrace",
                column: "RaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserTrace_Races_RaceId",
                table: "ApplicationUserTrace",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserTrace_Races_RaceId",
                table: "ApplicationUserTrace");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserTrace_RaceId",
                table: "ApplicationUserTrace");

            migrationBuilder.DropColumn(
                name: "RaceId",
                table: "ApplicationUserTrace");
        }
    }
}
