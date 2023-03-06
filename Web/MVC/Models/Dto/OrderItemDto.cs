using MVC.ViewModels.Models;

namespace MVC.Models.Dto
{
    public class OrderItemDto
    {
        public UserDto User { get; set; }
        public CatalogItemDto Item { get; set; }
    }
}
