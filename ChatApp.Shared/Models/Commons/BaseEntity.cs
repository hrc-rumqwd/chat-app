using ChatApp.Shared.Models.Commons.Abstractions;

namespace ChatApp.Shared.Models.Commons
{
    public class BaseEntity<TKey> : IEntityId<TKey>, IAuditableEntity, IRemovableEntity
    {
        public TKey Id { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActived { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
