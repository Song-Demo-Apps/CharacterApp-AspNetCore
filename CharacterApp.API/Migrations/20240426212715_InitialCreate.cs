using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CharacterApp.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CharacterApp");

            migrationBuilder.CreateTable(
                name: "Items",
                schema: "CharacterApp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Value = table.Column<decimal>(type: "money", nullable: false, defaultValue: 0m),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.CheckConstraint("CK_Item_Value", "[Value] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "Species",
                schema: "CharacterApp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                schema: "CharacterApp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    DoB = table.Column<DateOnly>(type: "date", nullable: false),
                    CharacterSpeciesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Species_CharacterSpeciesId",
                        column: x => x.CharacterSpeciesId,
                        principalSchema: "CharacterApp",
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterItems",
                schema: "CharacterApp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterItems", x => x.Id);
                    table.CheckConstraint("CK_CharacterItem_Quantity", "[Quantity] >= 1");
                    table.ForeignKey(
                        name: "FK_CharacterItems_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "CharacterApp",
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "CharacterApp",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterItems_CharacterId",
                schema: "CharacterApp",
                table: "CharacterItems",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterItems_ItemId",
                schema: "CharacterApp",
                table: "CharacterItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CharacterSpeciesId",
                schema: "CharacterApp",
                table: "Characters",
                column: "CharacterSpeciesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterItems",
                schema: "CharacterApp");

            migrationBuilder.DropTable(
                name: "Characters",
                schema: "CharacterApp");

            migrationBuilder.DropTable(
                name: "Items",
                schema: "CharacterApp");

            migrationBuilder.DropTable(
                name: "Species",
                schema: "CharacterApp");
        }
    }
}
