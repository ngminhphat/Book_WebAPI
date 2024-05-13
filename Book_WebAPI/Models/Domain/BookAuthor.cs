﻿using System.ComponentModel.DataAnnotations;

namespace Book_WebAPI.Models.Domain
{
    public class BookAuthor
    {
        [Key]
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}
