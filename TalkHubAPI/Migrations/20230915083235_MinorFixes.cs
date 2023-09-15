using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class MinorFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__VideoUse__3214EC071132BA91",
                table: "VideoUserLikes");

            migrationBuilder.AddPrimaryKey(
                name: "PK__VideoUse__3214EC07710CAE9C",
                table: "VideoUserLikes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__VideoUse__3214EC07710CAE9C",
                table: "VideoUserLikes");

            migrationBuilder.AddPrimaryKey(
                name: "PK__VideoUse__3214EC071132BA91",
                table: "VideoUserLikes",
                column: "Id");
        }
    }
}
