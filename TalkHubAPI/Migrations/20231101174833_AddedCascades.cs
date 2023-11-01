using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedCascades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ForumMess__Forum__5CD6CB2B",
                table: "ForumMessages");

            migrationBuilder.DropForeignKey(
                name: "FK__Messenger__RoomI__18EBB532",
                table: "MessengerMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_PhotoCategory",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK__UserMessa__RoomI__4D5F7D71",
                table: "UserMessageRooms");

            migrationBuilder.DropForeignKey(
                name: "FK__UserUpvot__Messa__74AE54BC",
                table: "UserUpvotes");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoComm__Video__3F115E1A",
                table: "VideoComments");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoComm__Video__489AC854",
                table: "VideoCommentsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoPlay__Playl__395884C4",
                table: "VideoPlaylists");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoPlay__Video__3A4CA8FD",
                table: "VideoPlaylists");

            migrationBuilder.DropForeignKey(
                name: "FK__Videos__TagId__339FAB6E",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoUser__Video__70DDC3D8",
                table: "VideoUserLikes");

            migrationBuilder.AlterColumn<string>(
                name: "TagName",
                table: "VideoTags",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldUnicode: false,
                oldMaxLength: 45);

            migrationBuilder.AlterColumn<string>(
                name: "VideoName",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "VideoDescription",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "ThumbnailName",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "MP4Name",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "MessageContent",
                table: "VideoComments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "PlaylistName",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "MessageContent",
                table: "MessengerMessages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "MessengerMessages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoomName",
                table: "MessageRooms",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldUnicode: false,
                oldMaxLength: 45);

            migrationBuilder.AlterColumn<string>(
                name: "ThreadName",
                table: "ForumThreads",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldUnicode: false,
                oldMaxLength: 45);

            migrationBuilder.AlterColumn<string>(
                name: "ThreadDescription",
                table: "ForumThreads",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldUnicode: false,
                oldMaxLength: 45);

            migrationBuilder.AlterColumn<string>(
                name: "MessageContent",
                table: "ForumMessages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "ForumMessages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__ForumMess__Forum__5CD6CB2B",
                table: "ForumMessages",
                column: "ForumThreadId",
                principalTable: "ForumThreads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Messenger__RoomI__18EBB532",
                table: "MessengerMessages",
                column: "RoomId",
                principalTable: "MessageRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_PhotoCategory",
                table: "Photos",
                column: "CategoryId",
                principalTable: "PhotoCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__UserMessa__RoomI__4D5F7D71",
                table: "UserMessageRooms",
                column: "RoomId",
                principalTable: "MessageRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__UserUpvot__Messa__74AE54BC",
                table: "UserUpvotes",
                column: "MessageId",
                principalTable: "ForumMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__VideoComm__Video__3F115E1A",
                table: "VideoComments",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__VideoComm__Video__489AC854",
                table: "VideoCommentsLikes",
                column: "VideoCommentId",
                principalTable: "VideoComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__VideoPlay__Playl__395884C4",
                table: "VideoPlaylists",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__VideoPlay__Video__3A4CA8FD",
                table: "VideoPlaylists",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Videos__TagId__339FAB6E",
                table: "Videos",
                column: "TagId",
                principalTable: "VideoTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__VideoUser__Video__70DDC3D8",
                table: "VideoUserLikes",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ForumMess__Forum__5CD6CB2B",
                table: "ForumMessages");

            migrationBuilder.DropForeignKey(
                name: "FK__Messenger__RoomI__18EBB532",
                table: "MessengerMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_PhotoCategory",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK__UserMessa__RoomI__4D5F7D71",
                table: "UserMessageRooms");

            migrationBuilder.DropForeignKey(
                name: "FK__UserUpvot__Messa__74AE54BC",
                table: "UserUpvotes");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoComm__Video__3F115E1A",
                table: "VideoComments");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoComm__Video__489AC854",
                table: "VideoCommentsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoPlay__Playl__395884C4",
                table: "VideoPlaylists");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoPlay__Video__3A4CA8FD",
                table: "VideoPlaylists");

            migrationBuilder.DropForeignKey(
                name: "FK__Videos__TagId__339FAB6E",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoUser__Video__70DDC3D8",
                table: "VideoUserLikes");

            migrationBuilder.AlterColumn<string>(
                name: "TagName",
                table: "VideoTags",
                type: "varchar(45)",
                unicode: false,
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(45)",
                oldMaxLength: 45);

            migrationBuilder.AlterColumn<string>(
                name: "VideoName",
                table: "Videos",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VideoDescription",
                table: "Videos",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ThumbnailName",
                table: "Videos",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MP4Name",
                table: "Videos",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MessageContent",
                table: "VideoComments",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PlaylistName",
                table: "Playlists",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Photos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MessageContent",
                table: "MessengerMessages",
                type: "varchar(max)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "MessengerMessages",
                type: "varchar(max)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoomName",
                table: "MessageRooms",
                type: "varchar(45)",
                unicode: false,
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(45)",
                oldMaxLength: 45);

            migrationBuilder.AlterColumn<string>(
                name: "ThreadName",
                table: "ForumThreads",
                type: "varchar(45)",
                unicode: false,
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "ThreadDescription",
                table: "ForumThreads",
                type: "varchar(45)",
                unicode: false,
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "MessageContent",
                table: "ForumMessages",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "ForumMessages",
                type: "varchar(max)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__ForumMess__Forum__5CD6CB2B",
                table: "ForumMessages",
                column: "ForumThreadId",
                principalTable: "ForumThreads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Messenger__RoomI__18EBB532",
                table: "MessengerMessages",
                column: "RoomId",
                principalTable: "MessageRooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_PhotoCategory",
                table: "Photos",
                column: "CategoryId",
                principalTable: "PhotoCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__UserMessa__RoomI__4D5F7D71",
                table: "UserMessageRooms",
                column: "RoomId",
                principalTable: "MessageRooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__UserUpvot__Messa__74AE54BC",
                table: "UserUpvotes",
                column: "MessageId",
                principalTable: "ForumMessages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__VideoComm__Video__3F115E1A",
                table: "VideoComments",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__VideoComm__Video__489AC854",
                table: "VideoCommentsLikes",
                column: "VideoCommentId",
                principalTable: "VideoComments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__VideoPlay__Playl__395884C4",
                table: "VideoPlaylists",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__VideoPlay__Video__3A4CA8FD",
                table: "VideoPlaylists",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Videos__TagId__339FAB6E",
                table: "Videos",
                column: "TagId",
                principalTable: "VideoTags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__VideoUser__Video__70DDC3D8",
                table: "VideoUserLikes",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");
        }
    }
}
