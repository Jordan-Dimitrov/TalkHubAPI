using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
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
                name: "PhotoCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhotoCat__3214EC072A92E285", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TokenCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    TokenExpires = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RefreshT__3214EC077AC6B002", x => x.Id);
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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    RefreshTokenId = table.Column<int>(type: "int", nullable: true),
                    PermissionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__3214EC07617E5831", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Users",
                        column: x => x.RefreshTokenId,
                        principalTable: "RefreshTokens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ForumMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageContent = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    FileName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ReplyId = table.Column<int>(type: "int", nullable: true),
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
                name: "MessengerMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    MessageContent = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    FileName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false)
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
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Photos__3214EC07370B195E", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_PhotoCategory",
                        column: x => x.CategoryId,
                        principalTable: "PhotoCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Photos__userId__31EC6D26",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
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
                    table.PrimaryKey("PK__UserMess__3214EC07D2F91607", x => x.Id);
                    table.ForeignKey(
                        name: "FK__UserMessa__RoomI__4D5F7D71",
                        column: x => x.RoomId,
                        principalTable: "MessageRooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__UserMessa__UserI__4C6B5938",
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
                name: "UserUpvotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
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
                    table.PrimaryKey("PK__VideoUse__3214EC07710CAE9C", x => x.Id);
                    table.ForeignKey(
                        name: "FK__VideoUser__UserI__6FE99F9F",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__VideoUser__Video__70DDC3D8",
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
                    table.PrimaryKey("PK__VideoCom__3214EC076CC6E6B7", x => x.Id);
                    table.ForeignKey(
                        name: "FK__VideoComm__UserI__498EEC8D",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__VideoComm__Video__489AC854",
                        column: x => x.VideoCommentId,
                        principalTable: "VideoComments",
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
                name: "IX_MessengerMessages_RoomId",
                table: "MessengerMessages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_MessengerMessages_UserId",
                table: "MessengerMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_CategoryId",
                table: "Photos",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UserId",
                table: "Photos",
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
                name: "IX_Users_RefreshTokenId",
                table: "Users",
                column: "RefreshTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUpvotes_MessageId",
                table: "UserUpvotes",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUpvotes_UserId",
                table: "UserUpvotes",
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
                name: "IX_VideoCommentsLikes_UserId",
                table: "VideoCommentsLikes",
                column: "UserId");

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
                name: "MessengerMessages");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "UserMessageRooms");

            migrationBuilder.DropTable(
                name: "UserUpvotes");

            migrationBuilder.DropTable(
                name: "VideoCommentsLikes");

            migrationBuilder.DropTable(
                name: "VideoPlaylists");

            migrationBuilder.DropTable(
                name: "VideoUserLikes");

            migrationBuilder.DropTable(
                name: "PhotoCategories");

            migrationBuilder.DropTable(
                name: "MessageRooms");

            migrationBuilder.DropTable(
                name: "ForumMessages");

            migrationBuilder.DropTable(
                name: "VideoComments");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "ForumThreads");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "VideoTags");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "RefreshTokens");
        }
    }
}
