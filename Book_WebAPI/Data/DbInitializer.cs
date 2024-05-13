using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Book_WebAPI.Models;
using Book_WebAPI.Models.Domain;
using static System.Net.WebRequestMethods;

namespace Book_WebAPI.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder _builder;
        public DbInitializer(ModelBuilder builder)
        {
            _builder = builder;
        }
        public void Seed()
        {
            _builder.Entity<Book>(a =>
            {
                a.HasData(new Book
                {
                    BookId = 1,
                    Title = "Dark Nhan Tamn",
                    Description = "trending book",
                    IsRead = true,
                    Rate = 10,
                    Genre = 1,
                    CoverUrl = "https://nxbhcm.com.vn/Image/Biasach/dacnhantam86.jpg",
                    DateAdded = DateTime.Now,
                    PublisherID = 1,

                }); 
                a.HasData(new Book
                {
                    BookId = 2,
                    Title = "Lập trình Web MVC",
                    Description = "MVC",
                    IsRead = true,
                    Rate = 10,
                    Genre = 2,
                    CoverUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcReihuPgO3ty4cK9ePdAuMETwnAReeBLhEFNObJQCnDnmhqeEP9",
                    DateAdded = DateTime.Now,
                    PublisherID = 2
                });
                a.HasData(new Book
                {
                    BookId = 3,
                    Title = "Hướng đối tượng (OOP)",
                    Description = "oop",
                    IsRead = true,
                    Rate = 10,
                    Genre = 1,
                    CoverUrl = "https://m.media-amazon.com/images/I/51OV+q4yBkS._AC_UF1000,1000_QL80_DpWeblab_.jpg",
                    PublisherID = 3
                }); 
            });

            _builder.Entity<Book_WebAPI.Models.Domain.Author>(b =>
            {
                b.HasData(new Book_WebAPI.Models.Domain.Author
                {
                    AuthorID = 1,
                    AuthorName = "Phat"
                });
                b.HasData(new Book_WebAPI.Models.Domain.Author
                {
                    AuthorID = 2,
                    AuthorName = "Dat"
                });
                b.HasData(new Book_WebAPI.Models.Domain.Author
                {
                    AuthorID = 3,
                    AuthorName = "Linh"
                });
            });

            _builder.Entity<BookAuthor>(c =>
            {
                c.HasData(new BookAuthor
                {
                    Id = 1,
                    BookId = 1,
                    AuthorId = 1
                });
                c.HasData(new BookAuthor
                {
                    Id = 2,
                    BookId = 1,
                    AuthorId = 2
                });
            });
            _builder.Entity<Publisher>(d =>
            {
                d.HasData(new Publisher
                {
                    PublisherId = 1,
                    PublisherName = "Adam"

                });
                d.HasData(new Publisher
                {
                    PublisherId = 2,
                    PublisherName = "Eva"
                });
                d.HasData(new Publisher
                {
                    PublisherId = 3,
                    PublisherName = "Smith"
                });
            });

        }
    }
}
