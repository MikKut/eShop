using Catalog.Host.Data.Entities.Interfaces;

namespace Catalog.Host.Data.Entities;

public class CatalogType : ICatalogType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;
}