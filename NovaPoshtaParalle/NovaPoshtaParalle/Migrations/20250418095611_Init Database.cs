using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaPoshtaParalle.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblAreas",
                columns: table => new
                {
                    Ref = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAreas", x => x.Ref);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Ref = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    SettlementTypeDescription = table.Column<string>(type: "TEXT", nullable: false),
                    Area = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Ref);
                    table.ForeignKey(
                        name: "FK_Cities_tblAreas_Area",
                        column: x => x.Area,
                        principalTable: "tblAreas",
                        principalColumn: "Ref",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblDepartments",
                columns: table => new
                {
                    Ref = table.Column<string>(type: "TEXT", nullable: false),
                    CityRef = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    ShortAddress = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDepartments", x => x.Ref);
                    table.ForeignKey(
                        name: "FK_tblDepartments_Cities_CityRef",
                        column: x => x.CityRef,
                        principalTable: "Cities",
                        principalColumn: "Ref",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Area",
                table: "Cities",
                column: "Area");

            migrationBuilder.CreateIndex(
                name: "IX_tblDepartments_CityRef",
                table: "tblDepartments",
                column: "CityRef");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblDepartments");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "tblAreas");
        }
    }
}
