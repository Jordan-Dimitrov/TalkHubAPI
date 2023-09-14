using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserLikesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VideoUserLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VideoId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VideoUse__3214EC07BDE52B56", x => x.Id);
                    table.ForeignKey(
                        name: "FK__VideoUser__Ratin__625A9A57",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__VideoUser__Video__634EBE90",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoUserLikes_UserId",
                table: "VideoUserLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoUserLikes_VideoId",
                table: "VideoUserLikes",
                column: "VideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoUserLikes");
        }
    }
}
