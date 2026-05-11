using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MtgInventoryManagement.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class cards_migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Artist = table.Column<string>(type: "text", nullable: true),
                    Border = table.Column<string>(type: "text", nullable: true),
                    Cmc = table.Column<int>(type: "integer", nullable: false),
                    ColorIdentity = table.Column<List<string>>(type: "text[]", nullable: false),
                    Colors = table.Column<List<string>>(type: "text[]", nullable: false),
                    ColorIndicator = table.Column<List<string>>(type: "text[]", nullable: false),
                    Cost = table.Column<string>(type: "text", nullable: true),
                    Flavor = table.Column<string>(type: "text", nullable: true),
                    FrameVersion = table.Column<string>(type: "text", nullable: true),
                    Layout = table.Column<string>(type: "text", nullable: false),
                    HasAlternativeDeckLimit = table.Column<bool>(type: "boolean", nullable: false),
                    Alternative = table.Column<bool>(type: "boolean", nullable: false),
                    Funny = table.Column<bool>(type: "boolean", nullable: false),
                    Rebalanced = table.Column<bool>(type: "boolean", nullable: false),
                    StorySpotlight = table.Column<bool>(type: "boolean", nullable: false),
                    Power = table.Column<string>(type: "text", nullable: true),
                    Toughness = table.Column<string>(type: "text", nullable: true),
                    Loyalty = table.Column<int>(type: "integer", nullable: false),
                    Rarity = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true),
                    MkmId = table.Column<int>(type: "integer", nullable: false),
                    TcgPlayerId = table.Column<int>(type: "integer", nullable: false),
                    ScryfallId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScryfallIllustrationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Keywords = table.Column<List<string>>(type: "text[]", nullable: false),
                    Subtypes = table.Column<List<string>>(type: "text[]", nullable: false),
                    Supertypes = table.Column<List<string>>(type: "text[]", nullable: false),
                    Types = table.Column<List<string>>(type: "text[]", nullable: false),
                    Finishes = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Editions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Set = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    CardCount = table.Column<int>(type: "integer", nullable: false),
                    KeyRuneCode = table.Column<string>(type: "text", nullable: true),
                    Booster = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForeignNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CardId = table.Column<Guid>(type: "uuid", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Flavor = table.Column<string>(type: "text", nullable: true),
                    GathererId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForeignNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForeignNames_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Legalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CardId = table.Column<Guid>(type: "uuid", nullable: false),
                    Format = table.Column<string>(type: "text", nullable: false),
                    FormatLegality = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Legalities_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardEditions",
                columns: table => new
                {
                    CardId = table.Column<Guid>(type: "uuid", nullable: false),
                    EditionId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardEditions", x => new { x.CardId, x.EditionId });
                    table.ForeignKey(
                        name: "FK_CardEditions_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardEditions_Editions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Editions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardEditions_EditionId",
                table: "CardEditions",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ForeignNames_CardId",
                table: "ForeignNames",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Legalities_CardId",
                table: "Legalities",
                column: "CardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardEditions");

            migrationBuilder.DropTable(
                name: "ForeignNames");

            migrationBuilder.DropTable(
                name: "Legalities");

            migrationBuilder.DropTable(
                name: "Editions");

            migrationBuilder.DropTable(
                name: "Cards");
        }
    }
}
