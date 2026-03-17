namespace ChatApp.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> PaginatedQuery<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            // TODO: Ensure pageIndex is at least 1 and pageSize is positive, or apply a policy to handle invalid values.
            return query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
