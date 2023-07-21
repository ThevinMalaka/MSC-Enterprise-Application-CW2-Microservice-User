using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace userService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CheatMeals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CheatMeals_UserId",
                table: "CheatMeals",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheatMeals_Users_UserId",
                table: "CheatMeals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheatMeals_Users_UserId",
                table: "CheatMeals");

            migrationBuilder.DropIndex(
                name: "IX_CheatMeals_UserId",
                table: "CheatMeals");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CheatMeals");
        }
    }
}
