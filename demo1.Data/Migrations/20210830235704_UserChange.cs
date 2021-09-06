using Microsoft.EntityFrameworkCore.Migrations;

namespace demo1.Data.Migrations
{
    public partial class UserChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HolidayRequests_Users_UserId",
                table: "HolidayRequests");

            migrationBuilder.DropIndex(
                name: "IX_HolidayRequests_UserId",
                table: "HolidayRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "HolidayRequests");

            migrationBuilder.AddColumn<int>(
                name: "User",
                table: "HolidayRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "HolidayRequests");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "HolidayRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HolidayRequests_UserId",
                table: "HolidayRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HolidayRequests_Users_UserId",
                table: "HolidayRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
