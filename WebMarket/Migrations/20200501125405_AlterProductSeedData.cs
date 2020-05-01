using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMarket.Migrations
{
    public partial class AlterProductSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "AddedDate", "AddedToCart", "CardImageLink", "Description", "Discount", "FileName", "Link", "Name", "OnlyOneCommentPerUser", "OnlyRegisteredCanComment", "OwnerID", "Price", "Type", "ZipFilePath" },
                values: new object[] { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 10f, null, null, "AnotherTestProduct", false, false, null, 14.990m, "Game", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2);
        }
    }
}
