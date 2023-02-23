namespace Catalog.Host.Services.Interfaces
{
    public interface ICrudService<T>
        where T : class
    {
        Task<int?> AddAsync(T itemToAdd);
        Task<bool> DeleteAsync(T itemToDelete);
        Task<bool> UpdateAsync(int id, T itemToAdd);
    }
}
