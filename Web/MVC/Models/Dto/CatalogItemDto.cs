namespace MVC.Models.Dto
{
    public class CatalogItemDto
        : IEquatable<CatalogItemDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; } = null!;
        public string TypeName { get; set; } = null!;
        public decimal Price { get; set; }

        public bool Equals(CatalogItemDto? other)
        {
            return other != null && (ReferenceEquals(this, other) || other.Id == Id);
        }
    }
}
