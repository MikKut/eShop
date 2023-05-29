using Catalog.Host.Data.Entities;
using Catalog.Host.Data.EntityConfigurations;

namespace Catalog.Host.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<CatalogItem> CatalogItems { get; set; } = null!;

    public DbSet<CatalogBrand> CatalogBrands { get; set; } = null!;

    public DbSet<CatalogType> CatalogTypes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        _ = builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
        _ = builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
        _ = builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
    }
}
