using ChatApp.Application.Contracts.Services;

namespace ChatApp.Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        public T Add<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public T AddOrUpdate<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public T Update<T>(string key, T newValue)
        {
            throw new NotImplementedException();
        }
    }
}
