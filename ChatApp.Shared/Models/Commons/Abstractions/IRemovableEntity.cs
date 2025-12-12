namespace ChatApp.Shared.Models.Commons.Abstractions
{
    public interface IRemovableEntity
    {
        bool IsDeleted { get; set; }
        bool IsActived { get; set; }
    }
}
