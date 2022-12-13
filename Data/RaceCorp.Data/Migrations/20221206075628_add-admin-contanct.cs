#nullable disable

namespace RaceCorp.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class addadmincontanct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminContacts_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminContacts_AdminId",
                table: "AdminContacts",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminContacts_IsDeleted",
                table: "AdminContacts",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminContacts");
        }
    }
}
