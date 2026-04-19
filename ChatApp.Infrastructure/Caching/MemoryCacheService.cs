using ChatApp.Application.Contracts.Services;
using Microsoft.Extensions.Caching.Memory;

namespace ChatApp.Infrastructure.Caching
{
    public class MemoryCacheService(IMemoryCache cache) : ICacheService
    {
        public T Add<T>(string key, T value)
        {
            return cache.Set(key, value);
        }

        public T AddOrUpdate<T>(string key, T value)
        {
            //if(cache.TryGetValue(key, out T existingValue))
            //{
            //    if (typeof(T).IsAssignableFrom(typeof(IEnumerable<T>)))
            //    {
            //        var existingList = existingValue as IEnumerable<T>;
            //        var newList = value as IEnumerable<T>;
            //        if (existingList != null && newList != null)
            //        {
            //            var combinedList = existingList.Concat(newList).ToList();
            //            return cache.Set(key, (T)(object)combinedList);
            //        }
            //    }

            //}
            return cache.Set(key, value);
        }

        public void Delete(string key)
        {
            cache.Remove(key);
        }

        public T Get<T>(string key)
        {
            return cache.Get<T>(key);
        }

        public Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(cache.Get<T>(key));
        }

        public T Update<T>(string key, T newValue)
        {
            return cache.Set(key, newValue);
        }
    }
}
