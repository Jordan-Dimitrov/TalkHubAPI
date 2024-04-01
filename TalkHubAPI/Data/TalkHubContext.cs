using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.MessengerModels;
using TalkHubAPI.Models.PhotosManagerModels;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Data;

public partial class TalkHubContext : DbContext
{
    public TalkHubContext()
    {
    }

    public TalkHubContext(DbContextOptions<TalkHubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ForumMessage> ForumMessages { get; set; }

    public virtual DbSet<ForumThread> ForumThreads { get; set; }

    public virtual DbSet<MessageRoom> MessageRooms { get; set; }

    public virtual DbSet<MessengerMessage> MessengerMessages { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<PhotoCategory> PhotoCategories { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserMessageRoom> UserMessageRooms { get; set; }

    public virtual DbSet<UserSubscribtion> UserSubscribtions { get; set; }

    public virtual DbSet<UserUpvote> UserUpvotes { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<VideoComment> VideoComments { get; set; }

    public virtual DbSet<VideoCommentsLike> VideoCommentsLikes { get; set; }

    public virtual DbSet<VideoPlaylist> VideoPlaylists { get; set; }

    public virtual DbSet<VideoTag> VideoTags { get; set; }

    public virtual DbSet<VideoUserLike> VideoUserLikes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ForumMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ForumMes__3214EC0755C27262");

            entity.HasIndex(e => e.ForumThreadId, "IX_ForumMessages_ForumThreadId");

            entity.HasIndex(e => e.ReplyId, "IX_ForumMessages_ReplyId");

            entity.HasIndex(e => e.UserId, "IX_ForumMessages_UserId");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.MessageContent).HasMaxLength(1000);

            entity.HasOne(d => d.ForumThread).WithMany(p => p.ForumMessages)
                .HasForeignKey(d => d.ForumThreadId)
                .HasConstraintName("FK__ForumMess__Forum__5CD6CB2B");

            entity.HasOne(d => d.Reply).WithMany(p => p.InverseReply)
                .HasForeignKey(d => d.ReplyId)
                .HasConstraintName("FK__ForumMess__Reply__5AEE82B9");

            entity.HasOne(d => d.User).WithMany(p => p.ForumMessages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ForumMess__UserI__5BE2A6F2");
        });

        modelBuilder.Entity<ForumThread>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ForumThr__3214EC07B31A0EE4");

            entity.Property(e => e.ThreadDescription).HasMaxLength(255);
            entity.Property(e => e.ThreadName).HasMaxLength(30);
        });

        modelBuilder.Entity<MessageRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MessageR__3214EC07ED42FBDE");

            entity.Property(e => e.RoomName).HasMaxLength(45);
        });

        modelBuilder.Entity<MessengerMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messenge__3214EC07417F862B");

            entity.HasIndex(e => e.RoomId, "IX_MessengerMessages_RoomId");

            entity.HasIndex(e => e.UserId, "IX_MessengerMessages_UserId");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.MessageContent).HasMaxLength(255);

            entity.HasOne(d => d.Room).WithMany(p => p.MessengerMessages)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__Messenger__RoomI__18EBB532");

            entity.HasOne(d => d.User).WithMany(p => p.MessengerMessages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messenger__UserI__17F790F9");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Photos__3214EC07370B195E");

            entity.HasIndex(e => e.CategoryId, "IX_Photos_CategoryId");

            entity.HasIndex(e => e.UserId, "IX_Photos_UserId");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Photos)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Photos_PhotoCategory");

            entity.HasOne(d => d.User).WithMany(p => p.Photos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Photos__userId__31EC6D26");
        });

        modelBuilder.Entity<PhotoCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhotoCat__3214EC072A92E285");

            entity.Property(e => e.CategoryName).HasMaxLength(30);
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Playlist__3214EC071252C5C2");

            entity.HasIndex(e => e.UserId, "IX_Playlists_UserId");

            entity.Property(e => e.PlaylistName).HasMaxLength(30);

            entity.HasOne(d => d.User).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Playlists__UserI__367C1819");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC077AC6B002");

            entity.Property(e => e.TokenCreated).HasColumnType("datetime");
            entity.Property(e => e.TokenExpires).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07617E5831");

            entity.HasIndex(e => e.RefreshTokenId, "IX_Users_RefreshTokenId");

            entity.Property(e => e.Email).HasDefaultValue("");
            entity.Property(e => e.ResetTokenExpires).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(30);
            entity.Property(e => e.VerifiedAt).HasColumnType("datetime");

            entity.HasOne(d => d.RefreshToken).WithMany(p => p.Users)
                .HasForeignKey(d => d.RefreshTokenId)
                .HasConstraintName("FK_Tokens_Users");
        });

        modelBuilder.Entity<UserMessageRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserMess__3214EC07D2F91607");

            entity.HasIndex(e => e.RoomId, "IX_UserMessageRooms_RoomId");

            entity.HasIndex(e => e.UserId, "IX_UserMessageRooms_UserId");

            entity.HasOne(d => d.Room).WithMany(p => p.UserMessageRooms)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__UserMessa__RoomI__4D5F7D71");

            entity.HasOne(d => d.User).WithMany(p => p.UserMessageRooms)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserMessa__UserI__4C6B5938");
        });

        modelBuilder.Entity<UserSubscribtion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserSubs__3214EC07DD7515EE");

            entity.HasOne(d => d.UserChannel).WithMany(p => p.UserSubscribtionUserChannels)
                .HasForeignKey(d => d.UserChannelId)
                .HasConstraintName("FK__UserSubsc__UserC__2739D489");

            entity.HasOne(d => d.UserSubscriber).WithMany(p => p.UserSubscribtionUserSubscribers)
                .HasForeignKey(d => d.UserSubscriberId)
                .HasConstraintName("FK__UserSubsc__UserS__282DF8C2");
        });

        modelBuilder.Entity<UserUpvote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserUpvo__3214EC076B4006A6");

            entity.HasIndex(e => e.MessageId, "IX_UserUpvotes_MessageId");

            entity.HasIndex(e => e.UserId, "IX_UserUpvotes_UserId");

            entity.HasOne(d => d.Message).WithMany(p => p.UserUpvotes)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK__UserUpvot__Messa__74AE54BC");

            entity.HasOne(d => d.User).WithMany(p => p.UserUpvotes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserUpvot__UserI__73BA3083");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Videos__3214EC07412B2B44");

            entity.HasIndex(e => e.TagId, "IX_Videos_TagId");

            entity.HasIndex(e => e.UserId, "IX_Videos_UserId");

            entity.Property(e => e.Mp4name).HasColumnName("MP4Name");
            entity.Property(e => e.VideoDescription).HasMaxLength(255);
            entity.Property(e => e.VideoName).HasMaxLength(30);

            entity.HasOne(d => d.Tag).WithMany(p => p.Videos)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK__Videos__TagId__339FAB6E");

            entity.HasOne(d => d.User).WithMany(p => p.Videos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Videos__UserId__32AB8735");
        });

        modelBuilder.Entity<VideoComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VideoCom__3214EC0796C7F708");

            entity.HasIndex(e => e.ReplyId, "IX_VideoComments_ReplyId");

            entity.HasIndex(e => e.UserId, "IX_VideoComments_UserId");

            entity.HasIndex(e => e.VideoId, "IX_VideoComments_VideoId");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.MessageContent).HasMaxLength(1000);

            entity.HasOne(d => d.Reply).WithMany(p => p.InverseReply)
                .HasForeignKey(d => d.ReplyId)
                .HasConstraintName("FK__VideoComm__Reply__3D2915A8");

            entity.HasOne(d => d.User).WithMany(p => p.VideoComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoComm__UserI__3E1D39E1");

            entity.HasOne(d => d.Video).WithMany(p => p.VideoComments)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("FK__VideoComm__Video__3F115E1A");
        });

        modelBuilder.Entity<VideoCommentsLike>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VideoCom__3214EC076CC6E6B7");

            entity.HasIndex(e => e.UserId, "IX_VideoCommentsLikes_UserId");

            entity.HasIndex(e => e.VideoCommentId, "IX_VideoCommentsLikes_VideoCommentId");

            entity.HasOne(d => d.User).WithMany(p => p.VideoCommentsLikes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoComm__UserI__498EEC8D");

            entity.HasOne(d => d.VideoComment).WithMany(p => p.VideoCommentsLikes)
                .HasForeignKey(d => d.VideoCommentId)
                .HasConstraintName("FK__VideoComm__Video__489AC854");
        });

        modelBuilder.Entity<VideoPlaylist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VideoPla__3214EC07F06C00FA");

            entity.HasIndex(e => e.PlaylistId, "IX_VideoPlaylists_PlaylistId");

            entity.HasIndex(e => e.VideoId, "IX_VideoPlaylists_VideoId");

            entity.HasOne(d => d.Playlist).WithMany(p => p.VideoPlaylists)
                .HasForeignKey(d => d.PlaylistId)
                .HasConstraintName("FK__VideoPlay__Playl__395884C4");

            entity.HasOne(d => d.Video).WithMany(p => p.VideoPlaylists)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("FK__VideoPlay__Video__3A4CA8FD");
        });

        modelBuilder.Entity<VideoTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VideoTag__3214EC07AF11C7AA");

            entity.Property(e => e.TagName).HasMaxLength(30);
        });

        modelBuilder.Entity<VideoUserLike>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VideoUse__3214EC07710CAE9C");

            entity.HasIndex(e => e.UserId, "IX_VideoUserLikes_UserId");

            entity.HasIndex(e => e.VideoId, "IX_VideoUserLikes_VideoId");

            entity.HasOne(d => d.User).WithMany(p => p.VideoUserLikes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoUser__UserI__6FE99F9F");

            entity.HasOne(d => d.Video).WithMany(p => p.VideoUserLikes)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("FK__VideoUser__Video__70DDC3D8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
