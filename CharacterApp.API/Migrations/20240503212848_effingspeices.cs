using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CharacterApp.API.Migrations
{
    /// <inheritdoc />
    public partial class effingspeices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Money",
                schema: "CharacterApp",
                table: "Characters",
                type: "money",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "money");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Money",
                schema: "CharacterApp",
                table: "Characters",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldDefaultValue: 0m);
        }
    }
}
