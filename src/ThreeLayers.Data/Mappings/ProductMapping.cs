using ThreeLayers.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreeLayers.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.Property(x => x.Value)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.CreationDate)
            .IsRequired();

        builder.Property(x => x.Active)
            .IsRequired();

        builder.ToTable("Products");
    }
}