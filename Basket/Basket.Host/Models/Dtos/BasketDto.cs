namespace Basket.Host.Models.Dtos
{
    public class BasketDto<T>
    {
        public IEnumerable<T> Data { get; set; } = null!;
    }
}
