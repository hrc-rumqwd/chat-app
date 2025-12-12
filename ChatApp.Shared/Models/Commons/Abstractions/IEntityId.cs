namespace ChatApp.Shared.Models.Commons.Abstractions
{
    public interface IEntityId<TKey>
    {
        TKey Id { get; set; }
    }
}
