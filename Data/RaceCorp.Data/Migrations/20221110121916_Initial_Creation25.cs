using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceCorp.Data.Migrations
{
    public partial class Initial_Creation25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FacobookLink",
                table: "AspNetUsers",
                newName: "FacoBookLink");

            migrationBuilder.AddColumn<string>(
                name: "LogoImagePath",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamId",
                table: "Images",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_TeamId",
                table: "Images",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Teams_TeamId",
                table: "Images",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Teams_TeamId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_TeamId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "LogoImagePath",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "FacoBookLink",
                table: "AspNetUsers",
                newName: "FacobookLink");
        }
    }
}
