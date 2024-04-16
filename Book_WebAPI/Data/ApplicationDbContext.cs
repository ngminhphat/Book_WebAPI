using Book_WebAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace Book_WebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publishers> Publishers { get; set; }
        public DbSet<Book_WebAPIs> Book_WebAPIss { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book_WebAPIs>()
                .HasKey(ba => new { ba.BookID, ba.AuthorID });

            modelBuilder.Entity<Book_WebAPIs>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.Book_WebAPIs)
                .HasForeignKey(ba => ba.BookID);

            modelBuilder.Entity<Book_WebAPIs>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.Book_WebAPIs)
                .HasForeignKey(ba => ba.AuthorID);
        }
    }
}