namespace Catalog.Host.Models.Requests
{
    public class UpdateProductRequest
    {
        public int ID { get; set; }
        public string NewName { get; set; } = null!;

        public string NewDescription { get; set; } = null!;

        public decimal NewPrice { get; set; }

        public string NewPictureFileName { get; set; } = null!;

        public int NewCatalogTypeId { get; set; }

        public int NewCatalogBrandId { get; set; }

        public int NewAvailableStock { get; set; }
    }
}
