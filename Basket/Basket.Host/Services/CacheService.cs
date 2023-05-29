using Basket.Host.Configurations;
using Basket.Host.Services.Interfaces;
using Infrastructure.JsonConverterWrapper;
using StackExchange.Redis;

namespace Basket.Host.Services
{
    public class CacheService : ICacheService
    {
        private readonly ILogger<CacheService> _logger;
        private readonly IRedisCacheConnectionService _redisCacheConnectionService;
        private readonly IJsonConvertWrapper _jsonSerializer;
        private readonly RedisConfig _config;

        public CacheService(
            ILogger<CacheService> logger,
            IRedisCacheConnectionService redisCacheConnectionService,
            IOptions<RedisConfig> config,
            IJsonConvertWrapper jsonSerializer)
        {
            _logger = logger;
            _redisCacheConnectionService = redisCacheConnectionService;
            _jsonSerializer = jsonSerializer;
            _config = config.Value;
        }

        public async Task AddOrUpdateAsync<T>(string key, T value)
        {
            await AddOrUpdateInternalAsync(key, value);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            IDatabase redis = GetRedisDatabase();

            RedisValue serialized = await redis.StringGetAsync(key);
            _logger.LogInformation($"{nameof(GetAsync)}: serialized value: {serialized}. key: {key}. Type: {typeof(T)}");
            T? deserialized = serialized.HasValue ?
                _jsonSerializer.Deserialize<T>(serialized.ToString())!
                : default!;
            _logger.LogInformation($"{nameof(GetAsync)}: Deserialized value is null:{deserialized == null}. The value: {deserialized}");
            return deserialized;
        }

        public async Task ClearCacheByKeyAsync(string key)
        {
            IDatabase redis = GetRedisDatabase();
            bool deleted = await redis.KeyDeleteAsync(key);
            if (deleted)
            {
                _logger.LogInformation($"Redis cache cleared for key {key}");
            }
            else
            {
                _logger.LogInformation($"No Redis cache found for key {key}");
            }
        }

        private async Task AddOrUpdateInternalAsync<T>(string key, T value,
            IDatabase redis = null!, TimeSpan? expiry = null)
        {
            redis ??= GetRedisDatabase();
            expiry ??= _config.CacheTimeout;

            string serialized = _jsonSerializer.Serialize(value!);
            _logger.LogInformation($"{nameof(AddOrUpdateAsync)}: Serialized value {serialized}");

            if (await redis.StringSetAsync(key, serialized, expiry))
            {
                _logger.LogInformation($"{nameof(AddOrUpdateAsync)}: Cached value for key {key} cached");
            }
            else
            {
                _logger.LogInformation($"{nameof(AddOrUpdateAsync)}: Cached value for key {key} updated");
            }
        }

        private IDatabase GetRedisDatabase()
        {
            return _redisCacheConnectionService.Connection.GetDatabase();
        }
    }
}