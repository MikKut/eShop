using System.ComponentModel.DataAnnotations;

namespace Order.Host.Models.Requests
{
    public class PurchaseRequest<T>
         where T : class
    {
        [Required]
        public IEnumerable<T> Data { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int ID { get; set; }
    }
}
