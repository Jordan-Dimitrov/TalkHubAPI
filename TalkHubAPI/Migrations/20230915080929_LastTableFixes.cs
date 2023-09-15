using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class LastTableFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__VideoUser__Ratin__625A9A57",
                table: "VideoUserLikes");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoUser__Video__634EBE90",
                table: "VideoUserLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK__VideoUse__3214EC07BDE52B56",
                table: "VideoUserLikes");

            migrationBuilder.AddPrimaryKey(
                name: "PK__VideoUse__3214EC071132BA91",
                table: "VideoUserLikes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__VideoUser__UserI__6FE99F9F",
                table: "VideoUserLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__VideoUser__Video__70DDC3D8",
                table: "VideoUserLikes",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__VideoUser__UserI__6FE99F9F",
                table: "VideoUserLikes");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoUser__Video__70DDC3D8",
                table: "VideoUserLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK__VideoUse__3214EC071132BA91",
                table: "VideoUserLikes");

            migrationBuilder.AddPrimaryKey(
                name: "PK__VideoUse__3214EC07BDE52B56",
                table: "VideoUserLikes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__VideoUser__Ratin__625A9A57",
                table: "VideoUserLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__VideoUser__Video__634EBE90",
                table: "VideoUserLikes",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");
        }
    }
}
