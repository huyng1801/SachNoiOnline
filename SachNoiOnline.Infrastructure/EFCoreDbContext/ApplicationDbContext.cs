using Microsoft.EntityFrameworkCore;
using SachNoiOnline.Domain.Entities;
using System;

namespace SachNoiOnline.Infrastructure.EFCoreDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Story> Stories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Audio> Audios { get; set; }
        public DbSet<Narrator> Narrators { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureAccountEntity(modelBuilder);
            ConfigureAuthorEntity(modelBuilder);
            ConfigureCategoryEntity(modelBuilder);
            ConfigureNarratorEntity(modelBuilder);
            ConfigureStoryEntity(modelBuilder);
            ConfigureAudioEntity(modelBuilder);
            ConfigureRatingEntity(modelBuilder);
        }

        private void ConfigureAccountEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Username)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(a => a.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(a => a.PasswordHash)
                    .IsRequired();

                entity.Property(a => a.Role)
                    .IsRequired();

                entity.Property(a => a.DeletedAt)
                    .IsRequired(false);

            });
        }

        private void ConfigureAuthorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.AuthorName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(a => a.DeletedAt)
                    .IsRequired(false);

            });
        }

        private void ConfigureCategoryEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.CategoryName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(c => c.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(c => c.UpdatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(c => c.DeletedAt)
                    .IsRequired(false);
            });
        }

        private void ConfigureNarratorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Narrator>(entity =>
            {
                entity.HasKey(n => n.Id);

                entity.Property(n => n.NarratorName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(n => n.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(n => n.UpdatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(n => n.DeletedAt)
                    .IsRequired(false);


                entity.HasMany(n => n.Stories)
                    .WithOne(s => s.Narrator)
                    .HasForeignKey(s => s.NarratorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureStoryEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Story>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(s => s.CoverImageUrl)
                    .HasMaxLength(500);

                entity.Property(s => s.Description)
                    .HasMaxLength(1000);

                entity.Property(s => s.ListenersCount)
                    .HasDefaultValue(0);

                entity.Property(s => s.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(s => s.UpdatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(s => s.DeletedAt)
                    .IsRequired(false);


                entity.HasOne(s => s.Author)
                    .WithMany(a => a.Stories)
                    .HasForeignKey(s => s.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.Category)
                    .WithMany(c => c.Stories)
                    .HasForeignKey(s => s.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.Narrator)
                    .WithMany(n => n.Stories)
                    .HasForeignKey(s => s.NarratorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(s => s.Audios)
                    .WithOne(a => a.Story)
                    .HasForeignKey(a => a.StoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(s => s.Ratings)
                    .WithOne(r => r.Story)
                    .HasForeignKey(r => r.StoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureAudioEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Audio>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(a => a.AudioFileUrl)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(a => a.Duration)
                    .HasDefaultValue(0);

                entity.Property(a => a.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(a => a.UpdatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(a => a.DeletedAt)
                    .IsRequired(false);

                entity.HasOne(a => a.Story)
                    .WithMany(s => s.Audios)
                    .HasForeignKey(a => a.StoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureRatingEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.RatingValue)
                    .IsRequired();

                entity.Property(r => r.Comment)
                    .HasMaxLength(1000);

                entity.Property(r => r.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(r => r.UpdatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(r => r.DeletedAt)
                    .IsRequired(false);

                entity.HasOne(r => r.Account)
                    .WithMany(u => u.Ratings)
                    .HasForeignKey(r => r.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Story)
                    .WithMany(s => s.Ratings)
                    .HasForeignKey(r => r.StoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
