using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NyTimesData.Migrations
{
    public partial class Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roots",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    copyright = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    section = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    num_results = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RootId = table.Column<long>(type: "bigint", nullable: false),
                    section = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    subsection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    @abstract = table.Column<string>(name: "abstract", type: "nvarchar(max)", nullable: true),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    byline = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    published_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    material_type_facet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    kicker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    des_facet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    org_facet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    per_facet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    geo_facet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    short_url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Results_Roots_RootId",
                        column: x => x.RootId,
                        principalTable: "Roots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Multimedia",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResultId = table.Column<long>(type: "bigint", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    format = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    height = table.Column<int>(type: "int", nullable: false),
                    width = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    subtype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    copyright = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Multimedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Multimedia_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Multimedia_ResultId",
                table: "Multimedia",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_RootId",
                table: "Results",
                column: "RootId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Multimedia");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Roots");
        }
    }
}
