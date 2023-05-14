using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data.EntityConfigurations;

public class CatalogItemEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        _ = builder.ToTable("Catalog");

        _ = builder.Property(ci => ci.Id)
            .UseHiLo("catalog_hilo")
            .IsRequired();

        _ = builder.Property(ci => ci.Name)
            .IsRequired(true)
            .HasMaxLength(50);

        _ = builder.Property(ci => ci.Price)
            .IsRequired(true);

        _ = builder.Property(ci => ci.PictureFileName)
            .IsRequired(false);

        _ = builder.HasOne(ci => ci.CatalogBrand)
            .WithMany()
            .HasForeignKey(ci => ci.CatalogBrandId);

        _ = builder.HasOne(ci => ci.CatalogType)
            .WithMany()
            .HasForeignKey(ci => ci.CatalogTypeId);
    }
}