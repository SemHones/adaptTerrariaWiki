using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace terraria_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate0_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArmorPieces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false),
                    Href = table.Column<string>(type: "TEXT", nullable: false),
                    ObtainedFrom = table.Column<string>(type: "TEXT", nullable: false),
                    Defense = table.Column<int>(type: "INTEGER", nullable: false),
                    BodySlot = table.Column<string>(type: "TEXT", nullable: false),
                    ToolTip = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmorPieces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArmorSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false),
                    Href = table.Column<string>(type: "TEXT", nullable: false),
                    SetBonus = table.Column<string>(type: "TEXT", nullable: false),
                    HeadId = table.Column<int>(type: "INTEGER", nullable: false),
                    BodyId = table.Column<int>(type: "INTEGER", nullable: false),
                    LegsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmorSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArmorSets_ArmorPieces_BodyId",
                        column: x => x.BodyId,
                        principalTable: "ArmorPieces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArmorSets_ArmorPieces_HeadId",
                        column: x => x.HeadId,
                        principalTable: "ArmorPieces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArmorSets_ArmorPieces_LegsId",
                        column: x => x.LegsId,
                        principalTable: "ArmorPieces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArmorSets_BodyId",
                table: "ArmorSets",
                column: "BodyId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmorSets_HeadId",
                table: "ArmorSets",
                column: "HeadId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmorSets_LegsId",
                table: "ArmorSets",
                column: "LegsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArmorSets");

            migrationBuilder.DropTable(
                name: "ArmorPieces");
        }
    }
}
