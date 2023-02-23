#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class DeleteRequest<T>
        where T : class
    {
        [Required]
        public T Data { get; set; }
    }
}
