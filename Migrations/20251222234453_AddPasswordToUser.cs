using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projec.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AssistCount", "BirthYear", "GoalCount", "MatchCount", "Name", "Password", "Username" },
                values: new object[] { 528, 2002, 998, 39, "enes", "123", "varım" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AssistCount", "BirthYear", "GoalCount", "MatchCount", "Name", "Username" },
                values: new object[] { 0, 2000, 0, 0, "Admin", "admin" });
        }
    }
}
