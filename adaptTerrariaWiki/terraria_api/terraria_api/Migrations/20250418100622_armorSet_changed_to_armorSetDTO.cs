using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace terraria_api.Migrations
{
    /// <inheritdoc />
    public partial class armorSet_changed_to_armorSetDTO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArmorSetDTOs",
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
                    table.PrimaryKey("PK_ArmorSetDTOs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArmorSetDTOs");
        }
    }
}
