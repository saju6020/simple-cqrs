using Blog.ORM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
                .UseSqlServer("Data Source=DESKTOP-KTI6J0L\\SQLEXPRESS;Initial Catalog=BLOG;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

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
