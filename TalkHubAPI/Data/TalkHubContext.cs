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

    public virtual DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0725B705F6");

            entity.ToTable("User");

            entity.Property(e => e.RefreshToken).HasMaxLength(255);
            entity.Property(e => e.TokenCreated).HasColumnType("datetime");
            entity.Property(e => e.TokenExpires).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
