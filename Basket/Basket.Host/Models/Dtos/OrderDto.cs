using MVC.Models.Dto;

namespace Basket.Host.Models.Dtos
{
    public class OrderDto<T>
    {
        public UserDto User { get; set; }
        public IEnumerable<T> Orders { get; set; }
    }
}