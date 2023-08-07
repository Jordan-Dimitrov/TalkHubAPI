﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TalkHubAPI.Data;

#nullable disable

namespace TalkHubAPI.Migrations
{
    [DbContext(typeof(TalkHubContext))]
    partial class TalkHubContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TalkHubAPI.Models.Photo", b =>
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
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime");

                    b.HasKey("Id")
                        .HasName("PK__Photos__3214EC07370B195E");

                    b.HasIndex("CategoryId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("TalkHubAPI.Models.PhotoCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PhotoName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__PhotoCat__3214EC072A92E285");

                    b.ToTable("PhotoCategory", (string)null);
                });

            modelBuilder.Entity("TalkHubAPI.Models.PhotosUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PhotoId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__PhotosUs__3214EC079F452C94");

                    b.ToTable("PhotosUsers");
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

                    b.HasIndex("RefreshTokenId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("TalkHubAPI.Models.Photo", b =>
                {
                    b.HasOne("TalkHubAPI.Models.PhotoCategory", "Category")
                        .WithMany("Photos")
                        .HasForeignKey("CategoryId")
                        .IsRequired()
                        .HasConstraintName("FK_Photos_PhotoCategory");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("TalkHubAPI.Models.User", b =>
                {
                    b.HasOne("TalkHubAPI.Models.RefreshToken", "RefreshToken")
                        .WithMany("Users")
                        .HasForeignKey("RefreshTokenId")
                        .HasConstraintName("FK_Tokens_Users");

                    b.Navigation("RefreshToken");
                });

            modelBuilder.Entity("TalkHubAPI.Models.PhotoCategory", b =>
                {
                    b.Navigation("Photos");
                });

            modelBuilder.Entity("TalkHubAPI.Models.RefreshToken", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
