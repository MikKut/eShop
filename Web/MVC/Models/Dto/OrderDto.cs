namespace MVC.Models.Dto
{
    public class OrderDto<T>
    {
        public UserDto User { get; set; }
        public IEnumerable<T> Orders { get; set; }
    }
}
