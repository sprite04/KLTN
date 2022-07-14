using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure
{
    public class RealEstateDbContext:IdentityDbContext
    {
        public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) : base(options) { }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<FavoritePost> FavoritePosts { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostType> PostTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<VerifyPhone> VerifyPhones { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        //public DbSet<ReportType> ReportTypes { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportProcessing> ReportProcessings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Keyword> Keywords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FavoritePost>(entity =>
            {
                entity.ToTable("FavoritePost");
                entity.HasKey(e => new { e.UserID, e.PostID });

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.FavoritePosts)
                    .HasForeignKey(d => d.PostID)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoritePosts)
                    .HasForeignKey(d => d.UserID)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasOne(d => d.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.ClientCascade);
            });


            modelBuilder.Entity<Follow>(entity =>
            {
                entity.ToTable("Follow");
                entity.HasKey(e => new { e.FollowID, e.FollowedID });
                entity.HasOne(d => d.FollowUser)
                    .WithMany(p => p.FollowUsers)
                    .HasForeignKey(d => d.FollowID)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(d => d.FollowedUser)
                    .WithMany(p => p.FollowedUsers)
                    .HasForeignKey(d => d.FollowedID)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.ImageUrl).IsUnicode(false);
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.PostID);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Address).HasColumnType("nvarchar(200)");
                entity.Property(e => e.Title).HasColumnType("nvarchar(200)");
                entity.Property(e => e.Details).HasColumnType("ntext");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.Property(e => e.IsSold).HasDefaultValue(false);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CreatorID);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CategoryID);
                
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.Title).HasColumnType("nvarchar(300)");
                entity.Property(e => e.Details).HasColumnType("ntext");
                entity.Property(e => e.Display).HasDefaultValue(false);
                entity.Property(e => e.CreatedDate).HasDefaultValue(DateTime.Now);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.CreatorID)
                    .OnDelete(DeleteBehavior.ClientCascade);

            });

            modelBuilder.Entity<Keyword>(entity =>
            {
                entity.Property(e => e.StrKeywords).HasColumnType("ntext");

                entity.HasOne(d => d.User)
                .WithOne(p => p.Keywords)
                .HasForeignKey<Keyword>(d => d.UserID)
                .OnDelete(DeleteBehavior.ClientCascade);
            });

            //------------------------------------khong xoa--------------------------------------
            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");

                entity.Property(e => e.Details).HasColumnType("ntext");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.PostID)
                    .OnDelete(DeleteBehavior.ClientCascade);

                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.Reports)
                //    .HasForeignKey(d => d.UserID)
                //    .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<ReportProcessing>(entity =>
            {
                entity.ToTable("ReportProcessing");
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.ReportProcessings)
                    .HasForeignKey(d => d.PostID)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });
        }
    }
}

