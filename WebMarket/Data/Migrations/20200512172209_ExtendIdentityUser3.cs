using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMarket.Data.Migrations
{
    public partial class ExtendIdentityUser3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoughtProduct");

            migrationBuilder.DropTable(
                name: "Product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CardImageLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Discount = table.Column<float>(type: "real", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    OnlyOneCommentPerUser = table.Column<bool>(type: "bit", nullable: false),
                    OnlyRegisteredCanComment = table.Column<bool>(type: "bit", nullable: false),
                    OwnerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BoughtProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserRefId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoughtProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoughtProduct_AspNetUsers_AppUserRefId",
                        column: x => x.AppUserRefId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoughtProduct_Product_ProductRefId",
                        column: x => x.ProductRefId,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoughtProduct_AppUserRefId",
                table: "BoughtProduct",
                column: "AppUserRefId");

            migrationBuilder.CreateIndex(
                name: "IX_BoughtProduct_ProductRefId",
                table: "BoughtProduct",
                column: "ProductRefId");
        }
    }
}
