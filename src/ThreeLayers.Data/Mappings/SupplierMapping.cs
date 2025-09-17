using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeLayers.Business.Models;

namespace ThreeLayers.Data.Mappings;

public class SupplierMapping : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

        builder.Property(x => x.Document).IsRequired().HasMaxLength(20);

        // 1 : 1 => Supplier : Address
        builder.HasOne(x => x.Address).WithOne(x => x.Supplier);

        // 1 : N => Supplier : Products
        builder.HasMany(x => x.Products).WithOne(x => x.Supplier).HasForeignKey(x => x.SupplierId);

        builder.ToTable("Suppliers");
    }
}
