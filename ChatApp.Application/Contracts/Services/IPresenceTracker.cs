namespace ChatApp.Application.Contracts.Services
{
    public interface IPresenceTracker
    {
        Task<bool> ConnectionOpenedAsync(long userId, string connectionId);
        Task<bool> ConnectionClosedAsync(long userId, string connectionId);
        Task<bool> IsUserOnline(long userId);
        Task<long[]> GetOnlineUsersAsync();
        Task<string[]> GetUserConnectionsAsync(long userId);
    }
}
