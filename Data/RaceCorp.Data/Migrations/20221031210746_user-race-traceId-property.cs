using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceCorp.Data.Migrations
{
    public partial class userracetraceIdproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserRace_Traces_TraceId",
                table: "ApplicationUserRace");

            migrationBuilder.DropColumn(
                name: "TraceName",
                table: "ApplicationUserRace");

            migrationBuilder.AlterColumn<int>(
                name: "TraceId",
                table: "ApplicationUserRace",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserRace_Traces_TraceId",
                table: "ApplicationUserRace",
                column: "TraceId",
                principalTable: "Traces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserRace_Traces_TraceId",
                table: "ApplicationUserRace");

            migrationBuilder.AlterColumn<int>(
                name: "TraceId",
                table: "ApplicationUserRace",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "TraceName",
                table: "ApplicationUserRace",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserRace_Traces_TraceId",
                table: "ApplicationUserRace",
                column: "TraceId",
                principalTable: "Traces",
                principalColumn: "Id");
        }
    }
}
