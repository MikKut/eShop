using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data.EntityConfigurations;

public class CatalogTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        _ = builder.ToTable("CatalogType");

        _ = builder.HasKey(ci => ci.Id);

        _ = builder.Property(ci => ci.Id)
            .UseHiLo("catalog_type_hilo")
            .IsRequired();

        _ = builder.Property(cb => cb.Type)
            .IsRequired()
            .HasMaxLength(100);
    }
}