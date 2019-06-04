using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Recall.Data.Migrations
{
    public partial class postgreQQLInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Username = table.Column<string>(nullable: true),
                    HashedPassword = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    RootDirectoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Directories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    NumberOfSubdirectories = table.Column<int>(nullable: false),
                    NumberOfVideos = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ParentDirectoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Directories_Directories_ParentDirectoryId",
                        column: x => x.ParentDirectoryId,
                        principalTable: "Directories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Directories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    SeekTo = table.Column<int>(nullable: true),
                    IsYouTube = table.Column<bool>(nullable: false),
                    IsVimeo = table.Column<bool>(nullable: false),
                    IsLocal = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    DirectoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_Directories_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "Directories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Videos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Content = table.Column<string>(nullable: true),
                    SeekTo = table.Column<int>(nullable: true),
                    Formatting = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    BorderThickness = table.Column<int>(nullable: false),
                    BorderColor = table.Column<string>(nullable: true),
                    BackgroundColor = table.Column<string>(nullable: true),
                    TextColor = table.Column<string>(nullable: true),
                    ParentNoteId = table.Column<int>(nullable: true),
                    VideoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Notes_ParentNoteId",
                        column: x => x.ParentNoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notes_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Directories_ParentDirectoryId",
                table: "Directories",
                column: "ParentDirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Directories_UserId",
                table: "Directories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ParentNoteId",
                table: "Notes",
                column: "ParentNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_VideoId",
                table: "Notes",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RootDirectoryId",
                table: "Users",
                column: "RootDirectoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_DirectoryId",
                table: "Videos",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_UserId",
                table: "Videos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Directories_RootDirectoryId",
                table: "Users",
                column: "RootDirectoryId",
                principalTable: "Directories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Directories_Users_UserId",
                table: "Directories");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Directories");
        }
    }
}
