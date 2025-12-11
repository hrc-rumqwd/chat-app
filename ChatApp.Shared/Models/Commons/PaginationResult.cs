namespace ChatApp.Shared.Models.Commons
{
    public class PaginationResult : PaginationBase
    {
        public int TotalItems { get; set; }
        public int PageCount => TotalItems / PageSize;
    }

    public class PaginationResult<T> : PaginationResult
    {
        public IEnumerable<T> Items { get; set; } = [];
    }
}
