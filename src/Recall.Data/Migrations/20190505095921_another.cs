using Microsoft.EntityFrameworkCore.Migrations;

namespace Recall.Data.Migrations
{
    public partial class another : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RootDirectoryId",
                table: "Users",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RootDirectoryId",
                table: "Users",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
