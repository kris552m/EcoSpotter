using Microsoft.EntityFrameworkCore;
using EcoSpotterBackend.Model;
namespace EcoSpotterBackend.Persistence
{
    public class ecoDBContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public ecoDBContext(DbContextOptions<ecoDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Profile entity configuration
            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(p => p.Id);

                // One Profile has many Posts
                entity.HasMany(p => p.Posts)
                      .WithOne(p => p.AuthorProfile)
                      .HasForeignKey(p => p.UserId)
                      .IsRequired();
            });

            // Post entity configuration
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.PostImage)
                        .WithOne(i => i.Post)
                        .HasForeignKey<PostImage>(p => p.PostId)
                        .IsRequired();
            });

            modelBuilder.Entity<PostImage>(entity =>
            {
                entity.HasKey(i => i.Id);
            });

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
