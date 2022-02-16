using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework.Context
{
    public class BooksContext : DbContext
    {
        public BooksContext(DbContextOptions<BooksContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable(nameof(Book), "Book");
            modelBuilder.Entity<Category>().ToTable(nameof(Category), "Category");
            modelBuilder.Entity<OperationClaim>().ToTable(nameof(OperationClaim), "User");
            modelBuilder.Entity<User>().ToTable(nameof(User), "User");
            modelBuilder.Entity<UserOperationClaim>().ToTable(nameof(UserOperationClaim), "User");
        }
    }
}
