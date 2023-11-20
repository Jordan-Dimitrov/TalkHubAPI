using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUserTableAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Videos__UserId__32AB8735",
                table: "Videos");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Videos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SubscriberCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserSubscribtion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserSubscriberId = table.Column<int>(type: "int", nullable: false),
                    UserChannelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserSubs__3214EC074F58E239", x => x.Id);
                    table.ForeignKey(
                        name: "FK__UserSubsc__UserC__17036CC0",
                        column: x => x.UserChannelId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__UserSubsc__UserS__160F4887",
                        column: x => x.UserSubscriberId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscribtion_UserChannelId",
                table: "UserSubscribtion",
                column: "UserChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscribtion_UserSubscriberId",
                table: "UserSubscribtion",
                column: "UserSubscriberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Users_UserId",
                table: "Videos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Users_UserId",
                table: "Videos");

            migrationBuilder.DropTable(
                name: "UserSubscribtion");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "SubscriberCount",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK__Videos__UserId__32AB8735",
                table: "Videos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
