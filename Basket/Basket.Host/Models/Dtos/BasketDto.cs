namespace Basket.Host.Models.Dtos
{
    public class BasketDto<T>
        where T : class
    {
        public IEnumerable<T> Data { get; set; } = null!;
    }
}
