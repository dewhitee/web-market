using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMarket.Migrations
{
    public partial class ProductMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    CostIntegral = table.Column<int>(nullable: false),
                    CostFractional = table.Column<int>(nullable: false),
                    Discount = table.Column<float>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    CardImageLink = table.Column<string>(nullable: true),
                    OnlyRegisteredCanComment = table.Column<bool>(nullable: false),
                    OnlyOneCommentPerUser = table.Column<bool>(nullable: false),
                    AddedToCart = table.Column<bool>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    ZipFilePath = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    OwnerID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
