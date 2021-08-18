using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace demo1.Data.Migrations
{
    public partial class HolidayRequestAppoval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HolidayRequestApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolidayRequestId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayRequestApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolidayRequestApprovals_HolidayRequests_HolidayRequestId",
                        column: x => x.HolidayRequestId,
                        principalTable: "HolidayRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HolidayRequestApprovals_HolidayRequestId",
                table: "HolidayRequestApprovals",
                column: "HolidayRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HolidayRequestApprovals");
        }
    }
}
