using Microsoft.EntityFrameworkCore.Migrations;

namespace demo1.Data.Migrations
{
    public partial class UserChange_Changenameto_UserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User",
                table: "HolidayRequests",
                newName: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "HolidayRequests",
                newName: "User");
        }
    }
}
