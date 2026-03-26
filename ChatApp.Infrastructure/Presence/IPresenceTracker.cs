namespace ChatApp.Infrastructure.Presence
{
    public interface IPresenceTracker
    {
        Task<bool> ConnectionOpenedAsync(long userId, string connectionId);
        Task<bool> ConnectionClosedAsync(long userId, string connectionId);
        Task<bool> IsUserOnline(long userId);
        Task<long[]> GetOnlineUsersAsync(); 
    }
}
