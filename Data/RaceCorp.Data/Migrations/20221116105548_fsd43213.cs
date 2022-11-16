using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceCorp.Data.Migrations
{
    public partial class fsd43213 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gpxs_AspNetUsers_ApplicationUserId1",
                table: "Gpxs");

            migrationBuilder.DropIndex(
                name: "IX_Gpxs_ApplicationUserId1",
                table: "Gpxs");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Gpxs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Gpxs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gpxs_ApplicationUserId1",
                table: "Gpxs",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Gpxs_AspNetUsers_ApplicationUserId1",
                table: "Gpxs",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
