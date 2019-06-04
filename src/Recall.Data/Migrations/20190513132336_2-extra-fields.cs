using Microsoft.EntityFrameworkCore.Migrations;

namespace Recall.Data.Migrations
{
    public partial class _2extrafields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Videos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Videos");
        }
    }
}
