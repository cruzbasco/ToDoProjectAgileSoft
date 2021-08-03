using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoDataAccess.Migrations
{
    public partial class addingKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRefId",
                table: "UserTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_UserRefId",
                table: "UserTasks",
                column: "UserRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_Users_UserRefId",
                table: "UserTasks",
                column: "UserRefId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_Users_UserRefId",
                table: "UserTasks");

            migrationBuilder.DropIndex(
                name: "IX_UserTasks_UserRefId",
                table: "UserTasks");

            migrationBuilder.DropColumn(
                name: "UserRefId",
                table: "UserTasks");
        }
    }
}
