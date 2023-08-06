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

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
