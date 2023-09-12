using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedVideosAndMessagesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "ForumMessages",
                type: "varchar(max)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);

            migrationBuilder.CreateTable(
                name: "MessageRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomName = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MessageR__3214EC07ED42FBDE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaylistName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Playlist__3214EC071252C5C2", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Playlists__UserI__367C1819",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VideoTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VideoTag__3214EC07AF11C7AA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessengerMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    MessageContent = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    FileName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Messenge__3214EC07417F862B", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Messenger__RoomI__18EBB532",
                        column: x => x.RoomId,
                        principalTable: "MessageRooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Messenger__UserI__17F790F9",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserMessageRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserMess__3214EC07FF3707BC", x => x.Id);
                    table.ForeignKey(
                        name: "FK__UserMessa__RoomI__29221CFB",
                        column: x => x.RoomId,
                        principalTable: "MessageRooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__UserMessa__UserI__2A164134",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserRoom__3214EC07D16F1851", x => x.Id);
                    table.ForeignKey(
                        name: "FK__UserRooms__RoomI__2FCF1A8A",
                        column: x => x.RoomId,
                        principalTable: "MessageRooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__UserRooms__UserI__2EDAF651",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    MP4Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    ThumbnailName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    VideoDescription = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Videos__3214EC07412B2B44", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Videos__TagId__339FAB6E",
                        column: x => x.TagId,
                        principalTable: "VideoTags",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Videos__UserId__32AB8735",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VideoComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageContent = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    ReplyId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VideoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VideoCom__3214EC0796C7F708", x => x.Id);
                    table.ForeignKey(
                        name: "FK__VideoComm__Reply__3D2915A8",
                        column: x => x.ReplyId,
                        principalTable: "VideoComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__VideoComm__UserI__3E1D39E1",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__VideoComm__Video__3F115E1A",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VideoPlaylists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaylistId = table.Column<int>(type: "int", nullable: false),
                    VideoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VideoPla__3214EC07F06C00FA", x => x.Id);
                    table.ForeignKey(
                        name: "FK__VideoPlay__Playl__395884C4",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__VideoPlay__Video__3A4CA8FD",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VideoCommentsLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VideoCommentId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VideoCom__3214EC0711BA0C9B", x => x.Id);
                    table.ForeignKey(
                        name: "FK__VideoComm__Video__41EDCAC5",
                        column: x => x.VideoCommentId,
                        principalTable: "VideoComments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessengerMessages_RoomId",
                table: "MessengerMessages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_MessengerMessages_UserId",
                table: "MessengerMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_UserId",
                table: "Playlists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessageRooms_RoomId",
                table: "UserMessageRooms",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessageRooms_UserId",
                table: "UserMessageRooms",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRooms_RoomId",
                table: "UserRooms",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRooms_UserId",
                table: "UserRooms",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoComments_ReplyId",
                table: "VideoComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoComments_UserId",
                table: "VideoComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoComments_VideoId",
                table: "VideoComments",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoCommentsLikes_VideoCommentId",
                table: "VideoCommentsLikes",
                column: "VideoCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoPlaylists_PlaylistId",
                table: "VideoPlaylists",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoPlaylists_VideoId",
                table: "VideoPlaylists",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_TagId",
                table: "Videos",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_UserId",
                table: "Videos",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessengerMessages");

            migrationBuilder.DropTable(
                name: "UserMessageRooms");

            migrationBuilder.DropTable(
                name: "UserRooms");

            migrationBuilder.DropTable(
                name: "VideoCommentsLikes");

            migrationBuilder.DropTable(
                name: "VideoPlaylists");

            migrationBuilder.DropTable(
                name: "MessageRooms");

            migrationBuilder.DropTable(
                name: "VideoComments");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "VideoTags");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "ForumMessages",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldNullable: true);
        }
    }
}
