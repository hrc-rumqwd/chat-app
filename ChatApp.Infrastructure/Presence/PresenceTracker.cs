using System.Collections.Concurrent;

namespace ChatApp.Infrastructure.Presence
{
    public class PresenceTracker : IPresenceTracker
    {
        public readonly ConcurrentDictionary<long, HashSet<string>> _onlineUsers;

        public PresenceTracker()
        {
            _onlineUsers = new ConcurrentDictionary<long, HashSet<string>>();
        }

        public Task<bool> ConnectionClosedAsync(long userId, string connectionId)
        {
            bool isLastConnection = false;

            if(_onlineUsers.TryGetValue(userId, out var connections))
            {
                lock(connections)
                {
                    connections.Remove(connectionId);
                    if (connections.Count == 0)
                    {
                        isLastConnection = true;
                        _onlineUsers.TryRemove(userId, out _);
                    }
                }
            }

            return Task.FromResult(isLastConnection);
        }

        public Task<bool> ConnectionOpenedAsync(long userId, string connectionId)
        {
            bool isFirstConnection = false;

            _onlineUsers.AddOrUpdate(userId,
                _ =>
                {
                    isFirstConnection = true;
                    return new HashSet<string> { connectionId };
                },
                (_, connections) =>
                {
                    lock (connections)
                    {
                        connections.Add(connectionId);
                    }
                    return connections;
                }
            );

            return Task.FromResult(isFirstConnection);
        }

        public Task<long[]> GetOnlineUsersAsync()
        {
            return Task.FromResult(_onlineUsers.Keys.ToArray());
        }

        public Task<string[]> GetUserConnectionsAsync(long userId)
        {
            _onlineUsers.TryGetValue(userId, out var connectionIds);
            return Task.FromResult(connectionIds?.ToArray() ?? []);
        }

        public Task<bool> IsUserOnline(long userId)
        {
            return Task.FromResult(_onlineUsers.TryGetValue(userId, out _));
        }
    }
}
