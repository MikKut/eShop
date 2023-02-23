namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICrudRepository<T>
        where T : class
    {
        Task<int?> AddAsync(T itemToAdd);
        Task<bool> DeleteAsync(T itemToDelete);
        Task<bool> UpdateAsync(int id, T itemToAdd);
    }
}
