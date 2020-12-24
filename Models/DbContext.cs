using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Models
{
    public class ChatContext : IdentityDbContext<ApplicationUser>
    {
        public ChatContext(DbContextOptions options):base(options){
        }
        public DbSet<PrivateChat> Chats{get;set;}
        public DbSet<Media> Medias{get;set;}
        public DbSet<RefreshToken> refreshTokens{get;set;}
        public DbSet<PrivateChat> PrivateChats{get;set;}
        public DbSet<ChatMap> ChatMaps{get;set;}
        public DbSet<ServerChannelMap> serverChannelMaps{get;set;}
        public DbSet<GroupMap> GroupMaps{get;set;}
        public DbSet<MessageChannel> messageChannels{get;set;}
        public DbSet<ServerChannel> ServerChannels{get;set;}


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Setting Many to Many Relation between Application User to MessageChannel
            builder.Entity<GroupMap>().HasKey(ex => new{ex.UserId,ex.MessageChannelKey,ex.Key});
            builder.Entity<GroupMap>().HasOne(x => x.user).WithMany(x => x.groups).HasForeignKey(x => x.UserId);
            builder.Entity<GroupMap>().HasOne(x => x.messageChannel).WithMany(x => x.groupMap).HasForeignKey(x => x.MessageChannelKey);
            //Setting Many to Many Relation between Application User to ServerChannel
            builder.Entity<ServerChannelMap>().HasKey(ex => new{ex.UserId,ex.ServerChannelKey,ex.Key});
            builder.Entity<ServerChannelMap>().HasOne(x => x.user).WithMany(x => x.servers).HasForeignKey(x => x.UserId);
            builder.Entity<ServerChannelMap>().HasOne(x => x.server).WithMany(x => x.ServerChannelMaps).HasForeignKey(x => x.ServerChannelKey);
           //Setting up many to many relation  
           builder.Entity<ChatMap>().HasKey(ex =>  new {ex.UserId,ex.ChatId,ex.Key});
           builder.Entity<ChatMap>().HasOne(x => x.chat).WithMany(x => x.chatMaps).HasForeignKey(x => x.ChatId);
           builder.Entity<ChatMap>().HasOne(x => x.user).WithMany(x => x.chats).HasForeignKey(x => x.UserId);
           //One to many relationship with from Application user to Medias
           builder.Entity<Media>().HasOne<ApplicationUser>(usr => usr.User).WithMany(p => p.Medias ).HasForeignKey(cht => cht.SendFrom);
           // One to many relationship from Privatechats to Media
          builder.Entity<Media>().HasOne<PrivateChat>(pri => pri.Chat).WithMany(pri => pri.Media).HasForeignKey(s => s.ChatId);
          base.OnModelCreating(builder);
        }

        

    }
}