using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMarket.Migrations
{
    public partial class UpdateImages_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "Images",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "Images",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256);
        }
    }
}
