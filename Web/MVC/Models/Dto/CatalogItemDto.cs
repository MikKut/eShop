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
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Id == this.Id;
        }
    }
}
