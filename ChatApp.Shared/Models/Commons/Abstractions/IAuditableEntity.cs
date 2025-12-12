namespace ChatApp.Shared.Models.Commons.Abstractions
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        string? CreatedBy { get; set; }
        string? UpdatedBy { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
