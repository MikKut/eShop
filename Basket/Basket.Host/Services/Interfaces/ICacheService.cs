namespace Basket.Host.Services.Interfaces
{
    public interface ICacheService
    {
        Task AddOrUpdateAsync<T>(string key, T value);
        Task ClearCacheByKeyAsync(string v);
        Task<T> GetAsync<T>(string key);
    }
}