using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceCorp.Data.Migrations
{
    public partial class addadmincontanctreply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminContactReplyId",
                table: "AdminContacts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdminContactReplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AdminContactId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminContactReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminContactReplies_AdminContacts_AdminContactId",
                        column: x => x.AdminContactId,
                        principalTable: "AdminContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdminContactReplies_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminContacts_AdminContactReplyId",
                table: "AdminContacts",
                column: "AdminContactReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminContactReplies_AdminContactId",
                table: "AdminContactReplies",
                column: "AdminContactId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminContactReplies_AdminId",
                table: "AdminContactReplies",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminContactReplies_IsDeleted",
                table: "AdminContactReplies",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminContacts_AdminContactReplies_AdminContactReplyId",
                table: "AdminContacts",
                column: "AdminContactReplyId",
                principalTable: "AdminContactReplies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminContacts_AdminContactReplies_AdminContactReplyId",
                table: "AdminContacts");

            migrationBuilder.DropTable(
                name: "AdminContactReplies");

            migrationBuilder.DropIndex(
                name: "IX_AdminContacts_AdminContactReplyId",
                table: "AdminContacts");

            migrationBuilder.DropColumn(
                name: "AdminContactReplyId",
                table: "AdminContacts");
        }
    }
}
