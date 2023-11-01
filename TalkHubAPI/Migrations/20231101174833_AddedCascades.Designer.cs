﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TalkHubAPI.Data;

#nullable disable

namespace TalkHubAPI.Migrations
{
    [DbContext(typeof(TalkHubContext))]
    [Migration("20231101174833_AddedCascades")]
    partial class AddedCascades
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TalkHubAPI.Models.ForumModels.ForumMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ForumThreadId")
                        .HasColumnType("int");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ReplyId")
                        .HasColumnType("int");

                    b.Property<int>("UpvoteCount")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__ForumMes__3214EC0755C27262");

                    b.HasIndex(new[] { "ForumThreadId" }, "IX_ForumMessages_ForumThreadId");

                    b.HasIndex(new[] { "ReplyId" }, "IX_ForumMessages_ReplyId");

                    b.HasIndex(new[] { "UserId" }, "IX_ForumMessages_UserId");

                    b.ToTable("ForumMessages");
                });

            modelBuilder.Entity("TalkHubAPI.Models.ForumModels.ForumThread", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ThreadDescription")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ThreadName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__ForumThr__3214EC07B31A0EE4");

                    b.ToTable("ForumThreads");
                });

            modelBuilder.Entity("TalkHubAPI.Models.ForumModels.UserUpvote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MessageId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__UserUpvo__3214EC076B4006A6");

                    b.HasIndex(new[] { "MessageId" }, "IX_UserUpvotes_MessageId");

                    b.HasIndex(new[] { "UserId" }, "IX_UserUpvotes_UserId");

                    b.ToTable("UserUpvotes");
                });

            modelBuilder.Entity("TalkHubAPI.Models.MessengerModels.MessageRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("RoomName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.HasKey("Id")
                        .HasName("PK__MessageR__3214EC07ED42FBDE");

                    b.ToTable("MessageRooms");
                });

            modelBuilder.Entity("TalkHubAPI.Models.MessengerModels.MessengerMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Messenge__3214EC07417F862B");

                    b.HasIndex(new[] { "RoomId" }, "IX_MessengerMessages_RoomId");

                    b.HasIndex(new[] { "UserId" }, "IX_MessengerMessages_UserId");

                    b.ToTable("MessengerMessages");
                });

            modelBuilder.Entity("TalkHubAPI.Models.MessengerModels.UserMessageRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__UserMess__3214EC07D2F91607");

                    b.HasIndex(new[] { "RoomId" }, "IX_UserMessageRooms_RoomId");

                    b.HasIndex(new[] { "UserId" }, "IX_UserMessageRooms_UserId");

                    b.ToTable("UserMessageRooms");
                });

            modelBuilder.Entity("TalkHubAPI.Models.PhotosManagerModels.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Photos__3214EC07370B195E");

                    b.HasIndex(new[] { "CategoryId" }, "IX_Photos_CategoryId");

                    b.HasIndex(new[] { "UserId" }, "IX_Photos_UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("TalkHubAPI.Models.PhotosManagerModels.PhotoCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__PhotoCat__3214EC072A92E285");

                    b.ToTable("PhotoCategories");
                });

            modelBuilder.Entity("TalkHubAPI.Models.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("TokenCreated")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("TokenExpires")
                        .HasColumnType("datetime");

                    b.HasKey("Id")
                        .HasName("PK__RefreshT__3214EC077AC6B002");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("TalkHubAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("PermissionType")
                        .HasColumnType("int");

                    b.Property<int?>("RefreshTokenId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__User__3214EC07617E5831");

                    b.HasIndex(new[] { "RefreshTokenId" }, "IX_Users_RefreshTokenId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.Playlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PlaylistName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Playlist__3214EC071252C5C2");

                    b.HasIndex(new[] { "UserId" }, "IX_Playlists_UserId");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LikeCount")
                        .HasColumnType("int");

                    b.Property<string>("Mp4name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("MP4Name");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.Property<string>("ThumbnailName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("VideoDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VideoName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK__Videos__3214EC07412B2B44");

                    b.HasIndex(new[] { "TagId" }, "IX_Videos_TagId");

                    b.HasIndex(new[] { "UserId" }, "IX_Videos_UserId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.VideoComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime");

                    b.Property<int>("LikeCount")
                        .HasColumnType("int");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ReplyId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("VideoId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__VideoCom__3214EC0796C7F708");

                    b.HasIndex(new[] { "ReplyId" }, "IX_VideoComments_ReplyId");

                    b.HasIndex(new[] { "UserId" }, "IX_VideoComments_UserId");

                    b.HasIndex(new[] { "VideoId" }, "IX_VideoComments_VideoId");

                    b.ToTable("VideoComments");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.VideoCommentsLike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("VideoCommentId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__VideoCom__3214EC076CC6E6B7");

                    b.HasIndex(new[] { "UserId" }, "IX_VideoCommentsLikes_UserId");

                    b.HasIndex(new[] { "VideoCommentId" }, "IX_VideoCommentsLikes_VideoCommentId");

                    b.ToTable("VideoCommentsLikes");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.VideoPlaylist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PlaylistId")
                        .HasColumnType("int");

                    b.Property<int>("VideoId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__VideoPla__3214EC07F06C00FA");

                    b.HasIndex(new[] { "PlaylistId" }, "IX_VideoPlaylists_PlaylistId");

                    b.HasIndex(new[] { "VideoId" }, "IX_VideoPlaylists_VideoId");

                    b.ToTable("VideoPlaylists");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.VideoTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.HasKey("Id")
                        .HasName("PK__VideoTag__3214EC07AF11C7AA");

                    b.ToTable("VideoTags");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.VideoUserLike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("VideoId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__VideoUse__3214EC07710CAE9C");

                    b.HasIndex(new[] { "UserId" }, "IX_VideoUserLikes_UserId");

                    b.HasIndex(new[] { "VideoId" }, "IX_VideoUserLikes_VideoId");

                    b.ToTable("VideoUserLikes");
                });

            modelBuilder.Entity("TalkHubAPI.Models.ForumModels.ForumMessage", b =>
                {
                    b.HasOne("TalkHubAPI.Models.ForumModels.ForumThread", "ForumThread")
                        .WithMany("ForumMessages")
                        .HasForeignKey("ForumThreadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__ForumMess__Forum__5CD6CB2B");

                    b.HasOne("TalkHubAPI.Models.ForumModels.ForumMessage", "Reply")
                        .WithMany("InverseReply")
                        .HasForeignKey("ReplyId")
                        .HasConstraintName("FK__ForumMess__Reply__5AEE82B9");

                    b.HasOne("TalkHubAPI.Models.User", "User")
                        .WithMany("ForumMessages")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__ForumMess__UserI__5BE2A6F2");

                    b.Navigation("ForumThread");

                    b.Navigation("Reply");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TalkHubAPI.Models.ForumModels.UserUpvote", b =>
                {
                    b.HasOne("TalkHubAPI.Models.ForumModels.ForumMessage", "Message")
                        .WithMany("UserUpvotes")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__UserUpvot__Messa__74AE54BC");

                    b.HasOne("TalkHubAPI.Models.User", "User")
                        .WithMany("UserUpvotes")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__UserUpvot__UserI__73BA3083");

                    b.Navigation("Message");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TalkHubAPI.Models.MessengerModels.MessengerMessage", b =>
                {
                    b.HasOne("TalkHubAPI.Models.MessengerModels.MessageRoom", "Room")
                        .WithMany("MessengerMessages")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Messenger__RoomI__18EBB532");

                    b.HasOne("TalkHubAPI.Models.User", "User")
                        .WithMany("MessengerMessages")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__Messenger__UserI__17F790F9");

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TalkHubAPI.Models.MessengerModels.UserMessageRoom", b =>
                {
                    b.HasOne("TalkHubAPI.Models.MessengerModels.MessageRoom", "Room")
                        .WithMany("UserMessageRooms")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__UserMessa__RoomI__4D5F7D71");

                    b.HasOne("TalkHubAPI.Models.User", "User")
                        .WithMany("UserMessageRooms")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__UserMessa__UserI__4C6B5938");

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TalkHubAPI.Models.PhotosManagerModels.Photo", b =>
                {
                    b.HasOne("TalkHubAPI.Models.PhotosManagerModels.PhotoCategory", "Category")
                        .WithMany("Photos")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Photos_PhotoCategory");

                    b.HasOne("TalkHubAPI.Models.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__Photos__userId__31EC6D26");

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TalkHubAPI.Models.User", b =>
                {
                    b.HasOne("TalkHubAPI.Models.RefreshToken", "RefreshToken")
                        .WithMany("Users")
                        .HasForeignKey("RefreshTokenId")
                        .HasConstraintName("FK_Tokens_Users");

                    b.Navigation("RefreshToken");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.Playlist", b =>
                {
                    b.HasOne("TalkHubAPI.Models.User", "User")
                        .WithMany("Playlists")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__Playlists__UserI__367C1819");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.Video", b =>
                {
                    b.HasOne("TalkHubAPI.Models.VideoPlayerModels.VideoTag", "Tag")
                        .WithMany("Videos")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Videos__TagId__339FAB6E");

                    b.HasOne("TalkHubAPI.Models.User", "User")
                        .WithMany("Videos")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__Videos__UserId__32AB8735");

                    b.Navigation("Tag");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.VideoComment", b =>
                {
                    b.HasOne("TalkHubAPI.Models.VideoPlayerModels.VideoComment", "Reply")
                        .WithMany("InverseReply")
                        .HasForeignKey("ReplyId")
                        .HasConstraintName("FK__VideoComm__Reply__3D2915A8");

                    b.HasOne("TalkHubAPI.Models.User", "User")
                        .WithMany("VideoComments")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__VideoComm__UserI__3E1D39E1");

                    b.HasOne("TalkHubAPI.Models.VideoPlayerModels.Video", "Video")
                        .WithMany("VideoComments")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__VideoComm__Video__3F115E1A");

                    b.Navigation("Reply");

                    b.Navigation("User");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.VideoCommentsLike", b =>
                {
                    b.HasOne("TalkHubAPI.Models.User", "User")
                        .WithMany("VideoCommentsLikes")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__VideoComm__UserI__498EEC8D");

                    b.HasOne("TalkHubAPI.Models.VideoPlayerModels.VideoComment", "VideoComment")
                        .WithMany("VideoCommentsLikes")
                        .HasForeignKey("VideoCommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__VideoComm__Video__489AC854");

                    b.Navigation("User");

                    b.Navigation("VideoComment");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.VideoPlaylist", b =>
                {
                    b.HasOne("TalkHubAPI.Models.VideoPlayerModels.Playlist", "Playlist")
                        .WithMany("VideoPlaylists")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__VideoPlay__Playl__395884C4");

                    b.HasOne("TalkHubAPI.Models.VideoPlayerModels.Video", "Video")
                        .WithMany("VideoPlaylists")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__VideoPlay__Video__3A4CA8FD");

                    b.Navigation("Playlist");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.VideoUserLike", b =>
                {
                    b.HasOne("TalkHubAPI.Models.User", "User")
                        .WithMany("VideoUserLikes")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__VideoUser__UserI__6FE99F9F");

                    b.HasOne("TalkHubAPI.Models.VideoPlayerModels.Video", "Video")
                        .WithMany("VideoUserLikes")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__VideoUser__Video__70DDC3D8");

                    b.Navigation("User");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("TalkHubAPI.Models.ForumModels.ForumMessage", b =>
                {
                    b.Navigation("InverseReply");

                    b.Navigation("UserUpvotes");
                });

            modelBuilder.Entity("TalkHubAPI.Models.ForumModels.ForumThread", b =>
                {
                    b.Navigation("ForumMessages");
                });

            modelBuilder.Entity("TalkHubAPI.Models.MessengerModels.MessageRoom", b =>
                {
                    b.Navigation("MessengerMessages");

                    b.Navigation("UserMessageRooms");
                });

            modelBuilder.Entity("TalkHubAPI.Models.PhotosManagerModels.PhotoCategory", b =>
                {
                    b.Navigation("Photos");
                });

            modelBuilder.Entity("TalkHubAPI.Models.RefreshToken", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("TalkHubAPI.Models.User", b =>
                {
                    b.Navigation("ForumMessages");

                    b.Navigation("MessengerMessages");

                    b.Navigation("Photos");

                    b.Navigation("Playlists");

                    b.Navigation("UserMessageRooms");

                    b.Navigation("UserUpvotes");

                    b.Navigation("VideoComments");

                    b.Navigation("VideoCommentsLikes");

                    b.Navigation("VideoUserLikes");

                    b.Navigation("Videos");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.Playlist", b =>
                {
                    b.Navigation("VideoPlaylists");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.Video", b =>
                {
                    b.Navigation("VideoComments");

                    b.Navigation("VideoPlaylists");

                    b.Navigation("VideoUserLikes");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.VideoComment", b =>
                {
                    b.Navigation("InverseReply");

                    b.Navigation("VideoCommentsLikes");
                });

            modelBuilder.Entity("TalkHubAPI.Models.VideoPlayerModels.VideoTag", b =>
                {
                    b.Navigation("Videos");
                });
#pragma warning restore 612, 618
        }
    }
}
