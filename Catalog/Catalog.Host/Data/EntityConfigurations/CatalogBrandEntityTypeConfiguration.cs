using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data.EntityConfigurations;

public class CatalogBrandEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogBrand>
{
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        _ = builder.ToTable("CatalogBrand");

        _ = builder.HasKey(ci => ci.Id);

        _ = builder.Property(ci => ci.Id)
            .UseHiLo("catalog_brand_hilo")
            .IsRequired();

        _ = builder.Property(cb => cb.Brand)
            .IsRequired()
            .HasMaxLength(100);
    }
}