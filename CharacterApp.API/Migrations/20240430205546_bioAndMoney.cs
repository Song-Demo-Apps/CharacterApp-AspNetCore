using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CharacterApp.API.Migrations
{
    /// <inheritdoc />
    public partial class bioAndMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                schema: "CharacterApp",
                table: "Characters",
                type: "nvarchar(1000)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Money",
                schema: "CharacterApp",
                table: "Characters",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                schema: "CharacterApp",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Money",
                schema: "CharacterApp",
                table: "Characters");
        }
    }
}
