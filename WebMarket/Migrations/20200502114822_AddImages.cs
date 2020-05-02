using Microsoft.EntityFrameworkCore.Migrations;

namespace WebMarket.Migrations
{
    public partial class AddImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    ProductID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
