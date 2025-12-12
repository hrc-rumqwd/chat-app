using ChatApp.Shared.Models.Commons.Abstractions;

namespace ChatApp.Shared.Models.Commons
{
    public class BaseEntity<TKey> : IEntityId<TKey>
    {
        public TKey Id { get; set; }
    }
}
