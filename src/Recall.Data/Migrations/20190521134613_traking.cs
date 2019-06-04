using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Recall.Data.Migrations
{
    public partial class traking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAccessed",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesAccessed",
                table: "Videos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LastAccessed",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "TimesAccessed",
                table: "Videos");
        }
    }
}
