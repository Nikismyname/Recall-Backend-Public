using Microsoft.EntityFrameworkCore.Migrations;

namespace Recall.Data.Migrations
{
    public partial class publicaccess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimesPublicAccessed",
                table: "Videos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimesPublicAccessed",
                table: "Videos");
        }
    }
}
