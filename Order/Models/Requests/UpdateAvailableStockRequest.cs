using System.ComponentModel.DataAnnotations;

namespace Order.Host.Models.Requests
{
    public class UpdateAvailableStockRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Id { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public int AvailableStock { get; set; }
    }
}
