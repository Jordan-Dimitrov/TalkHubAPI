using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedForumTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForumThreads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThreadName = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    ThreadDescription = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ForumThr__3214EC07B31A0EE4", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForumMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageContent = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    FileName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    ReplyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    ForumThreadId = table.Column<int>(type: "int", nullable: false),
                    UpvoteCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ForumMes__3214EC0755C27262", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ForumMess__Forum__5CD6CB2B",
                        column: x => x.ForumThreadId,
                        principalTable: "ForumThreads",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__ForumMess__Reply__5AEE82B9",
                        column: x => x.ReplyId,
                        principalTable: "ForumMessages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__ForumMess__UserI__5BE2A6F2",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserUpvotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserUpvo__3214EC076B4006A6", x => x.Id);
                    table.ForeignKey(
                        name: "FK__UserUpvot__Messa__74AE54BC",
                        column: x => x.MessageId,
                        principalTable: "ForumMessages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__UserUpvot__UserI__73BA3083",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumMessages_ForumThreadId",
                table: "ForumMessages",
                column: "ForumThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumMessages_ReplyId",
                table: "ForumMessages",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumMessages_UserId",
                table: "ForumMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUpvotes_MessageId",
                table: "UserUpvotes",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUpvotes_UserId",
                table: "UserUpvotes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserUpvotes");

            migrationBuilder.DropTable(
                name: "ForumMessages");

            migrationBuilder.DropTable(
                name: "ForumThreads");
        }
    }
}
