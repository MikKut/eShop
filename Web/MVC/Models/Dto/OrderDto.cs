namespace MVC.Models.Dto
{
    public class OrderDto
    {
        public UserDto User { get; set; }
        public IEnumerable<CatalogItemDto> Items { get; set; }
    }
}
