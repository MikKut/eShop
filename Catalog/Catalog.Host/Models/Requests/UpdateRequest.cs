namespace Catalog.Host.Models.Requests
{
    public class UpdateRequest<T>
        where T : class
    {
        public int ID { get; set; }
        public T NewData { get; set; }
    }
}
