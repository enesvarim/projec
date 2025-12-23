using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projec.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerCountToTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayerCount",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerCount",
                table: "Teams");
        }
    }
}
