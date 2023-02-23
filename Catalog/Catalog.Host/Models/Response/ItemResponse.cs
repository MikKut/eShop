namespace Catalog.Host.Models.Response
{
    public class ItemResponse<T>
        where T : class
    {
        public T? Data { get; set; } = default(T);
    }
}
