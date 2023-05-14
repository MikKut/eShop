namespace Basket.Host.Models.Dtos
{
    public class CatalogItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; } = null!;
        public string TypeName { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
