using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PosNet.Infrastructure.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name).IsRequired();

            builder.Property(p => p.BrandId).IsRequired();

            builder.Property(p => p.CategoryId).IsRequired();

            builder.Property(p => p.Stock)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(p => p.UnitPrice)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Brand)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
