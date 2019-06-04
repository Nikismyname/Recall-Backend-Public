using Microsoft.EntityFrameworkCore.Migrations;

namespace Recall.Data.Migrations
{
    public partial class extensionVidType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ExtensionAddedVideos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ExtensionAddedVideos");
        }
    }
}
