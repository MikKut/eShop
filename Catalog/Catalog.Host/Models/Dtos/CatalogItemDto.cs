namespace Catalog.Host.Models.Dtos;

public class CatalogItemDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string PictureUrl { get; set; }

    public int CatalogTypeId { get; set; }

    public int CatalogBrandId { get; set; }

    public CatalogTypeDto CatalogType { get; set; }

    public CatalogBrandDto CatalogBrand { get; set; }

    public int AvailableStock { get; set; }
}
