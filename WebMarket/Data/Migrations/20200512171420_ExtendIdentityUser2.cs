using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMarket.Data.Migrations
{
    public partial class ExtendIdentityUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    Type = table.Column<string>(maxLength: 32, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<float>(nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    Link = table.Column<string>(maxLength: 128, nullable: true),
                    CardImageLink = table.Column<string>(nullable: true),
                    OnlyRegisteredCanComment = table.Column<bool>(nullable: false),
                    OnlyOneCommentPerUser = table.Column<bool>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    OwnerID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BoughtProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserRefId = table.Column<string>(nullable: true),
                    ProductRefId = table.Column<int>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoughtProduct");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
