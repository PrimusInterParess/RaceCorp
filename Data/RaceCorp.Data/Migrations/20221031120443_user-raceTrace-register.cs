using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceCorp.Data.Migrations
{
    public partial class userraceTraceregister : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_UserId",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "PictureId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TraceId",
                table: "ApplicationUserRace",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TraceName",
                table: "ApplicationUserRace",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationUserTrace",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TraceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserTrace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserTrace_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationUserTrace_Traces_TraceId",
                        column: x => x.TraceId,
                        principalTable: "Traces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_UserId",
                table: "Images",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserRace_TraceId",
                table: "ApplicationUserRace",
                column: "TraceId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTrace_ApplicationUserId",
                table: "ApplicationUserTrace",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTrace_TraceId",
                table: "ApplicationUserTrace",
                column: "TraceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserRace_Traces_TraceId",
                table: "ApplicationUserRace",
                column: "TraceId",
                principalTable: "Traces",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserRace_Traces_TraceId",
                table: "ApplicationUserRace");

            migrationBuilder.DropTable(
                name: "ApplicationUserTrace");

            migrationBuilder.DropIndex(
                name: "IX_Images_UserId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserRace_TraceId",
                table: "ApplicationUserRace");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TraceId",
                table: "ApplicationUserRace");

            migrationBuilder.DropColumn(
                name: "TraceName",
                table: "ApplicationUserRace");

            migrationBuilder.CreateIndex(
                name: "IX_Images_UserId",
                table: "Images",
                column: "UserId");
        }
    }
}
