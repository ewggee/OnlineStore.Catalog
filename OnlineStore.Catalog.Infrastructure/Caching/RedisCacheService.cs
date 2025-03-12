using OnlineStore.Catalog.Application.Abstractions;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace OnlineStore.Catalog.Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IRedisDatabase _redisDb;
        private readonly TimeSpan _cacheLifetime = TimeSpan.FromHours(1);

        public RedisCacheService(IRedisDatabase redisDb)
        {
            _redisDb = redisDb;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var cachedItem = await _redisDb.GetAsync<T?>(key);
            return cachedItem;
        }

        public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> func, TimeSpan? lifetime = null)
        {
            var cachedItem = await _redisDb.GetAsync<T?>(key);
            if (cachedItem != null)
            {
                return cachedItem;
            }

            var item = await func();
            if (item != null)
            {
                await SetAsync(key, item, lifetime);
            }

            return item;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? lifetime = null)
        {
            if (lifetime != null)
            {
                await _redisDb.AddAsync(key, value, lifetime.Value);
            }
            else
            {
                await _redisDb.AddAsync(key, value, _cacheLifetime);
            }
        }

        public async Task RefreshAsync<T>(string key, T value, TimeSpan? lifetime = null)
        {
            var cachedItem = await _redisDb.GetAsync<T?>(key);
            if (cachedItem != null)
            {
                await _redisDb.RemoveAsync(key);
            }

            await SetAsync(key, value, lifetime);
        }

        public async Task RemoveAsync<T>(string key)
        {
            await _redisDb.RemoveAsync(key);
        }
    }
}
