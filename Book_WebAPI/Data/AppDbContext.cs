using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Book_WebAPI.Models;
using Book_WebAPI.Models.Domain;
namespace Book_WebAPI.Data
{
    public class AppDbContext:DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Models.Domain.Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BookAuthor>()
                .HasOne(b => b.Book)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(bi => bi.BookId);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(b => b.Author)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(bi => bi.AuthorId);

            modelBuilder.Entity<Book>()
               .HasOne(x => x.Publisher)
               .WithMany(x => x.Books);

            new DbInitializer(modelBuilder).Seed();
        }
    }
}
