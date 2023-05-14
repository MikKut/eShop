namespace MVC.ViewModels
{
    public class GroupedItems<T>
    {
        public IEnumerable<T> Data { get; init; } = null!;
    }
}
