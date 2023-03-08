using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class CreateRequest<T>
    {
        [Required]
        public T Data { get; set; }
    }
}
