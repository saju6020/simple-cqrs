using Blog.ORM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Blog.ORM.Context
{
    public class BlogDbContext : DbContext
    {
        private readonly IConfiguration Configuration;

        public BlogDbContext() { }

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder                
                .UseNpgsql("Host=localhost; Database=SimpleCQRS; Username=postgres; Password=postgres");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BlogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());


        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<BlogDetails> Blogs { get; set; }

        public DbSet<Comment> Commets { get; set; }

        public DbSet<Tag> Tags { get; set; }

    }
}
