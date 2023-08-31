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

    public virtual DbSet<ForumUpvote> ForumUpvotes { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<PhotoCategory> PhotoCategories { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

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

        modelBuilder.Entity<ForumUpvote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ForumUpv__3214EC07226852B3");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
