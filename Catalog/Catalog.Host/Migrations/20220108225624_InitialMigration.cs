using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Host.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateSequence(
                name: "catalog_brand_hilo",
                incrementBy: 10);

            _ = migrationBuilder.CreateSequence(
                name: "catalog_hilo",
                incrementBy: 10);

            _ = migrationBuilder.CreateSequence(
                name: "catalog_type_hilo",
                incrementBy: 10);

            _ = migrationBuilder.CreateTable(
                name: "CatalogBrand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_CatalogBrand", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "CatalogType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_CatalogType", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Catalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    PictureFileName = table.Column<string>(type: "text", nullable: true),
                    CatalogTypeId = table.Column<int>(type: "integer", nullable: false),
                    CatalogBrandId = table.Column<int>(type: "integer", nullable: false),
                    AvailableStock = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Catalog", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Catalog_CatalogBrand_CatalogBrandId",
                        column: x => x.CatalogBrandId,
                        principalTable: "CatalogBrand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_Catalog_CatalogType_CatalogTypeId",
                        column: x => x.CatalogTypeId,
                        principalTable: "CatalogType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Catalog_CatalogBrandId",
                table: "Catalog",
                column: "CatalogBrandId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Catalog_CatalogTypeId",
                table: "Catalog",
                column: "CatalogTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "Catalog");

            _ = migrationBuilder.DropTable(
                name: "CatalogBrand");

            _ = migrationBuilder.DropTable(
                name: "CatalogType");

            _ = migrationBuilder.DropSequence(
                name: "catalog_brand_hilo");

            _ = migrationBuilder.DropSequence(
                name: "catalog_hilo");

            _ = migrationBuilder.DropSequence(
                name: "catalog_type_hilo");
        }
    }
}
