﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entities;

namespace WebAPIAutores
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthorBook>().HasKey(x => new { x.AuthorId, x.BookId });
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AuthorBook> AuthorsBooks { get; set; }
    }
}
