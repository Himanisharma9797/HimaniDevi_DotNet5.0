using Microsoft.EntityFrameworkCore.Migrations;

namespace Employee.Service.DAL.Migrations
{
    public partial class MOdelchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_taskDetails_UserInfos_userInfoUserId",
                table: "taskDetails");

            migrationBuilder.DropIndex(
                name: "IX_taskDetails_userInfoUserId",
                table: "taskDetails");

            migrationBuilder.DropColumn(
                name: "userInfoUserId",
                table: "taskDetails");

            migrationBuilder.AddColumn<double>(
                name: "EstimatedHours",
                table: "taskDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_taskDetails_UserId",
                table: "taskDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_taskDetails_UserInfos_UserId",
                table: "taskDetails",
                column: "UserId",
                principalTable: "UserInfos",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_taskDetails_UserInfos_UserId",
                table: "taskDetails");

            migrationBuilder.DropIndex(
                name: "IX_taskDetails_UserId",
                table: "taskDetails");

            migrationBuilder.DropColumn(
                name: "EstimatedHours",
                table: "taskDetails");

            migrationBuilder.AddColumn<int>(
                name: "userInfoUserId",
                table: "taskDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_taskDetails_userInfoUserId",
                table: "taskDetails",
                column: "userInfoUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_taskDetails_UserInfos_userInfoUserId",
                table: "taskDetails",
                column: "userInfoUserId",
                principalTable: "UserInfos",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
