using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class GetByIdRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int ID { get; set; }
    }
}
