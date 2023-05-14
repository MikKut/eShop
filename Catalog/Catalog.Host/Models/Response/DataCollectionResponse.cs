namespace Catalog.Host.Models.Response
{
    public class DataCollectionResponse<T>
        where T : class
    {
        public IEnumerable<T>? Data { get; set; }
    }
}
