using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class MinorChangesAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__UserSubsc__UserC__17036CC0",
                table: "UserSubscribtion");

            migrationBuilder.DropForeignKey(
                name: "FK__UserSubsc__UserS__160F4887",
                table: "UserSubscribtion");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Users_UserId",
                table: "Videos");

            migrationBuilder.DropPrimaryKey(
                name: "PK__UserSubs__3214EC074F58E239",
                table: "UserSubscribtion");

            migrationBuilder.RenameTable(
                name: "UserSubscribtion",
                newName: "UserSubscribtions");

            migrationBuilder.RenameIndex(
                name: "IX_UserSubscribtion_UserSubscriberId",
                table: "UserSubscribtions",
                newName: "IX_UserSubscribtions_UserSubscriberId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSubscribtion_UserChannelId",
                table: "UserSubscribtions",
                newName: "IX_UserSubscribtions_UserChannelId");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UserSubscriberId",
                table: "UserSubscribtions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserChannelId",
                table: "UserSubscribtions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK__UserSubs__3214EC07DD7515EE",
                table: "UserSubscribtions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__UserSubsc__UserC__2739D489",
                table: "UserSubscribtions",
                column: "UserChannelId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__UserSubsc__UserS__282DF8C2",
                table: "UserSubscribtions",
                column: "UserSubscriberId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Videos__UserId__32AB8735",
                table: "Videos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__UserSubsc__UserC__2739D489",
                table: "UserSubscribtions");

            migrationBuilder.DropForeignKey(
                name: "FK__UserSubsc__UserS__282DF8C2",
                table: "UserSubscribtions");

            migrationBuilder.DropForeignKey(
                name: "FK__Videos__UserId__32AB8735",
                table: "Videos");

            migrationBuilder.DropPrimaryKey(
                name: "PK__UserSubs__3214EC07DD7515EE",
                table: "UserSubscribtions");

            migrationBuilder.RenameTable(
                name: "UserSubscribtions",
                newName: "UserSubscribtion");

            migrationBuilder.RenameIndex(
                name: "IX_UserSubscribtions_UserSubscriberId",
                table: "UserSubscribtion",
                newName: "IX_UserSubscribtion_UserSubscriberId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSubscribtions_UserChannelId",
                table: "UserSubscribtion",
                newName: "IX_UserSubscribtion_UserChannelId");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "UserSubscriberId",
                table: "UserSubscribtion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserChannelId",
                table: "UserSubscribtion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__UserSubs__3214EC074F58E239",
                table: "UserSubscribtion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__UserSubsc__UserC__17036CC0",
                table: "UserSubscribtion",
                column: "UserChannelId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__UserSubsc__UserS__160F4887",
                table: "UserSubscribtion",
                column: "UserSubscriberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Users_UserId",
                table: "Videos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
