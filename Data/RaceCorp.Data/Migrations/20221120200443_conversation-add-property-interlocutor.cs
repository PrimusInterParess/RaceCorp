using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceCorp.Data.Migrations
{
    public partial class conversationaddpropertyinterlocutor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Conversations",
                newName: "InterlocutorId");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Conversations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Conversations");

            migrationBuilder.RenameColumn(
                name: "InterlocutorId",
                table: "Conversations",
                newName: "UserId");
        }
    }
}
