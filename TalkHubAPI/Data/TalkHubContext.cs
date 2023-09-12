using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Models;

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

    public virtual DbSet<UserRoom> UserRooms { get; set; }

    public virtual DbSet<UserUpvote> UserUpvotes { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<VideoComment> VideoComments { get; set; }

    public virtual DbSet<VideoCommentsLike> VideoCommentsLikes { get; set; }

    public virtual DbSet<VideoPlaylist> VideoPlaylists { get; set; }

    public virtual DbSet<VideoTag> VideoTags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TalkHub;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ForumMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ForumMes__3214EC0755C27262");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.FileName).IsUnicode(false);
            entity.Property(e => e.MessageContent).IsUnicode(false);

            entity.HasOne(d => d.ForumThread).WithMany(p => p.ForumMessages)
                .HasForeignKey(d => d.ForumThreadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
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

            entity.Property(e => e.ThreadDescription)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.ThreadName)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MessageRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MessageR__3214EC07ED42FBDE");

            entity.Property(e => e.RoomName)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MessengerMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messenge__3214EC07417F862B");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.FileName).IsUnicode(false);
            entity.Property(e => e.MessageContent).IsUnicode(false);

            entity.HasOne(d => d.Room).WithMany(p => p.MessengerMessages)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__Messenger__RoomI__18EBB532");

            entity.HasOne(d => d.User).WithMany(p => p.MessengerMessages)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Messenger__UserI__17F790F9");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Photos__3214EC07370B195E");

            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Photos)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Photos_PhotoCategory");

            entity.HasOne(d => d.User).WithMany(p => p.Photos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Photos__userId__31EC6D26");
        });

        modelBuilder.Entity<PhotoCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhotoCat__3214EC072A92E285");

            entity.Property(e => e.CategoryName).HasMaxLength(255);
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Playlist__3214EC071252C5C2");

            entity.Property(e => e.PlaylistName).IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Playlists__UserI__367C1819");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC077AC6B002");

            entity.Property(e => e.Token).HasMaxLength(255);
            entity.Property(e => e.TokenCreated).HasColumnType("datetime");
            entity.Property(e => e.TokenExpires).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07617E5831");

            entity.Property(e => e.Username).HasMaxLength(255);

            entity.HasOne(d => d.RefreshToken).WithMany(p => p.Users)
                .HasForeignKey(d => d.RefreshTokenId)
                .HasConstraintName("FK_Tokens_Users");
        });

        modelBuilder.Entity<UserMessageRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserMess__3214EC07FF3707BC");

            entity.HasOne(d => d.Room).WithMany(p => p.UserMessageRooms)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserMessa__RoomI__29221CFB");

            entity.HasOne(d => d.User).WithMany(p => p.UserMessageRooms)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserMessa__UserI__2A164134");
        });

        modelBuilder.Entity<UserRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRoom__3214EC07D16F1851");

            entity.HasOne(d => d.Room).WithMany(p => p.UserRooms)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRooms__RoomI__2FCF1A8A");

            entity.HasOne(d => d.User).WithMany(p => p.UserRooms)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRooms__UserI__2EDAF651");
        });

        modelBuilder.Entity<UserUpvote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserUpvo__3214EC076B4006A6");

            entity.HasOne(d => d.Message).WithMany(p => p.UserUpvotes)
                .HasForeignKey(d => d.MessageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserUpvot__Messa__74AE54BC");

            entity.HasOne(d => d.User).WithMany(p => p.UserUpvotes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserUpvot__UserI__73BA3083");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Videos__3214EC07412B2B44");

            entity.Property(e => e.Mp4name)
                .IsUnicode(false)
                .HasColumnName("MP4Name");
            entity.Property(e => e.ThumbnailName).IsUnicode(false);
            entity.Property(e => e.VideoDescription).IsUnicode(false);
            entity.Property(e => e.VideoName).IsUnicode(false);

            entity.HasOne(d => d.Tag).WithMany(p => p.Videos)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Videos__TagId__339FAB6E");

            entity.HasOne(d => d.User).WithMany(p => p.Videos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Videos__UserId__32AB8735");
        });

        modelBuilder.Entity<VideoComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VideoCom__3214EC0796C7F708");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.MessageContent).IsUnicode(false);

            entity.HasOne(d => d.Reply).WithMany(p => p.InverseReply)
                .HasForeignKey(d => d.ReplyId)
                .HasConstraintName("FK__VideoComm__Reply__3D2915A8");

            entity.HasOne(d => d.User).WithMany(p => p.VideoComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoComm__UserI__3E1D39E1");

            entity.HasOne(d => d.Video).WithMany(p => p.VideoComments)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoComm__Video__3F115E1A");
        });

        modelBuilder.Entity<VideoCommentsLike>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VideoCom__3214EC0711BA0C9B");

            entity.HasOne(d => d.VideoComment).WithMany(p => p.VideoCommentsLikes)
                .HasForeignKey(d => d.VideoCommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoComm__Video__41EDCAC5");
        });

        modelBuilder.Entity<VideoPlaylist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VideoPla__3214EC07F06C00FA");

            entity.HasOne(d => d.Playlist).WithMany(p => p.VideoPlaylists)
                .HasForeignKey(d => d.PlaylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoPlay__Playl__395884C4");

            entity.HasOne(d => d.Video).WithMany(p => p.VideoPlaylists)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoPlay__Video__3A4CA8FD");
        });

        modelBuilder.Entity<VideoTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VideoTag__3214EC07AF11C7AA");

            entity.Property(e => e.TagName)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
