using System.ComponentModel.DataAnnotations;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Models.Requests;

public class CreateProductRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(400)]
    public string Description { get; set; } = null!;

    [Required]
    [Range(0, 1_000_000)]
    public decimal Price { get; set; }

    public string PictureFileName { get; set; } = null!;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public int CatalogTypeId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public int CatalogBrandId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public int AvailableStock { get; set; }
}