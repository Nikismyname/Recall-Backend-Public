using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Recall.Data.Migrations
{
    public partial class anoteherYet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserOptionsId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    VideoShowPercentageWatched = table.Column<bool>(nullable: false),
                    VideoShowNotesCount = table.Column<bool>(nullable: false),
                    Theme = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOptions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserOptionsId",
                table: "Users",
                column: "UserOptionsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserOptions_UserOptionsId",
                table: "Users",
                column: "UserOptionsId",
                principalTable: "UserOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserOptions_UserOptionsId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserOptions");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserOptionsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserOptionsId",
                table: "Users");
        }
    }
}
