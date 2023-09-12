using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class FinalTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__UserMessa__RoomI__29221CFB",
                table: "UserMessageRooms");

            migrationBuilder.DropForeignKey(
                name: "FK__UserMessa__UserI__2A164134",
                table: "UserMessageRooms");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoComm__Video__41EDCAC5",
                table: "VideoCommentsLikes");

            migrationBuilder.DropTable(
                name: "UserRooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK__VideoCom__3214EC0711BA0C9B",
                table: "VideoCommentsLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK__UserMess__3214EC07FF3707BC",
                table: "UserMessageRooms");

            migrationBuilder.AddPrimaryKey(
                name: "PK__VideoCom__3214EC076CC6E6B7",
                table: "VideoCommentsLikes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__UserMess__3214EC07D2F91607",
                table: "UserMessageRooms",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_VideoCommentsLikes_UserId",
                table: "VideoCommentsLikes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__UserMessa__RoomI__4D5F7D71",
                table: "UserMessageRooms",
                column: "RoomId",
                principalTable: "MessageRooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__UserMessa__UserI__4C6B5938",
                table: "UserMessageRooms",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__VideoComm__UserI__498EEC8D",
                table: "VideoCommentsLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__VideoComm__Video__489AC854",
                table: "VideoCommentsLikes",
                column: "VideoCommentId",
                principalTable: "VideoComments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__UserMessa__RoomI__4D5F7D71",
                table: "UserMessageRooms");

            migrationBuilder.DropForeignKey(
                name: "FK__UserMessa__UserI__4C6B5938",
                table: "UserMessageRooms");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoComm__UserI__498EEC8D",
                table: "VideoCommentsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK__VideoComm__Video__489AC854",
                table: "VideoCommentsLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK__VideoCom__3214EC076CC6E6B7",
                table: "VideoCommentsLikes");

            migrationBuilder.DropIndex(
                name: "IX_VideoCommentsLikes_UserId",
                table: "VideoCommentsLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK__UserMess__3214EC07D2F91607",
                table: "UserMessageRooms");

            migrationBuilder.AddPrimaryKey(
                name: "PK__VideoCom__3214EC0711BA0C9B",
                table: "VideoCommentsLikes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__UserMess__3214EC07FF3707BC",
                table: "UserMessageRooms",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_UserRooms_RoomId",
                table: "UserRooms",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRooms_UserId",
                table: "UserRooms",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__UserMessa__RoomI__29221CFB",
                table: "UserMessageRooms",
                column: "RoomId",
                principalTable: "MessageRooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__UserMessa__UserI__2A164134",
                table: "UserMessageRooms",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__VideoComm__Video__41EDCAC5",
                table: "VideoCommentsLikes",
                column: "VideoCommentId",
                principalTable: "VideoComments",
                principalColumn: "Id");
        }
    }
}
