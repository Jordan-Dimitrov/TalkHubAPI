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

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<PhotoCategory> PhotoCategories { get; set; }

    public virtual DbSet<PhotosUser> PhotosUsers { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TalkHub;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Photos__3214EC07370B195E");

            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(255);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Photos)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Photos_PhotoCategory");
        });

        modelBuilder.Entity<PhotoCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhotoCat__3214EC072A92E285");

            entity.ToTable("PhotoCategory");

            entity.Property(e => e.PhotoName).HasMaxLength(255);
        });

        modelBuilder.Entity<PhotosUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhotosUs__3214EC079F452C94");
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

            entity.ToTable("User");

            entity.Property(e => e.Username).HasMaxLength(255);

            entity.HasOne(d => d.RefreshToken).WithMany(p => p.Users)
                .HasForeignKey(d => d.RefreshTokenId)
                .HasConstraintName("FK_Tokens_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
