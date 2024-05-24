using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using Todo.ServiceContracts;

namespace Todo.Services
{
    public class CachingService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CachingService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public bool Get<T>(string key, out T value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public void Set<T>(string key, T value)
        {
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(120))
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(600));

            _cache.Set(key, value, cacheEntryOptions) ;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

       
    }
}
