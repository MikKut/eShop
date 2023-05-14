using Catalog.Host.Data.Entities.Interfaces;

namespace Catalog.Host.Data.Entities;

public class CatalogBrand : ICatalogBrand
{
    public int Id { get; set; }

    public string Brand { get; set; } = null!;
}