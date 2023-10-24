using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog.ORM.Models;

namespace Blog.ORM.Context
{
    public class BlogEntityTypeConfiguration : IEntityTypeConfiguration<BlogDetails>
    {
        public void Configure(EntityTypeBuilder<BlogDetails> builder)
        {
            builder.HasMany(c => c.Comments)
                 .WithOne(b => b.Blog)
                 .HasForeignKey(b => b.BlogId)
                 .IsRequired();

            builder.Property(b => b.Title).IsRequired().HasMaxLength(256);

            builder.Property(b => b.BlogContents).HasMaxLength(5000);

            builder.Property(b => b.CategoryId).IsRequired();

            builder.Property(b => b.IsPublished).HasDefaultValue(false);

            builder.Property(b => b.BlogId).IsUnicode();
        }
    }
}
