﻿// <auto-generated />
using System;
using ChatApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChatApp.Migrations
{
    [DbContext(typeof(ChatContext))]
    [Migration("20201224221752_AddedTables")]
    partial class AddedTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("ChatApp.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConnectedUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ChatApp.Models.ChatMap", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ChatId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("UserId_2")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "ChatId", "Key");

                    b.HasIndex("ChatId");

                    b.ToTable("ChatMaps");
                });

            modelBuilder.Entity("ChatApp.Models.GroupMap", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MessageChannelKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("ServerName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "MessageChannelKey", "Key");

                    b.HasIndex("MessageChannelKey");

                    b.ToTable("GroupMaps");
                });

            modelBuilder.Entity("ChatApp.Models.Media", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ChatId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageChannelKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("SendFrom")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SendFromEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TimeUtc")
                        .HasColumnType("bigint");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Key");

                    b.HasIndex("ChatId");

                    b.HasIndex("MessageChannelKey");

                    b.HasIndex("SendFrom");

                    b.ToTable("Medias");
                });

            modelBuilder.Entity("ChatApp.Models.MessageChannel", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("ServerKey")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Key");

                    b.HasIndex("ServerKey");

                    b.ToTable("messageChannels");
                });

            modelBuilder.Entity("ChatApp.Models.PrivateChat", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Key");

                    b.ToTable("PrivateChat");
                });

            modelBuilder.Entity("ChatApp.Models.RefreshToken", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsValid")
                        .HasColumnType("bit");

                    b.Property<string>("JwtToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("UserKey")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Key");

                    b.HasIndex("UserKey");

                    b.ToTable("refreshTokens");
                });

            modelBuilder.Entity("ChatApp.Models.ServerChannel", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Key");

                    b.ToTable("ServerChannels");
                });

            modelBuilder.Entity("ChatApp.Models.ServerChannelMap", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ServerChannelKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("UserId", "ServerChannelKey", "Key");

                    b.HasIndex("ServerChannelKey");

                    b.ToTable("serverChannelMaps");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ChatApp.Models.ChatMap", b =>
                {
                    b.HasOne("ChatApp.Models.PrivateChat", "chat")
                        .WithMany("chatMaps")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatApp.Models.ApplicationUser", "user")
                        .WithMany("chats")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("chat");

                    b.Navigation("user");
                });

            modelBuilder.Entity("ChatApp.Models.GroupMap", b =>
                {
                    b.HasOne("ChatApp.Models.MessageChannel", "messageChannel")
                        .WithMany("groupMap")
                        .HasForeignKey("MessageChannelKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatApp.Models.ApplicationUser", "user")
                        .WithMany("groups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("messageChannel");

                    b.Navigation("user");
                });

            modelBuilder.Entity("ChatApp.Models.Media", b =>
                {
                    b.HasOne("ChatApp.Models.PrivateChat", "Chat")
                        .WithMany("Media")
                        .HasForeignKey("ChatId");

                    b.HasOne("ChatApp.Models.MessageChannel", "messageChannel")
                        .WithMany("Medias")
                        .HasForeignKey("MessageChannelKey");

                    b.HasOne("ChatApp.Models.ApplicationUser", "User")
                        .WithMany("Medias")
                        .HasForeignKey("SendFrom");

                    b.Navigation("Chat");

                    b.Navigation("messageChannel");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChatApp.Models.MessageChannel", b =>
                {
                    b.HasOne("ChatApp.Models.ServerChannel", "server")
                        .WithMany("MessageChannels")
                        .HasForeignKey("ServerKey");

                    b.Navigation("server");
                });

            modelBuilder.Entity("ChatApp.Models.RefreshToken", b =>
                {
                    b.HasOne("ChatApp.Models.ApplicationUser", "User")
                        .WithMany("refreshTokens")
                        .HasForeignKey("UserKey");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChatApp.Models.ServerChannelMap", b =>
                {
                    b.HasOne("ChatApp.Models.ServerChannel", "server")
                        .WithMany("ServerChannelMaps")
                        .HasForeignKey("ServerChannelKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatApp.Models.ApplicationUser", "user")
                        .WithMany("servers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("server");

                    b.Navigation("user");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ChatApp.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ChatApp.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatApp.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ChatApp.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ChatApp.Models.ApplicationUser", b =>
                {
                    b.Navigation("chats");

                    b.Navigation("groups");

                    b.Navigation("Medias");

                    b.Navigation("refreshTokens");

                    b.Navigation("servers");
                });

            modelBuilder.Entity("ChatApp.Models.MessageChannel", b =>
                {
                    b.Navigation("groupMap");

                    b.Navigation("Medias");
                });

            modelBuilder.Entity("ChatApp.Models.PrivateChat", b =>
                {
                    b.Navigation("chatMaps");

                    b.Navigation("Media");
                });

            modelBuilder.Entity("ChatApp.Models.ServerChannel", b =>
                {
                    b.Navigation("MessageChannels");

                    b.Navigation("ServerChannelMaps");
                });
#pragma warning restore 612, 618
        }
    }
}
