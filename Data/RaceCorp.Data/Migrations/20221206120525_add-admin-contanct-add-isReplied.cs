#nullable disable

namespace RaceCorp.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class addadmincontanctaddisReplied : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReplied",
                table: "AdminContacts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReplied",
                table: "AdminContacts");
        }
    }
}
