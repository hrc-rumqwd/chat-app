namespace ChatApp.Infrastructure.Caching
{
    public interface ICacheService
    {
        T Add<T>(string key, T value);
        T Update<T>(string key, T newValue);
        T AddOrUpdate<T>(string key, T value);
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);
        void Delete(string key);
    }
}
