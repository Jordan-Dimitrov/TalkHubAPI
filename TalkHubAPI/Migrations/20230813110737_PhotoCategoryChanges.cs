using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class PhotoCategoryChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Photos");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "PhotoCategory",
                newName: "PhotoCategories");

            migrationBuilder.RenameIndex(
                name: "IX_User_RefreshTokenId",
                table: "Users",
                newName: "IX_Users_RefreshTokenId");

            migrationBuilder.RenameColumn(
                name: "PhotoName",
                table: "PhotoCategories",
                newName: "CategoryName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "PhotoCategories",
                newName: "PhotoCategory");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RefreshTokenId",
                table: "User",
                newName: "IX_User_RefreshTokenId");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "PhotoCategory",
                newName: "PhotoName");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Photos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
