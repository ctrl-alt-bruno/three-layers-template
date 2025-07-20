using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using ThreeLayers.Business.Models;

namespace ThreeLayers.Data.Context;

public sealed class MyDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Address> Addresses { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> dbContextOptions) : base(dbContextOptions)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);

        foreach (IMutableProperty property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetProperties()
                         .Where(p => p.ClrType == typeof(string))))
            property.SetMaxLength(100);

        foreach (IMutableForeignKey? relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.SetNull;

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (EntityEntry entry in ChangeTracker.Entries()
                     .Where(entry => entry.Entity.GetType().GetProperty("CreationDate") != null))
        {
            if (entry.State == EntityState.Added)
                entry.Property("CreationDate").CurrentValue = DateTime.UtcNow;

            if (entry.State == EntityState.Modified)
                entry.Property("CreationDate").IsModified = false;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}